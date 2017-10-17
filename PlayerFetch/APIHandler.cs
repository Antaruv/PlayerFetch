using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using AngleSharp;
using Newtonsoft.Json;

namespace PlayerFetch
{
    class APIHandler
    {
	    public const int countLimit = 60;
	    public const int timeLimit = 60;

	    private readonly string key;
	    private readonly WebClient loader= new WebClient();
	    public List<DateTime> requests = new List<DateTime>();

	    public APIHandler(string key)
	    {
		    this.key = key;
	    }

	    private string tryDownload(string url, int attempts)
	    {
		    if (attempts > 0)
		    {
			    try
			    {
				    return loader.DownloadString(url);
			    }
			    catch
			    {
				    return tryDownload(url, --attempts);
			    }
		    }
		    else
		    {
			    throw new Exception();
		    }
	    }

	    public void startRequest()
	    {
			Thread.Sleep(getWaitTime());
			requests.Add(DateTime.Now);
	    }

	    public TimeSpan getWaitTime()
	    {
		    if (requests.Count == 0)
			    return TimeSpan.Zero;

		    var now = DateTime.Now;
		    requests.RemoveAll(r => now - r >= TimeSpan.FromSeconds(timeLimit));
		    var earliest = requests.Min(a => a);

		    return requests.Count >= countLimit ? 
				TimeSpan.FromSeconds(timeLimit) - (now - earliest) : 
				TimeSpan.Zero;
	    }

	    public Player loadPlayer(uint id)
	    {
		    startRequest();
		    string apiURL = "http://osu.ppy.sh/api/get_user?k=" + key + "&mode=0&type=id&u=" + id;

		    string json = tryDownload(apiURL, 3);
		    var thisplayer = JsonConvert.DeserializeObject<Player[]>(json);
		    return thisplayer.First();
	    }

	    public List<Score> loadScores(int user_id, int number)
	    {
			startRequest();
		    string apiURL = "https://osu.ppy.sh/api/get_user_best?m=0&limit=" + number + "&k=" + key + "&u=" + user_id;

		    string json = tryDownload(apiURL, 3);
		    var scores = JsonConvert.DeserializeObject<Score[]>(json);

		    return scores.ToList();
	    }

	    public Beatmap loadBeatmap(uint beatmap_id)
	    {
			startRequest();
		    string apiURL = "https://osu.ppy.sh/api/get_beatmaps?m=0&b=" + beatmap_id + "&k=" + key;

		    string json = tryDownload(apiURL,3);
		    var beatmap = JsonConvert.DeserializeObject<Beatmap[]>(json);

		    return beatmap.First();
	    }

	    public List<Score> loadScores(int user_id) =>
		    loadScores(user_id, 100);

	}
}
