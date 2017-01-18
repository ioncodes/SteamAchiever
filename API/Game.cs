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
    public class Game
    {
        public string Name { get; set; }
        public ulong ID { get; set; }
        public List<Achievement> Achievements { get; set; }
    }
}
