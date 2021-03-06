﻿using System;

namespace CsSearch
{
	class SearchMain
	{
		private static void Log(string message)
		{
			Console.WriteLine(message);
		}

		static void Main(string[] args)
		{
			var options = new SearchOptions();
			try
			{
				var settings = options.SettingsFromArgs(args);
				var searcher = new Searcher(settings);
				searcher.Search();

				if (settings.PrintResults)
				{
					Log("");
					searcher.PrintResults();
				}

				if (settings.ListDirs)
				{
					searcher.PrintMatchingDirs();
				}

				if (settings.ListFiles)
				{
					searcher.PrintMatchingFiles();
				}

				if (settings.ListLines)
				{
					searcher.PrintMatchingLines();
				}
			}
			catch (SearchException e)
			{
				Log(string.Format("\nError: {0}", e.Message));
				options.Usage(1);
			}
		}
	}
}
