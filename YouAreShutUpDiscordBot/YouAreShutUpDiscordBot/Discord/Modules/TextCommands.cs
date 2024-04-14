using System.Text;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Options;
using YouAreShutUpDiscordBot.Discord.Configuration.Options;
using RunMode = Discord.Commands.RunMode;

namespace YouAreShutUpDiscordBot.Discord.Modules;

public class TextCommands : BaseCommands
{
    public TextCommands(IOptions<DiscordFriendsSettings> friendsSettings) : base(friendsSettings)
    {
    }
    [Command("hello", RunMode = RunMode.Async)]
    public async Task Hello()
    {
        await Context.Message.ReplyAsync($"Hello {Context.User.Username}. Nice to meet you!");
    }

    [Command("denis", RunMode = RunMode.Async)]
    public async Task Denis()
    {
        await Context.Message.ReplyAsync($"Hello {Context.User.Username}. I do agree that <@{_friendsSettings.Value.DenisId}> is a bitch!");
    }

    [Command("simpReminder", RunMode = RunMode.Async)]
    public async Task SimpReminder()
    {
        await Context.Message.ReplyAsync($"Yes, {Context.User.Mention}. I do agree that <@{_friendsSettings.Value.FirsId}> is a simp!");
        var firs = Context.Client.GetUser(_friendsSettings.Value.FirsId);

        if (firs is null) return;

        string simpTxt = "Ty simp!!!";

        StringBuilder textToSend = new StringBuilder();
        for (int i = 0; i < simpTxt.Length; i++)
        {
            textToSend.Append(simpTxt[i]);
            await firs.SendMessageAsync(textToSend.ToString());
        }

        textToSend.Clear();

        for (int i = simpTxt.Length - 1; i > 0; i--)
        {
            await firs.SendMessageAsync(simpTxt.Substring(0, i));
        }

    }
}