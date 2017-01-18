using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Steamworks;

namespace API
{
    public static class Steam
    {
        public static List<Game> GetGames()
        {
            SteamAPI.Init();
            ulong steamId = SteamUser.GetSteamID().m_SteamID;
            var apiJson = new StreamReader(
                // ReSharper disable once AssignNullToNotNullAttribute
                WebRequest.Create(
                "http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key=006C1D814005AF1CAE4B670EE4B38979&steamid=" + steamId + "&l=english&json")
                .GetResponse().GetResponseStream()).ReadToEnd();
            var gamesList = JObject.Parse(apiJson)["response"]["games"].Children().Select(current => current.SelectToken("appid").ToString()).ToList();
            var list = (from game in gamesList
                    let json = JObject.Parse(new StreamReader(WebRequest.Create("http://store.steampowered.com/api/appdetails?appids=" + game).GetResponse().GetResponseStream()).ReadToEnd())
                    where json[game]["success"].Value<bool>()
                    select new Game()
                    {
                        Name = json[game]["data"]["name"].Value<string>(),
                        Achievements = GetAchievements(game),
                        ID = Convert.ToUInt64(game)
                    }).ToList();
            SteamAPI.Shutdown();
            return list;
        }


        public static List<Achievement> GetAchievements(string appId)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var apiJson = new StreamReader(WebRequest.Create(
                    "http://api.steampowered.com/ISteamUserStats/GetSchemaForGame/v0002/?key=006C1D814005AF1CAE4B670EE4B38979&appid=" + appId + "&l=english&json")
                .GetResponse().GetResponseStream()).ReadToEnd();
            var achievs = new List<Achievement>();
            if (JObject.Parse(apiJson)["game"]["availableGameStats"] != null && JObject.Parse(apiJson)["game"]["availableGameStats"]["achievements"] != null)
            {
                float percentage = 0.0F;
                achievs.AddRange(JObject.Parse(apiJson)["game"]["availableGameStats"]["achievements"].Children()
                    .Where(achievement => SteamUserStats.GetAchievementAchievedPercent(achievement["name"].ToString(), out percentage))
                    .Where(achievement => percentage < 100)
                    .Select(achievment => new Achievement()
                {
                    Name = achievment.SelectToken("name").ToString(),
                    RealName = achievment.SelectToken("displayName").ToString()
                }));
            }
            return achievs;
        }
    }
}
