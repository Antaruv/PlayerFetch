using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace PlayerFetch
{
    class Page
    {
		public static List<Player> loadCountryPage(string url, WebClient loader)
		{
			List<Player> playerList = new List<Player>();

			var parser = new HtmlParser();
			var document = parser.Parse(loader.DownloadString(url));
			
			var playerNames = document.QuerySelectorAll(".beatmapListing tbody tr td a");
			var names = playerNames.Select(m => m.TextContent).ToList();
			var ids = playerNames.Select(m => m.GetAttribute("href")).ToList();

			int i = 0;
			foreach (var name in names)
			{
				var player = new Player();
				player.user_id = uint.Parse(ids[i].Substring(3));
				player.username = name;
				playerList.Add(player);

				i++;
			}

			return playerList;
		}

		public static List<Player> loadCountryPage(string code, int page, WebClient loader) => 
			loadCountryPage("http://osu.ppy.sh/p/pp/?c=" + code + "&m=0&s=3&o=1&f=&page=" + page, loader);

		public static List<Player> loadGlobalPage(string url, WebClient loader)
		{
			List<Player> playerList = new List<Player>();

			var parser = new HtmlParser();
			var document = parser.Parse(loader.DownloadString(url));

			var selector = "tr[class^='row'] td";
			var playerRanks = document.QuerySelectorAll(selector + " b");
			var playerNames = document.QuerySelectorAll(selector + " a");
			var names = playerNames.Select(m => m.TextContent).ToList();
			var ids = playerNames.Select(m => m.GetAttribute("href")).ToList();
			var ranks = playerRanks.Select(m => m.TextContent).ToList();

			int i = 0;
			foreach (var name in names)
			{
				var player = new Player(uint.Parse(ids[i].Substring(3)), name, uint.Parse(ranks[i].Substring(1)));
				playerList.Add(player);

				i++;
			}

			return playerList;
		}

		public static List<Player> loadGlobalPage(int pageNumber, WebClient loader) =>
			loadCountryPage("http://osu.ppy.sh/p/pp/?m=0&s=3&o=1&f=&page=" + pageNumber, loader);

	}
}
