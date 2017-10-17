using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Linq;

namespace PlayerFetch
{
    class DbHandler
    {
		private PlayerContext db = new PlayerContext();
	    private APIHandler apiHandler;

	    public DbHandler(APIHandler handler)
	    {
		    this.apiHandler = handler;
	    }


		public int storeFetchPlayers(List<Player> playerList)
		{
			foreach(var player in playerList)
			{
				var current_player = apiHandler.loadPlayer(player.user_id);

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
		
		public int storePlayerList(List<Player> playerList)
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

		public int loadAndStorePage(int pageNumber) =>
			storeFetchPlayers(Page.loadPage(pageNumber));

	    public int loadAndStoreScores(int user_id)
	    {
			var apihandler = new APIHandler(Environment.GetEnvironmentVariable("APIKEY"));
		    var scoreList = apihandler.loadScores(user_id);

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

	    public int loadAndStoreScores(Player player) =>
		    loadAndStoreScores((int) player.user_id);

	    public int loadAllPlayerScores()
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

	    public bool storeDummyBeatmap(uint beatmap_id)
	    {
		    if (!db.Beatmaps.Any(b => b.beatmap_id == beatmap_id))
		    {
			    var beatmap = new Beatmap();
			    beatmap.beatmap_id = beatmap_id;

			    db.Add(beatmap);
			    db.SaveChanges();
			    return true;
		    }
		    else
		    {
			    return false;
		    }

	    }

	    public int storeBeatmap(Beatmap beatmap)
	    {
			if (!db.Beatmaps.Any(b => b.beatmap_id == beatmap.beatmap_id))
			{
				db.Add(beatmap);
			}
			else
			{
				db.Update(beatmap);
			}

		    return db.SaveChanges();
	    }

	    public int storeBeatmaps(List<Beatmap> beatmaps)
	    {
		    foreach(var beatmap in beatmaps)
		    {
			    if (!db.Beatmaps.Any(b => b.beatmap_id == beatmap.beatmap_id))
			    {
				    db.Add(beatmap);
			    }
			    else
			    {
				    db.Update(beatmap);
			    }
		    }
		    return db.SaveChanges();
	    }

	    public int storeBeatmapsFromScores(List<Score> scores)
	    {
		    var beatmap_ids = scores.Select(s => s.beatmap_id)
									.Where(b => !db.Beatmaps.Any(bm => bm.beatmap_id == b && bm.total_length == 0))
									.Distinct();

		    var beatmaps = new List<Beatmap>();
		    foreach (var beatmap_id in beatmap_ids)
		    {
			    try
			    {
				    beatmaps.Add(apiHandler.loadBeatmap(beatmap_id));
			    }
			    catch
			    {
				    //TODO some kind of error logging?
			    }

		    }

			Console.WriteLine("Loaded beatmaps for " + beatmap_ids.Count() + "/" + scores.Count() + " scores.");

		    return storeBeatmaps(beatmaps);
	    }

	    public int storeAllBeatmapsFromScores()
	    {
		    var scores = db.Scores.ToList();
		    return storeBeatmapsFromScores(scores);
	    }
    }
}
