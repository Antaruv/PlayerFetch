using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerFetch
{
	class Score
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
		public bool perfect { get; set; }
		public int enabled_mods { get; set; }
		public uint user_id { get; set; }
		public string date { get; set; }
		public uint rank { get; set; }
		public float pp { get; set; }
	}
}
