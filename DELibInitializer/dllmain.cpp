#include <Windows.h>
#include <string>
#include <cstdio>
#include <cstdarg>
#include "DotNetHost.h"

namespace
{
    FILE* g_LogFile = nullptr;

    std::wstring GetParentPath(const std::wstring& path)
    {
        size_t pos = path.find_last_of(L"/\\");
        return pos == std::wstring::npos ? L"" : path.substr(0, pos);
    }

    std::wstring JoinPath(const std::wstring& dir, const std::wstring& file)
    {
        if (dir.empty())
            return file;
        wchar_t lastChar = dir.back();
        return (lastChar == L'/' || lastChar == L'\\') ? dir + file : dir + L"\\" + file;
    }

    std::wstring GetModuleDirectory(HMODULE module)
    {
        wchar_t path[MAX_PATH];
        DWORD len = GetModuleFileNameW(module, path, MAX_PATH);
        if (len == 0 || len == MAX_PATH)
            return L"";
        return GetParentPath(std::wstring(path, len));
    }

    void OpenLogFile(const std::wstring& directory)
    {
        if (g_LogFile)
            return;
        std::wstring logPath = JoinPath(directory, L"DELibInitializer.log");
        g_LogFile = _wfopen(logPath.c_str(), L"w");
    }

    void Log(const wchar_t* fmt, ...)
    {
        wchar_t buf[512];
        va_list args;
        va_start(args, fmt);
        vswprintf(buf, 512, fmt, args);
        va_end(args);

        OutputDebugStringW(buf);

        if (g_LogFile)
        {
            fwprintf(g_LogFile, L"%s", buf);
            fflush(g_LogFile);
        }
    }

    using InitializeManagedFn = int(CORECLR_DELEGATE_CALLTYPE*)(const wchar_t*);

    DotNetHost g_Host;

    // Runs the whole managed init sequence on its own thread. hostfxr/CoreCLR
    // initialization touches the loader and pumps callbacks internally, doing
    // this directly inside DllMain (which holds the loader lock) risks a deadlock.
    DWORD WINAPI InitThreadProc(LPVOID param)
    {
        HMODULE module = reinterpret_cast<HMODULE>(param);

        std::wstring directory = GetModuleDirectory(module);
        if (directory.empty())
        {
            OutputDebugStringW(L"[DELibInitializer] Failed to resolve module directory\n");
            ExitThread(1);
        }

        OpenLogFile(directory);
        Log(L"[DELibInitializer] Starting from %s\n", directory.c_str());

        std::wstring assemblyPath = JoinPath(directory, L"DELibrary.NET.dll");
        std::wstring runtimeConfigPath = JoinPath(directory, L"DELibrary.NET.runtimeconfig.json");

        if (GetFileAttributesW(runtimeConfigPath.c_str()) == INVALID_FILE_ATTRIBUTES)
        {
            Log(L"[DELibInitializer] Missing runtimeconfig.json at %s\n", runtimeConfigPath.c_str());
            ExitThread(1);
        }

        int code = g_Host.Initialize(runtimeConfigPath);

        if (code != 1)
        {
            switch (code)
            {
            case -1:
                Log(L"[DELibInitializer] Failed to load HostFXR\n");
                break;
            case 0:
                Log(L"[DELibInitializer] Failed to load Assembly delegate\n");
                break;
            }

            ExitThread(1);
        }

        InitializeManagedFn initialize = nullptr;

        if (!g_Host.GetManagedFunction(
            assemblyPath,
            L"DragonEngineLibrary.Entry, DELibrary.NET",
            L"Initialize",
            L"DragonEngineLibrary.InitializeDelegate, DELibrary.NET",
            initialize))
        {
            Log(L"[DELibInitializer] Failed to resolve DragonEngineLibrary.Entry.Initialize\n");
            ExitThread(1);
        }

        int result = initialize(directory.c_str());
        Log(L"[DELibInitializer] DragonEngineLibrary.Entry.Initialize returned %d\n", result);

        ExitThread(0);
        return 0;
    }
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD reasonForCall, LPVOID)
{
    if (reasonForCall == DLL_PROCESS_ATTACH)
    {
        // Note: intentionally never freed via FreeLibraryAndExitThread. This module
        // has to stay mapped for the life of the process, cimgui.dll/Y7Internal.dll/
        // DELibrary.NET.dll all stay resident the same way for the whole session.
        CloseHandle(CreateThread(nullptr, 0, InitThreadProc, hModule, 0, nullptr));
    }

    return TRUE;
}
