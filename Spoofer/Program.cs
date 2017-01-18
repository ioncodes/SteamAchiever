using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steamworks;

namespace Spoofer
{
    class Program
    {
        static void Main(string[] args)
        {
            SteamAPI.Init();
            SteamUserStats.SetAchievement(args[0]);
            SteamAPI.Shutdown();
        }
    }
}
