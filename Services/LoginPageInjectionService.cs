using System.Reflection;
using System.Runtime.Loader;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Dreamstreaming.TermsPlugin.Services;

public class LoginPageInjectionService
{
    private const string ScriptId =
        "dreamstreaming-terms-login-links";

    private const string InjectorAssemblyName =
        "Jellyfin.Plugin.JavaScriptInjector";

    private const string InjectorInterfaceName =
        "Jellyfin.Plugin.JavaScriptInjector.PluginInterface";

    private const string EmbeddedScriptName =
        "Dreamstreaming.TermsPlugin.Web.loginlinks.js";

    private readonly ILogger<LoginPageInjectionService> _logger;

    public LoginPageInjectionService(
        ILogger<LoginPageInjectionService> logger)
    {
        _logger = logger;
    }


    // ============================================================
    // REGISTER
    // ============================================================

    public bool RegisterScript()
    {
        try
        {
            var plugin =
                Plugin.Instance;

            if (plugin is null)
            {
                _logger.LogWarning(
                    "Dreamstreaming Terms plugin instance is not available."
                );

                return false;
            }


            var injectorAssembly =
                FindInjectorAssembly();

            if (injectorAssembly is null)
            {
                _logger.LogDebug(
                    "JavaScript Injector assembly is not loaded yet."
                );

                return false;
            }


            var pluginInterfaceType =
                injectorAssembly.GetType(
                    InjectorInterfaceName
                );

            if (pluginInterfaceType is null)
            {
                _logger.LogWarning(
                    "JavaScript Injector PluginInterface could not be found."
                );

                return false;
            }


            var registerMethod =
                pluginInterfaceType.GetMethod(
                    "RegisterScript",
                    BindingFlags.Public |
                    BindingFlags.Static
                );

            if (registerMethod is null)
            {
                _logger.LogWarning(
                    "JavaScript Injector RegisterScript method could not be found."
                );

                return false;
            }


            var parameters =
                registerMethod.GetParameters();

            if (parameters.Length != 1)
            {
                _logger.LogWarning(
                    "JavaScript Injector RegisterScript has an unexpected parameter count."
                );

                return false;
            }


            var script =
                LoadEmbeddedLoginScript();

            if (string.IsNullOrWhiteSpace(script))
            {
                _logger.LogError(
                    "Dreamstreaming Terms loginlinks.js could not be loaded."
                );

                return false;
            }


            var payloadData =
                new
                {
                    id =
                        ScriptId,

                    name =
                        "Dreamstreaming Terms - Login Links",

                    script,

                    enabled =
                        true,

                    requiresAuthentication =
                        false,

                    pluginId =
                        plugin.Id.ToString(),

                    pluginName =
                        plugin.Name,

                    pluginVersion =
                        plugin.Version.ToString()
                };


            var json =
                JsonSerializer.Serialize(
                    payloadData
                );


            var jobjectType =
                parameters[0].ParameterType;


            var parseMethod =
                jobjectType.GetMethod(
                    "Parse",
                    BindingFlags.Public |
                    BindingFlags.Static,
                    binder: null,
                    types:
                    [
                        typeof(string)
                    ],
                    modifiers: null
                );


            if (parseMethod is null)
            {
                _logger.LogWarning(
                    "Could not find JObject.Parse on the JavaScript Injector payload type."
                );

                return false;
            }


            var payload =
                parseMethod.Invoke(
                    null,
                    new object[]
                    {
                        json
                    }
                );


            if (payload is null)
            {
                _logger.LogWarning(
                    "Could not create JavaScript Injector registration payload."
                );

                return false;
            }


            var result =
                registerMethod.Invoke(
                    null,
                    new[]
                    {
                        payload
                    }
                );


            if (result is bool success &&
                success)
            {
                _logger.LogInformation(
                    "Dreamstreaming Terms login script registered with JavaScript Injector."
                );

                return true;
            }


            _logger.LogWarning(
                "JavaScript Injector did not accept the Dreamstreaming Terms login script."
            );

            return false;
        }
        catch (TargetInvocationException ex)
        {
            _logger.LogWarning(
                ex.InnerException ?? ex,
                "JavaScript Injector threw an error while registering Dreamstreaming Terms."
            );

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                ex,
                "Failed to register Dreamstreaming Terms with JavaScript Injector."
            );

            return false;
        }
    }


    // ============================================================
    // UNREGISTER
    // ============================================================

    public void UnregisterScript()
    {
        try
        {
            var injectorAssembly =
                FindInjectorAssembly();

            if (injectorAssembly is null)
            {
                return;
            }


            var pluginInterfaceType =
                injectorAssembly.GetType(
                    InjectorInterfaceName
                );

            if (pluginInterfaceType is null)
            {
                return;
            }


            var unregisterMethod =
                pluginInterfaceType.GetMethod(
                    "UnregisterScript",
                    BindingFlags.Public |
                    BindingFlags.Static
                );


            if (unregisterMethod is null)
            {
                return;
            }


            var result =
                unregisterMethod.Invoke(
                    null,
                    new object[]
                    {
                        ScriptId
                    }
                );


            if (result is bool success &&
                success)
            {
                _logger.LogInformation(
                    "Dreamstreaming Terms login script unregistered."
                );
            }
        }
        catch (Exception ex)
        {
            _logger.LogDebug(
                ex,
                "Dreamstreaming Terms login script could not be unregistered."
            );
        }
    }


    // ============================================================
    // FIND JAVASCRIPT INJECTOR
    // ============================================================

    private static Assembly? FindInjectorAssembly()
    {
        foreach (var loadContext in
                 AssemblyLoadContext.All)
        {
            foreach (var assembly in
                     loadContext.Assemblies)
            {
                var assemblyName =
                    assembly.GetName().Name;

                if (string.Equals(
                        assemblyName,
                        InjectorAssemblyName,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return assembly;
                }
            }
        }


        return null;
    }


    // ============================================================
    // LOAD EMBEDDED LOGIN SCRIPT
    // ============================================================

    private string? LoadEmbeddedLoginScript()
    {
        try
        {
            var assembly =
                typeof(LoginPageInjectionService)
                    .Assembly;


            using var stream =
                assembly.GetManifestResourceStream(
                    EmbeddedScriptName
                );


            if (stream is null)
            {
                _logger.LogError(
                    "Embedded resource {ResourceName} was not found.",
                    EmbeddedScriptName
                );

                return null;
            }


            using var reader =
                new StreamReader(
                    stream
                );


            return reader.ReadToEnd();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to read embedded loginlinks.js."
            );

            return null;
        }
    }
}