using AngleSharp.Dom.Html;
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
		private static WebClient loader = new WebClient();

		//TODO: this is not very DRY
		public static List<Player> loadCountryPage(string url)
		{
			List<Player> playerList = new List<Player>();

			var parser = new HtmlParser();
			var document = tryParse(url);
			
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

		public static List<Player> loadAllCountryPages(string code)
		{
			List<Player> playerList = new List<Player>();
			
			for(int pageNumber = 1; pageNumber<=200; pageNumber++) //pageNumber <= 4 is for testing purposes, should be 200.
			{
				var addition = loadPage(code, pageNumber);
				if(addition.Count == 0)
				{
					break;
				}

				playerList.AddRange(addition);

				//Console.CursorLeft = 0;
				Console.WriteLine("Loaded page " + pageNumber + " for " + code);
			}

			return playerList;
		}


		public static List<Player> loadGlobalPage(string url)
		{
			List<Player> playerList = new List<Player>();

			var parser = new HtmlParser();
			var document = tryParse(url);

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

		public static List<Player> loadGlobalPages(int nPages)
		{
			List<Player> playerList = new List<Player>();

			for(int pageNumber = 1; pageNumber<=nPages; pageNumber++)
			{
				var addition = loadPage(pageNumber);
				playerList.AddRange(addition);
				Console.CursorLeft = 0;
				Console.Write(pageNumber);
			}

			return playerList;
		}

		public static List<Player> loadGlobalPages(WebClient loader) =>
			loadGlobalPages(200);

		public static List<Player> loadPage(int pageNumber) =>
			loadGlobalPage("http://osu.ppy.sh/p/pp/?m=0&s=3&o=1&f=&page=" + pageNumber);

		public static List<Player> loadPage(string code, int pageNumber) =>
			loadCountryPage("http://osu.ppy.sh/p/pp/?c=" + code + "&m=0&s=3&o=1&f=&page=" + pageNumber);


		public static List<string> loadCountryCodesPage(string url)
		{
			var parser = new HtmlParser();
			var document = tryParse(url);

			var playerNames = document.QuerySelectorAll(".beatmapListing tbody tr td a");
			var ids = playerNames.Select(m => m.GetAttribute("href"))
				.Select(href => href.Substring(8));

			return ids.ToList();
		}

		public static List<string> loadCountryCodesPage(int page) =>
			loadCountryCodesPage("https://osu.ppy.sh/p/countryranking?p=countryranking&s=3&o=1&page=" + page);
		
		public static List<string> loadAllCountryCodes(WebClient loader)
		{
			List<string> codes = new List<string>();

			int pageNumber = 1;
			bool running = true;
			while(running)
			{
				var addition = loadCountryCodesPage(pageNumber);
				running = addition.Any();
				codes.AddRange(addition);
				pageNumber++;
			}

			return codes;
		}

		public static List<Player> badLoadMaxPlayers(WebClient loader)
		{
			List<Player> playerList = new List<Player>();

			var codes = loadAllCountryCodes(loader);
			codes.Reverse();

			int i = 0;
			int total = codes.Count();
			foreach(string code in codes)
			{
				//Console.SetCursorPosition(0, 0);
				Console.WriteLine("Loading " + code);
				playerList.AddRange(loadAllCountryPages(code));
				//Console.SetCursorPosition(0, 0);
				Console.WriteLine("Loaded " + code);
				Console.WriteLine(i++ + "/" + total);
			}

			return playerList;
		}

		public static IHtmlDocument tryParse(string url)
		{
			try
			{
				return new HtmlParser().Parse(loader.DownloadString(url));
			}
			catch
			{
				Console.WriteLine("Failed to load url: " + url);
				return tryParse(url);
			}
		}

		public void fetchStoreMaxPlayers(WebClient loader)
		{
			var codeList = loadAllCountryCodes(loader);

			using (var db = new PlayerContext())
			{
				foreach (var code in codeList)
				{
					for (int pageNumber = 1; pageNumber <= 200; pageNumber++)
					{
						var addition = loadPage(code, pageNumber);

					}
				}
			}
		}
	}
}
