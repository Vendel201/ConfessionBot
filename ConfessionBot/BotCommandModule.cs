using Discord.Commands;
using Discord;
using System;
using System.Threading.Tasks;

namespace ConfessionBot
{
    public class BotCommandModule : ModuleBase<SocketCommandContext>
    {
        [Command("info")]
        public async Task Ping()
        { 
        }
    }
}
