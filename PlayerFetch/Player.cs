using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;

namespace PlayerFetch
{
	public class PlayerList
	{
		public List<Player> list { get; set; }
	}

	public class Player
	{
		[Key]
		public uint user_id { get; set; }

		[Required]
		public string username { get; set; }

		public uint count300 { get; set; }
		public uint count100 { get; set; }
		public uint count50 { get; set; }
		public uint playcount { get; set; }
		public ulong ranked_score { get; set; }
		public ulong total_score { get; set; }
		public uint pp_rank { get; set; }
		public float level { get; set; }
		public float pp_raw { get; set; }
		public float accuracy { get; set; }
		public uint count_rank_ss { get; set; }
		public uint count_rank_s { get; set; }
		public uint count_rank_a { get; set; }
		public string country { get; set; }
		public uint pp_country_rank { get; set; }

		public List<Score> Scores { get; set; }
		//public List<Event> events { get; set; }

		public Player()
		{
		}

		public Player(uint id, string name, uint rank)
		{
			this.pp_rank = rank;
			this.user_id = id;
			this.username = name;
		}

		public static Player loadPlayer(uint id, WebClient loader)
		{
			string key = Environment.GetEnvironmentVariable("APIKEY");
			string apiURL = "http://osu.ppy.sh/api/get_user?k=" + key + "&mode=0&type=id&u=";

			string json = loader.DownloadString(apiURL + id);
			var thisplayer = JsonConvert.DeserializeObject<Player[]>(json);
			return thisplayer.First();
		}
	}

	public class Event
	{
		public uint eventID { get; set; }
		public string display_html { get; set; }
		public uint beatmap_id { get; set; }
		public uint beatmapset_id { get; set; }
		public DateTime date { get; set; }
		public string epicfactor { get; set; }
	}
}