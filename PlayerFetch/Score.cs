using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
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
		public Player player { get; set; }
		public Beatmap beatmap { get; set; }

		public uint user_id { get; set; }

		private static WebClient loader = new WebClient();

	}
}
