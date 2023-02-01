using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenMod.API.Plugins;
using OpenMod.API.Permissions;
using OpenMod.Unturned.Plugins;
using System;

[assembly: PluginMetadata("EvolutionPlugins.OpenDeathMessages", Author = "EvolutionPlugins", DisplayName = "Open Death Messages",
    Website = "https://discord.gg/5MT2yke")]

namespace EvolutionPlugins.OpenDeathMessages
{
    public class OpenDeathMessages : OpenModUnturnedPlugin
    {
        private readonly ILogger<OpenDeathMessages> m_Logger;
        private readonly IPermissionRegistry m_PermissionRegistry;

        public OpenDeathMessages(IServiceProvider serviceProvider, ILogger<OpenDeathMessages> logger, IPermissionRegistry permissionRegistry) : base(serviceProvider)
        {
            m_Logger = logger;
            m_PermissionRegistry = permissionRegistry;
        }

        protected override UniTask OnLoadAsync()
        {
            m_Logger.LogInformation($"Made with <3 by {Author}");
            m_Logger.LogInformation("https://github.com/evolutionplugins \\ https://github.com/diffoz");
            m_Logger.LogInformation($"Support discord: {Website}");
            m_PermissionRegistry.RegisterPermission(this, "DeathPermission", "Default Permission for sending death messages");
            m_PermissionRegistry.RegisterPermission(this, "English", "English permission for sending death messages");
            m_PermissionRegistry.RegisterPermission(this, "Spanish", "Spanish permission for sending death messages");

            return UniTask.CompletedTask;
        }
    }
}
