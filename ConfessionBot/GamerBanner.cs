using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Discord;
using Discord.WebSocket;
using System.Linq;

namespace ConfessionBot
{
    internal class GamerBanner
    {
        public static List<SocketGuildUser> listOfMembers = new List<SocketGuildUser>();
        public static List<LoLser> listOfLoLsers = new List<LoLser>();

        public static string[] games = { "LEAGUE OF LEGENDS", "OVERWATCH", "GENSHIN IMPACT", "WAR THUNDER", "DOTA" };

        public static void onTimedEvent(Object source, ElapsedEventArgs e)
        {
            checkGame();
            checkLoLser();
        }

        public static void checkGame()
        {
            foreach (var item in listOfMembers)
            {
                if(item.Activities != null)
                {
                    foreach (var activity in item.Activities)
                    {
                        string activityName = activity.Name.ToUpper();
                        if (games.Any(activityName.Contains))
                        {
                            LoLser lolser = new LoLser();
                            
                            if (activityName.Contains(games[0])) lolser.game = "League of Legends";
                            if (activityName.Contains(games[1])) lolser.game = "Overwatch 2";
                            if (activityName.Contains(games[2])) lolser.game = "Genshin Impact";
                            if (activityName.Contains(games[3])) lolser.game = "War Thunder";
                            if (activityName.Contains(games[4])) lolser.game = "Dota 2";
                            lolser.user = item;
                            lolser.spammed = false;
                            if (!listOfLoLsers.Any(item => item.user == lolser.user))
                            {
                                Console.WriteLine(lolser.user.Username + ": " + lolser.game + " has been added to the LoLsers.");
                                listOfLoLsers.Add(lolser);
                            }
                        }
                    }
                }
            }
        }

        public static async void checkLoLser()
        {
            foreach (var LoLser in listOfLoLsers.ToArray())
            {
                LoLser.timePlayed++;
                if (LoLser.timePlayed >= 10 && LoLser.spammed == false)
                {
                    //for (int i = 1; i < 20; i++)
                    //{
                    //    System.Threading.Thread.Sleep(500);
                    //    await LoLser.user.SendMessageAsync("You scum. You played " + LoLser.game + " for more than 10 minutes. How could you commit such war crimes. This is against the geneva conventions. Get p0wnd skrub.");
                    //}
                    await MainClass.spammedMessage(LoLser);
                    LoLser.spammed = true;
                }
            }
        }
    }


    public class LoLser
    {
        public bool spammed { get; set; }
        public string game { get; set; }
        public int timePlayed { get; set; }
        public SocketGuildUser user { get; set; }
    }
}
