using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace BGC
{
    class Program
    {
        /*
          <PropertyGroup>
	        <LangVersion>preview</LangVersion>
          </PropertyGroup>
        */

        [DllImport("wtsapi32.dll", SetLastError = true)]
        static extern bool WTSDisconnectSession(IntPtr hServer, int sessionId, bool bWait);

        static string NETWORK = ""; // set this
        static string backgrounds = @$"\\{NETWORK}\Users$\Students\{Environment.UserName}\Application Data\Microsoft\Windows\Themes\CachedFiles";
        enum Parameters
        {
            Sneaky,
            ForceBackup
        }
        static void Main(string[] args)
        {
            Directory.CreateDirectory(".backup"); Directory.CreateDirectory(".newBackground");

            WebClient webClient = new WebClient();
            Console.Write($"{webClient.DownloadString("https://raw.githubusercontent.com/biegaj/biegaj/main/intro")}\n");

            Console.Write("$ Insert your new desired computer background into the '.newBackground' directory.\nAfterward, log out of your device and upon relogging the new background should be active. \nPlease note the background can be automatically fixed from time to time.\nYou can revert your background by taking the image in '.backup' and putting it into '.newBackground'.\n\n" +
                "$ Proceed? (y/n) ");
            string proceedResponse = Console.ReadLine();

            Console.Write("\n");

            switch (proceedResponse)
            {
                case "y":
                    Act();
                    break;
                    /*
                case: "Sneaky":
                    Process.Start("ipconfig", "/release");
                    Act();
                    Process.Start("ipconfig", "/retry");
                    break;
                    */
                default:
                    Environment.Exit(1);
                    break;
            }

            static void Act()
            {
                string[] fileNames = Directory.GetFiles(backgrounds, "*.jpg", SearchOption.TopDirectoryOnly);

                foreach (string file in fileNames)
                {
                    var fileNamesActual = file.Substring(file.LastIndexOf("\\") + 1); Report("Indexed file names", true);

                    try { File.Copy(file, $".backup/{fileNamesActual}"); Report("Backup complete", true); } catch { Random rnd = new Random(); File.Copy(file, $".backup/{rnd.Next(11111, 99999)}{fileNamesActual}"); Report("Backup interrupted", false); }

                    DirectoryInfo gatherNamesTask = new DirectoryInfo(".newBackground"); Report("Gathered new backgrounds", true);
                    FileInfo[] Files = gatherNamesTask.GetFiles("*.jpg"); Report("Indexed jpegs", true);
                    foreach (FileInfo toReplace in Files) { File.Delete($"{backgrounds}/{fileNamesActual}"); File.Copy(toReplace.FullName, $"{backgrounds}/{fileNamesActual}"); }
                    Report("Replaced and backed up", true);
                }
            }

            static void Report(string report, bool good)
            {
                int oneorzero = good ? oneorzero = 1 : oneorzero = 0;
                if (oneorzero == 1) { Console.ForegroundColor = ConsoleColor.Green; } else { Console.ForegroundColor = ConsoleColor.Red; }
                Console.WriteLine($"{oneorzero} | {report}");
                Console.ResetColor();
            }

            Console.Write("\n$ Finished! For changes to take place, a relog is necessary. Proceed? (y/n) ");

            string relogOrNot = Console.ReadLine();

            switch (relogOrNot)
            {
                case "y":
                    WindowsLogOff();
                    break;
                default:
                    Environment.Exit(1);
                    break;
            }

            Console.ReadLine();

            [DllImport("user32.dll", SetLastError = true)]
            static extern bool ExitWindowsEx(uint uFlags, uint dwReason);
            static bool WindowsLogOff()
            {
                return ExitWindowsEx(0, 0);
            }
        }
    }
}
