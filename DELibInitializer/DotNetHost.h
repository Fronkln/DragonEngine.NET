#pragma once

#include <Windows.h>
#include <string>

// Statically link nethost so DELibInitializer.asi has no runtime dependency on
// nethost.dll. Some ASI loaders LoadLibrary plugins with a restricted search
// scope that doesn't include the plugin's own directory, which broke resolution
// of a co-located nethost.dll (LoadLibrary failed with error 126).
#define NETHOST_USE_AS_STATIC
#include <nethost.h>
#include <hostfxr.h>
#include <coreclr_delegates.h>

#pragma comment(lib, "libnethost.lib")

class DotNetHost
{
public:

    DotNetHost();
    ~DotNetHost();

    // Starts CoreCLR using the specified runtimeconfig.json.
    int Initialize(const std::wstring& runtimeConfigPath);

    // Returns whether the runtime has been initialized.
    bool IsInitialized() const;

    // Releases the runtime.
    void Shutdown();

    // Looks up any static managed method and returns its native function pointer.
    template<typename T>
    bool GetManagedFunction(
        const std::wstring& assemblyPath,
        const std::wstring& typeName,
        const std::wstring& methodName,
        const std::wstring& delegateTypeName,
        T& function)
    {
        if (!m_initialized || !m_loadAssembly)
            return false;

        void* delegate = nullptr;

        int rc = m_loadAssembly(
            assemblyPath.c_str(),
            typeName.c_str(),
            methodName.c_str(),
            delegateTypeName.empty() ? nullptr : delegateTypeName.c_str(),
            nullptr,
            &delegate);

        if (rc != 0 || delegate == nullptr)
            return false;

        function = reinterpret_cast<T>(delegate);

        return true;
    }

private:

    bool LoadHostFxr();
    bool LoadAssemblyDelegate(const std::wstring& runtimeConfigPath);

private:

    bool m_initialized = false;

    HMODULE m_hostfxrModule = nullptr;

    hostfxr_handle m_hostfxrHandle = nullptr;

    hostfxr_initialize_for_runtime_config_fn
        m_initialize = nullptr;

    hostfxr_get_runtime_delegate_fn
        m_getDelegate = nullptr;

    hostfxr_close_fn
        m_close = nullptr;

    load_assembly_and_get_function_pointer_fn
        m_loadAssembly = nullptr;
};
