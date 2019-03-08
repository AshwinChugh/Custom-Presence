using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;


using DiscordRPC;
using DiscordRPC.Logging;


namespace Custom_Presence
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;


        private static int _discordPipe = -1;

        private static string _ClientID = "553307638146924588";

        private static LogLevel DiscordLogLevel = LogLevel.Info;
        //the actual presence itself
        private static RichPresence presence = new RichPresence()
        {
            Details = "Detroying Libtards",
            State = "Libtards Rekt: ",
            Timestamps = Timestamps.Now,
            Assets = new Assets()
            {
                LargeImageKey = "ben_shapiro",
                LargeImageText = "Libtard Destroyer",
                SmallImageKey = "yeet",
                SmallImageText = "#Obliterated"
            }
        };


        private static DiscordRpcClient client;

        private StringBuilder word = new StringBuilder();

        private static void custom()
        {
            Console.WriteLine("Do you want to set a custom presence?(Y/N)");
            var response = Console.ReadLine();
            if (response == "Y" || response == "y")
            {
                Console.Write("Enter details of your custom RPCS: ");
                presence.Details = Console.ReadLine();

                Console.Write("Enter the state: ");
                presence.State = Console.ReadLine();

                Console.Write("Enter main image name: ");
                presence.Assets.LargeImageText = Console.ReadLine();

                Console.WriteLine("Enter the small image name: ");
                presence.Assets.SmallImageText = Console.ReadLine();
            }
            else if (response == "N" || response == "n")
            {
                Console.WriteLine("Fine nigger.");
            }
            else
            {
                Console.WriteLine("Please only respond with 'Y', 'y' and 'N', 'n'.");
                custom();
            }
        }

        static void Main(string[] args)
        {
            custom();
            var handle = GetConsoleWindow();

            // Show
            ShowWindow(handle, SW_SHOW);
            Console.WriteLine(@"Starting custom Presence.");

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-pipe":
                        _discordPipe = int.Parse(args[++i]);
                        break;
                }
            }

            presence.Party = new Party()
            {
                ID = Secrets.CreateSecret(new Random()),
                Max = 1337,
                Size = 666
            };
            mainPresence();
        }

        static void mainPresence()
        {
            var client = new DiscordRpcClient(_ClientID);

            //client.Logger = new ConsoleLogger() {Level = DiscordLogLevel, Colored = true};

            //client.OnReady += (sender, msg) => { Console.WriteLine("Connected to discord under user: {0}", msg.User.Username); };
            //client.OnPresenceUpdate += (sender, msg) => { Console.WriteLine("Presence has been updated"); };

            //timer to regularly invoke
            var timer = new System.Timers.Timer(150);
            timer.Elapsed += (sender, evt) => { client.Invoke(); };
            timer.Start();

            //connect
            client.Initialize();

            client.SetPresence(presence);

            Console.WriteLine("Discord Rich Presence Initialization Finished.");
            Console.WriteLine("This window close in 15 seconds.");
            Console.WriteLine("You can terminate the program in task manager. It will be called CustomPresence.exe in background processes.");
            Thread.Sleep(15000);
            //Hide
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);

            Console.ReadKey();
            timer.Dispose();
            client.Dispose();
        }
    }
}
