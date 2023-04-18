using Cysharp.Threading.Tasks;
using EvolutionPlugins.OpenDeathMessages.API;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenMod.API.Ioc;
using OpenMod.UnityEngine.Extensions;
using OpenMod.API.Permissions;
using OpenMod.Unturned.Players;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using Steamworks;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace EvolutionPlugins.OpenDeathMessages.Services
{
    [PluginServiceImplementation(Lifetime = ServiceLifetime.Singleton)]
    public class PlayerMessager : IPlayerMessager
    {
        private readonly DisplayType m_DisplayType;
        private readonly IUnturnedUserDirectory m_UnturnedUserDirectory;
        private readonly HashSet<CSteamID> m_GroupOnlyMessagger = new();
        private readonly IPermissionChecker m_PermissionChecker;

        public PlayerMessager(IConfiguration configuration, IUnturnedUserDirectory unturnedUserDirectory, IPermissionChecker permissionChecker)
        {
        m_DisplayType = (DisplayType)(configuration.GetValue("defaultDisplay:groupDeath", false) ? 1 : 0);
        m_UnturnedUserDirectory = unturnedUserDirectory;
        m_PermissionChecker = permissionChecker;
        }

        private bool ShouldMessageToGroup(UnturnedPlayer player) => m_GroupOnlyMessagger.Contains(player.SteamId) || m_DisplayType is DisplayType.Group;

        public Task ChangeDisplayTypeAsync(UnturnedPlayer player, DisplayType displayType)
        {
            if (displayType is DisplayType.Group)
            {
                m_GroupOnlyMessagger.Add(player.SteamId);
                return Task.CompletedTask;
            }

            m_GroupOnlyMessagger.Remove(player.SteamId);
            return Task.CompletedTask;
        }

        public async Task SendMessageGlobalOrGroupAsync(UnturnedPlayer player, string message, string? iconUrl, Color color)
        {
            var unityColor = color.ToUnityColor();
            await UniTask.SwitchToMainThread();

            foreach (var user in m_UnturnedUserDirectory.GetOnlineUsers())
                {
                    if (await m_PermissionChecker.CheckPermissionAsync(user, "English") is PermissionGrantResult.Grant)
                    {
                        await user.PrintMessageAsync(message);
                    }
                }

                return;
        }
    }
}