
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PlayerFetch
{
	public class Beatmap
	{
		public uint beatmapset_id { get; set; }

		[Key]
		public uint beatmap_id { get; set; }
		public uint approved { get; set; }
		public int total_length { get; set; }
		public int hit_length { get; set; }
		public string version { get; set; }
		public string file_md5 { get; set; }
		public int diff_size { get; set; }
		public int diff_overall { get; set; }
		public int diff_approach { get; set; }
		public int diff_drain { get; set; }
		public string mode { get; set; }
		public DateTime approved_date { get; set; }
		public DateTime last_update { get; set; }
		public string artist { get; set; }
		public string title { get; set; }
		public string creator { get; set; }
		public float bpm { get; set; }
		public string source { get; set; }
		public string tags { get; set; }
		public int genre_id { get; set; }
		public int language_id { get; set; }
		public int favourite_count { get; set; }
		public int playcount { get; set; }
		public int passcount { get; set; }
		public int max_combo { get; set; }
		public float difficultyrating { get; set; }
		public List<Score> Scores { get; set; }
	}


}
