using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;
using Newtonsoft.Json.Linq;

namespace SteamAchiever
{
    class Program
    {
        static void Main(string[] args)
        {
            var games = LoadGames();
            for (int i = 0; i < games.Count; i++)
            {
                Console.WriteLine(games.GetKey(i));
                SpoofID(games.GetKey(i));
                foreach (var achievement in games.Get(i).Split(','))
                {
                    Spoof(achievement);
                }
            }
            Console.WriteLine("Done");
            Console.Read();
        }

        static void SpoofID(string id)
        {
            File.WriteAllText("steam_appid.txt", id);
        }

        static void Spoof(string achievement)
        {
            var gameSpooferProcessInformation = new ProcessStartInfo
            {
                FileName = @"Spoofer.exe",
                Arguments = achievement,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            var newProcess = new Process
            {
                StartInfo = gameSpooferProcessInformation,
            };
            newProcess.Start();
            newProcess.WaitForExit();
        }

        static NameValueCollection LoadGames()
        {
            var gameSpooferProcessInformation = new ProcessStartInfo
            {
                FileName = @"Dummy.exe",
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            var newProcess = new Process
            {
                StartInfo = gameSpooferProcessInformation,
            };
            newProcess.Start();
            newProcess.WaitForExit();
            var strings = File.ReadAllLines("games.txt");
            var nvc = new NameValueCollection();
            foreach (string t in strings)
            {
                nvc.Add(t.Split(':')[0], t.Split(':')[1]);
            }
            return nvc;
        }
    }
}
