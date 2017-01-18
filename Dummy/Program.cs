using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API;

namespace Dummy
{
    class Program
    {
        static void Main(string[] args)
        {
            Steam steam = new Steam();
            var games = steam.GetGames();
            var gamesFile = new NameValueCollection();
            foreach (var game in games)
            {
                foreach (var achiev in game.Achievements)
                {
                    gamesFile.Add(game.ID.ToString(), achiev.Name);
                }
            }
            var data = gamesFile.Cast<object>().Select((t, i) => gamesFile.GetKey(i) + ":" + gamesFile.Get(i)).ToList();
            File.WriteAllLines("games.txt", data);
        }
    }
}
