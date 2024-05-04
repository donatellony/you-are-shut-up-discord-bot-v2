using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using YouAreShutUp.DiscordBot.Discord.Configuration.Options;

namespace YouAreShutUp.DiscordBot.Discord.Modules
{
    public class VoiceCommands : BaseCommands
    {
        public VoiceCommands(IOptions<DiscordFriendsSettings> friendsSettings) : base(friendsSettings)
        {
        }

        [Command("muteDenis", RunMode = RunMode.Async)]
        public async Task MuteDenis()
        {
            Random rand = new();
            int number = rand.Next(0, 101);

            var denis = Context.Guild.GetUser(_friendsSettings.Value.DenisId);
            bool selfMute = Context.Message.Author.Id == _friendsSettings.Value.DenisId;
            bool isDenisOnline = denis?.VoiceChannel != null;

            if (denis is not null &&
                (selfMute || (isDenisOnline && number <= 50))) // ne zarandomilo ili denis ne w seti
            {
                if (!denis.IsMuted)
                {
                    await Context.Message.ReplyAsync($"Zakroi ebalo {denis.Mention}");
                }
                else
                {
                    await Context.Message.ReplyAsync($"Otkroi ebalo {denis.Mention}");
                }

                await denis.ModifyAsync(func => { func.Mute = !denis.IsMuted; });
            }
            else
            {
                var kickMessage = "Ne powezlo, eblan";
                var trigger = Context.Guild.GetUser(Context.Message.Author.Id);
                await Context.Message.ReplyAsync($"{kickMessage} {Context.Message.Author.Mention}");
                await trigger.ModifyAsync(func => { func.Channel = null; });
            }
        }

        [Command("wakeUpJonsee", RunMode = RunMode.Async)]
        public async Task WakeUpJonsee(short transitionQuantity = 10)
        {
            SocketGuildUser? jonsee = GetSocketGuildUserById(_friendsSettings.Value.JonseeId);
            ulong? startChannelId = jonsee?.VoiceChannel?.Id;

            if (jonsee is null || !startChannelId.HasValue) // Jonsee isnt connected to the voice channel
                return;

            ulong channelIdToMoveTo;
            for (int i = 0; i < transitionQuantity; i++)
            {
                channelIdToMoveTo = (jonsee.VoiceChannel.Id == _friendsSettings.Value.MainChannelId)
                    ? _friendsSettings.Value.AdditionalChannelId
                    : _friendsSettings.Value.MainChannelId;

                await jonsee.ModifyAsync(func => { func.ChannelId = channelIdToMoveTo; });
            }

            // In the end, return jonsee to the main channel
            await jonsee.ModifyAsync(func => { func.ChannelId = _friendsSettings.Value.MainChannelId; });

            await Context.Message.ReplyAsync($"Wake the fuck up, {jonsee.Mention}");
        }
    }
}