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
		//TODO: this is not very DRY
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

		public static List<Player> loadAllCountryPages(string code,WebClient loader)
		{
			List<Player> playerList = new List<Player>();
			
			for(int pageNumber = 1; pageNumber<=4; pageNumber++) //pageNumber <= 4 is for testing purposes, should be 200.
			{
				var addition = loadCountryPage(code, pageNumber, loader);
				playerList.AddRange(addition);

				//Console.CursorLeft = 0;
				Console.WriteLine("Loaded page " + pageNumber + " for " + code);
			}

			return playerList;
		}


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

		public static List<Player> loadGlobalPages(int nPages, WebClient loader)
		{
			List<Player> playerList = new List<Player>();

			for(int pageNumber = 1; pageNumber<=nPages; pageNumber++)
			{
				var addition = loadGlobalPage(pageNumber, loader);
				playerList.AddRange(addition);
				Console.CursorLeft = 0;
				Console.Write(pageNumber);
			}

			return playerList;
		}

		public static List<Player> loadGlobalPages(WebClient loader) =>
			loadGlobalPages(200, loader);



		public static List<string> loadCountryCodesPage(string url, WebClient loader)
		{
			var parser = new HtmlParser();
			var document = parser.Parse(loader.DownloadString(url));

			var playerNames = document.QuerySelectorAll(".beatmapListing tbody tr td a");
			var ids = playerNames.Select(m => m.GetAttribute("href"))
				.Select(href => href.Substring(8));

			return ids.ToList();
		}

		public static List<string> loadCountryCodesPage(int page, WebClient loader) =>
			loadCountryCodesPage("https://osu.ppy.sh/p/countryranking?p=countryranking&s=3&o=1&page=" + page, loader);
		
		public static List<string> loadAllCountryCodes(WebClient loader)
		{
			List<string> codes = new List<string>();

			int pageNumber = 1;
			bool running = true;
			while(running)
			{
				var addition = loadCountryCodesPage(pageNumber, loader);
				running = addition.Any();
				codes.AddRange(addition);
				pageNumber++;
			}

			return codes;
		}

		public static List<Player> loadMaxPlayers(WebClient loader)
		{
			List<Player> playerList = new List<Player>();

			var codes = loadAllCountryCodes(loader);

			int i = 0;
			int total = codes.Count();
			foreach(string code in codes)
			{
				//Console.SetCursorPosition(0, 0);
				Console.WriteLine("Loading " + code);
				playerList.AddRange(loadAllCountryPages(code, loader));
				//Console.SetCursorPosition(0, 0);
				Console.WriteLine("Loaded " + code);
				Console.WriteLine(i++ + "/" + total);
			}

			return playerList;
		}
	}
}
