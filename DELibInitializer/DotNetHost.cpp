#include "DotNetHost.h"

namespace
{
    //--------------------------------------------------------------------
    // Loads hostfxr.dll using Microsoft's nethost library.
    //--------------------------------------------------------------------
    bool LoadHostFxrLibrary(HMODULE& module)
    {
        wchar_t buffer[MAX_PATH];

        size_t bufferSize = sizeof(buffer) / sizeof(wchar_t);

        int rc = get_hostfxr_path(buffer, &bufferSize, nullptr);

        if (rc != 0)
            return false;

        module = ::LoadLibraryW(buffer);

        return module != nullptr;
    }

    template<typename T>
    T GetExport(HMODULE module, const char* name)
    {
        return reinterpret_cast<T>(
            ::GetProcAddress(module, name));
    }
}

DotNetHost::DotNetHost()
{
}

DotNetHost::~DotNetHost()
{
    Shutdown();
}

bool DotNetHost::IsInitialized() const
{
    return m_initialized;
}

bool DotNetHost::LoadHostFxr()
{
    if (!LoadHostFxrLibrary(m_hostfxrModule))
        return false;

    m_initialize =
        GetExport<hostfxr_initialize_for_runtime_config_fn>(
            m_hostfxrModule,
            "hostfxr_initialize_for_runtime_config");

    m_getDelegate =
        GetExport<hostfxr_get_runtime_delegate_fn>(
            m_hostfxrModule,
            "hostfxr_get_runtime_delegate");

    m_close =
        GetExport<hostfxr_close_fn>(
            m_hostfxrModule,
            "hostfxr_close");

    if (!m_initialize)
        return false;

    if (!m_getDelegate)
        return false;

    if (!m_close)
        return false;

    return true;
}

void DotNetHost::Shutdown()
{
    if (m_hostfxrHandle)
    {
        m_close(m_hostfxrHandle);
        m_hostfxrHandle = nullptr;
    }

    if (m_hostfxrModule)
    {
        FreeLibrary(m_hostfxrModule);
        m_hostfxrModule = nullptr;
    }

    m_loadAssembly = nullptr;

    m_initialized = false;
}

int DotNetHost::Initialize(const std::wstring& runtimeConfigPath)
{
    if (m_initialized)
        return 1;

    if (!LoadHostFxr())
        return -1;

    if (!LoadAssemblyDelegate(runtimeConfigPath))
        return 0;

    m_initialized = true;

    return 1;
}

bool DotNetHost::LoadAssemblyDelegate(const std::wstring& runtimeConfigPath)
{
    hostfxr_handle context = nullptr;

    int rc = m_initialize(
        runtimeConfigPath.c_str(),
        nullptr,
        &context);

    if (rc != 0 || context == nullptr)
        return false;

    void* delegate = nullptr;

    rc = m_getDelegate(
        context,
        hdt_load_assembly_and_get_function_pointer,
        &delegate);

    // We don't need the initialization context anymore.
    m_close(context);

    if (rc != 0 || delegate == nullptr)
        return false;

    m_loadAssembly =
        reinterpret_cast<load_assembly_and_get_function_pointer_fn>(delegate);

    return true;
}
