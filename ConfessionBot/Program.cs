using System.Threading;

namespace ConfessionBot
{
    class Program
    {
        public static void Main(string[] args)
        {
            Program program = new Program();
            program.Start();
        }

        private void Start()
        {
            Thread thread = new Thread(new ThreadStart(RunBot));
            thread.Start();
        }

        private void RunBot()
        {
            var bot = new MainClass();
            bot.Run().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
