using Discord;
using Discord.Commands;
using Discord.WebSocket;
using YouAreShutUpDiscordBot.Discord.Configuration.Options;
using YouAreShutUpDiscordBot.Discord.Services.CommandHandler;

namespace YouAreShutUpDiscordBot.Discord.Configuration.Extensions;

internal static class DiscordConfigurationExtensions
{
    internal static WebApplicationBuilder AddDiscord(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<DiscordBotSettings>(builder.Configuration.GetSection(DiscordBotSettings.Key));
        builder.Services.Configure<DiscordFriendsSettings>(builder.Configuration.GetSection(DiscordFriendsSettings.Key));

        builder.Services.AddSingleton<CommandService>(_ =>
            new CommandService(new CommandServiceConfig { CaseSensitiveCommands = false }));

        builder.Services.AddSingleton<DiscordShardedClient>(_ =>
        {
            var client = new DiscordShardedClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | /*GatewayIntents.MessageContent | */ GatewayIntents.All
            });

            return client;
        });

        builder.Services.AddHostedService<DiscordHostedService>();

        return builder;
    }
}