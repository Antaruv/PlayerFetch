using AngleSharp;
using System;
using System.Net;
using AngleSharp.Parser.Html;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PlayerFetch
{
    class Program
    {
        static void Main(string[] args)
        {
			int MAX = 2;
			var loader = new WebClient();

			var playerList = new List<Player>();

			for (int pageNumber = 1; pageNumber <= MAX && pageNumber <= 200; pageNumber++)
			{
				var addition = Page.loadCountryPage("US", pageNumber, loader);

				if (!addition.Any())
				{
					break;
				}

				playerList.AddRange(addition);

				Console.WriteLine("Loaded page " + pageNumber + "...");
			}

			string key = Environment.GetEnvironmentVariable("APIKEY");
			string apiURL = "http://osu.ppy.sh/api/get_user?k=" + key + "&mode=0&type=id&u=";

			using (var db = new PlayerContext())
			{
				foreach (var player in playerList)
				{
					var a = Player.loadPlayer(player.user_id, loader);
					//a.pp_rank = player.pp_rank;

					if (db.Players.Any(p => p.user_id == a.user_id))
					{
						db.Update(a);
					}
					else
					{
						db.Add(a);
					}
					var count = db.SaveChanges();

					Console.WriteLine("#" + player.pp_rank + "\t" + a.pp_raw + "\t\t" + a.username);
					//if(a.pp_rank==2)
					//	break;
				}

				Console.WriteLine("All players in db:");
				foreach (var player in db.Players)
				{
					Console.WriteLine(player.username);
				}
			}
			Console.ReadLine();
		}
    }
}
