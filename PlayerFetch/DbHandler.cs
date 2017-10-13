using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Linq;

namespace PlayerFetch
{
    class DbHandler
    {
		private static WebClient loader = new WebClient();
		private static PlayerContext db = new PlayerContext();

		public static int storeFetchPlayers(List<Player> playerList)
		{
			foreach(var player in playerList)
			{
				var current_player = Player.loadPlayer(player.user_id, loader);

				if(db.Players.Any(p => p.user_id==current_player.user_id))
				{
					db.Update(current_player);
				}
				else
				{
					db.Add(current_player);
				}

				Console.WriteLine("Loaded player " + player.username);
			}

			return db.SaveChanges();
		}
		
		public static int storePlayerList(List<Player> playerList)
		{
			foreach(var player in playerList)
			{
				if(db.Players.Any(p => p.user_id==player.user_id))
				{
					db.Update(player);
				}
				else
				{
					db.Add(player);
				}
			}

			return db.SaveChanges();
		}

		public static int loadAndStorePage(int pageNumber) =>
			storeFetchPlayers(Page.loadPage(pageNumber));
    }
}
