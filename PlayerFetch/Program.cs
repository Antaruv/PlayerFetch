using AngleSharp;
using System;
using System.Net;
using AngleSharp.Parser.Html;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PlayerFetch
{
    class Program
    {
        static void Main(string[] args)
        {
			WebClient loader = new WebClient();

	        

			Console.Write("Done! ");
			Console.ReadLine();
		}

		static void addPlayers(PlayerContext db, List<Player> playerList)
		{
			foreach(var player in playerList)
			{
				if(db.Players.Any(p => p.user_id == player.user_id))
				{
					db.Update(player);
				} else
				{
					db.Add(player);
				}
			}

			db.SaveChanges();
		}
    }
}
