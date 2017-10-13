using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace PlayerFetch
{
	public class Score
	{
		public uint beatmap_id { get; set; }
		public uint score { get; set; }
		public uint maxcombo { get; set; }
		public uint count50 { get; set; }
		public uint count100 { get; set; }
		public uint count300 { get; set; }
		public uint countmiss { get; set; }
		public uint countkatu { get; set; }
		public uint countgeki { get; set; }
		public uint perfect { get; set; }
		public Mods enabled_mods { get; set; }
		public string date { get; set; }
		public string rank { get; set; }
		public float pp { get; set; }

		public uint user_id { get; set; }
		public Player player { get; set; }

		private static WebClient loader = new WebClient();

		public static List<Score> loadScores(int user_id, int number)
		{
			string key = Environment.GetEnvironmentVariable("APIKEY");
			string apiURL = "https://osu.ppy.sh/api/get_user_best?m=0&limit=" + number + "&k=" + key + "&u=" + user_id;

			string json = loader.DownloadString(apiURL);
			var scores = JsonConvert.DeserializeObject<Score[]>(json);

			return scores.ToList();
		}

		public static List<Score> loadScores(int user_id) =>
			loadScores(user_id, 100);
	}
}
