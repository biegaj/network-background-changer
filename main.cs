using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace sandbox
{
    class Program
    {
        static string NETWORK = "";

        #region Referances
        [DllImport("wtsapi32.dll", SetLastError = true)]
        static extern bool WTSDisconnectSession(IntPtr hServer, int sessionId, bool bWait);
        #endregion

        static string ack = "";
        static string backgrounds = @$"\\{NETWORK}\Users$\Students\{Environment.UserName}\Application Data\Microsoft\Windows\Themes\CachedFiles";
        static void Main(string[] args)
        {
            WebClient client = new WebClient();
            #region Introduction
            Directory.CreateDirectory(".newBackground"); Directory.CreateDirectory(".backup");
            #endregion

            Console.Clear();
            Console.WriteLine("1 New folder exists");
            try
            {
                client.DownloadString("https://www.google.com/");
                Console.WriteLine("0 Internet connection stable");
            }
            catch { Console.WriteLine("1 Internet connection dirupted"); }

            Act(backgrounds, ".backup"); // backup
            Console.WriteLine("1 Backups Created");
            Act(".newBackground", backgrounds, true); // main

            static void Act(string background, string newFolder, bool main = false)
            {
                // only way is to paste it twice rip
                #region Tasks
                DirectoryInfo gatherNamesTask = new DirectoryInfo(backgrounds);
                FileInfo[] Files = gatherNamesTask.GetFiles("*.jpg");
                foreach (FileInfo file in Files) { ack = file.Name; /* Act(".newBackground", backgrounds, true); */ } // reroute into different files
                DirectoryInfo replaceFinal = new DirectoryInfo(background);
                FileInfo[] _Files = replaceFinal.GetFiles("*.jpg");
                foreach (FileInfo file in _Files)
                {
                    File.Delete($"{newFolder}/{ack}");
                    File.Copy(file.FullName, $"{newFolder}/{ack}");
                }
                #endregion
            }

            Console.WriteLine("1 Process Complete");
            Console.WriteLine("? For this to make an effect, you will have to relog. Proceed (y/n)?");
            string msg = Console.ReadLine();
            switch (msg)
            {
                case "y":
                    WindowsLogOff();
                    break;
                case "n":
                    Environment.Exit(1);
                    break;
                default:
                    Environment.Exit(1);
                    break;
            }

            [DllImport("user32.dll", SetLastError = true)]
            static extern bool ExitWindowsEx(uint uFlags, uint dwReason);
            static bool WindowsLogOff()
            {
                return ExitWindowsEx(0, 0);
            }
        }
    }
}
