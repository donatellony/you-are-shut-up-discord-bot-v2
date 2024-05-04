using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using YouAreShutUp.DiscordBot.Discord.Configuration.Options;

namespace YouAreShutUp.DiscordBot.Discord.Modules
{
    public class BaseCommands : ModuleBase<ShardedCommandContext>
    {
        protected readonly IOptions<DiscordFriendsSettings> _friendsSettings;

        public BaseCommands(IOptions<DiscordFriendsSettings> friendsSettings)
        {
            _friendsSettings = friendsSettings;
        }

        public SocketGuildUser? GetSocketGuildUserById(ulong id)
        {
            return Context.Guild.GetUser(id);
        }
    }
}
