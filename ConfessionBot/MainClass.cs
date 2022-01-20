using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConfessionBot
{
    public class MainClass
    {
        private string _botToken = botToken.cock;

        private ServiceProvider _services;
        public DiscordSocketClient _discord;
        private CommandService _commands;

        private Dictionary<ulong, DateTime> _lastZapped = new Dictionary<ulong, DateTime>();
        private Dictionary<ulong, IAudioClient> _connections = new Dictionary<ulong, IAudioClient>();

        public async Task Run()
        {
            _services = ConfigureServices();
            _discord = _services.GetRequiredService<DiscordSocketClient>();
            _commands = new CommandService();
            _discord.Log += LogAsync;

            _discord.MessageReceived += MessageReceived;

            await _discord.LoginAsync(TokenType.Bot, _botToken);
            await _discord.StartAsync();

            await _services.GetRequiredService<CommandHandlingService>().InitializeAsync();

            await _discord.SetGameAsync("DM me with !help");

            // Keep the thing running...
            while (true)
            {
                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }

        private async Task MessageReceived(SocketMessage arg)
        {
            if (arg.Channel.Name.StartsWith("@Vendell"))
            {
                //await _discord.GetGuild(788977623039606794).GetTextChannel(799436478975180831).SendMessageAsync(arg.Content);
            }

            if (arg.Channel.Name.StartsWith("@"))
            { 
                if (arg.ToString().StartsWith("!help"))
                {
                    await arg.Channel.SendMessageAsync("To send an anonymous message in the server, use !confess(message). It will appear in the #confessions channel.\n\n");
                    await arg.Channel.SendMessageAsync("**To send an image do the following:**\n1) Send the image.\n\n**To send an emote do the following:**\n1) Make sure that the bot is in the server that the emote is in.\n2) Type the following into chat: \\:emoteName:. This will give you something like this:\n");
                    await arg.Channel.SendMessageAsync("https://cdn.discordapp.com/attachments/440118112977944578/801821181195583518/unknown.png \n");
                    await arg.Channel.SendMessageAsync("3); Copy&Paste that into your message!\n4) Pray.\n\n**To Ping someone do the following:**\n1) Get their user ID: Right click on their profile and click 'Copy ID'.\n2) Type the following into your message: <@userid> like this:\n https://cdn.discordapp.com/attachments/440118112977944578/801823083391615066/unknown.png \n");
                    await arg.Channel.SendMessageAsync("Make sure to put an > at the end of it. If done correctly, the user id thingy should turn into their username!\n3) Pray.\n");
                }

                if (arg.ToString().StartsWith("!confess"))
                {
                    string message = arg.ToString();
                    string confession = message[8..];


                    if (message.Contains("@everyone") || message.Contains("@here"))
                    {
                        await arg.Channel.SendMessageAsync("You dumb fucking cretin, you fucking fool, absolute fucking buffoon, you bumbling idiot. Fuck you. Fuck you and your @everyone. I'm not an idiot. I might hate you, myself, and your shitty little server, but I've got decency. Something you seem to lack. Go dig yourself a hole you shitty little wankstain. Birthing you was a mistake. I hope you and your hopes and dreams die in a fire and noone will even remember your existence. Fuck you.");
                        await _discord.GetGuild(788977623039606794).GetTextChannel(799436478975180831).SendMessageAsync($"Your pal {arg.Author.Mention} just tried to ping everyone. Go and flame the shit out of them!");
                        await _discord.GetGuild(799425879306403851).GetTextChannel(799425879306403854).SendMessageAsync($"Your pal {arg.Author.Mention} just tried to ping everyone. Go and flame the shit out of them!");
                        await _discord.GetGuild(799425879306403851).GetTextChannel(799425879306403854).SendMessageAsync($"||{arg.Channel.Name}|| tried to ping @everyone.");
                        await _discord.GetGuild(799425879306403851).GetTextChannel(799425879306403854).SendMessageAsync($"||{arg.Channel.Name} (ID:{arg.Author.Id}) || confessed to{confession}");
                        return;
                    }   


                    if (confession != "")
                    {
                        await _discord.GetGuild(788977623039606794).GetTextChannel(799427769720242216).SendMessageAsync(confession);
                        await _discord.GetGuild(799425879306403851).GetTextChannel(799425879306403854).SendMessageAsync($"||{arg.Channel.Name} (ID:{arg.Author.Id}) || confessed to{confession}");
                    }

                    if (arg.Attachments != null)
                    {
                        try
                        {
                            var attachments = arg.Attachments;
                            string url = attachments.ElementAt(0).Url;

                            await _discord.GetGuild(788977623039606794).GetTextChannel(799427769720242216).SendMessageAsync(url);
                            await _discord.GetGuild(799425879306403851).GetTextChannel(799425879306403854).SendMessageAsync($"||{arg.Channel.Name} (ID:{arg.Author.Id}) || confessed to{url}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            }
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<HttpClient>()
                .BuildServiceProvider();
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }
    }

}
