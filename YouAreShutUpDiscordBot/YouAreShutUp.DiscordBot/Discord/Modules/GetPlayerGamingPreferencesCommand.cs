using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MassTransit;
using YouAreShutUp.Contracts.GetPlayerGamingPreferences;
using YouAreShutUp.DiscordBot.Discord.Cache;

namespace YouAreShutUp.DiscordBot.Discord.Modules;

public class GetPlayerGamingPreferencesCommand : ModuleBase<ShardedCommandContext>
{
    private readonly IBus _bus;
    private readonly IDiscordMessageCache<IUserMessage, ulong> _messageCache;

    public GetPlayerGamingPreferencesCommand(IBus bus, IDiscordMessageCache<IUserMessage, ulong> messageCache)
    {
        _bus = bus;
        _messageCache = messageCache;
    }

    [Command("GetPlayerGamingPreferences", RunMode = RunMode.Async)]
    public async Task GetPlayerGamingPreferences(string? steamIdString)
    {
        // Probably not the best way to get parameters, but it would work for now :)
        if (!TryParseSteamId(steamIdString, Context.Message.Content, out var steamId))
        {
            await Context.Message.ReplyAsync("Could not parse the provided SteamId!");
            return;
        }

        _messageCache.SetMessage(Context.Message);
        await _bus.Publish(new GetPlayerGamingPreferencesRequest
            { SteamPlayerId = steamId, ExternalMessageId = Context.Message.Id });
    }

    private static bool TryParseSteamId(string? steamIdString, string message, out ulong steamId)
    {
        try
        {
            return ulong.TryParse(steamIdString ?? message.Split(" ")[1], out steamId);
        }
        catch
        {
            steamId = 0;
            return false;
        }
    }
}