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

	    public static int loadAndStoreScores(int user_id)
	    {
		    var scoreList = Score.loadScores(user_id);

		    db.SaveChanges();

		    foreach (var score in scoreList)
		    {
				
			    if (db.Players.Any(p => p.user_id == score.user_id))
			    {
					if (db.Scores.Any(s => s.user_id == score.user_id &&
											s.beatmap_id == score.beatmap_id &&
											s.enabled_mods == score.enabled_mods))
					{
					    db.Update(score);
				    }
				    else
				    {
					    db.Add(score);
				    }
			    }
		    }

		    return db.SaveChanges();
	    }

	    public static int loadAndStoreScores(Player player) =>
		    loadAndStoreScores((int) player.user_id);

	    public static int loadAllPlayerScores()
	    {
		    var total = 0;
		    foreach (var player in db.Players)
		    {
				int addition = loadAndStoreScores(player);
			    total += addition;
				Console.WriteLine("{0} scores loaded for {1}", addition, player.username);
		    }

		    return total;
	    }
    }
}
