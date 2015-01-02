﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CsSearch
{
	public class Searcher
	{
		private readonly FileUtil _fileUtil;
		public SearchSettings Settings { get; private set; }
		public IList<SearchResult> Results { get; private set; }
		public IDictionary<string, Stopwatch> Timers { get; private set; }

		public Searcher(SearchSettings settings)
		{
			Settings = settings;
			if (Settings.Verbose)
				Log(Settings + "\n");
			ValidateSettings();
			_fileUtil = new FileUtil();
			Results = new List<SearchResult>();
			Timers = new Dictionary<string, Stopwatch>();
		}

		private void Log(string message)
		{
			Console.WriteLine(message);
		}

		private void ValidateSettings()
		{
			if (string.IsNullOrEmpty(Settings.StartPath))
				throw new SearchArgumentException("Startpath not defined");
			if (!(new DirectoryInfo(Settings.StartPath)).Exists)
				throw new SearchArgumentException("Startpath not found");
			if (Settings.SearchPatterns.Count < 1)
				throw new SearchArgumentException("No search patterns specified");
		}

		private bool IsSearchDirectory(DirectoryInfo d)
		{
			if (Settings.ExcludeHidden && FileUtil.IsHiddenFile(d))
				return false;
			if (Settings.InDirPatterns.Count > 0 &&
				!Settings.InDirPatterns.Any(p => p.Match(d.Name).Success))
				return false;
			if (Settings.OutDirPatterns.Count > 0 &&
				Settings.OutDirPatterns.Any(p => p.Match(d.Name).Success))
				return false;
			return true;
		}

		private bool IsSearchFile(FileInfo f)
		{
			if (Settings.ExcludeHidden && FileUtil.IsHiddenFile(f))
				return false;
			if (Settings.InExtensions.Count > 0 &&
				!Settings.InExtensions.Contains(f.Extension))
				return false;
			if (Settings.OutExtensions.Count > 0 &&
				Settings.OutExtensions.Contains(f.Extension))
				return false;
			if (Settings.InFilePatterns.Count > 0 &&
				!Settings.InFilePatterns.Any(p => p.Match(f.Name).Success))
				return false;
			if (Settings.OutFilePatterns.Count > 0 &&
				Settings.OutFilePatterns.Any(p => p.Match(f.Name).Success))
				return false;
			return true;
		}

		private bool IsArchiveSearchFile(FileInfo f)
		{
			if (Settings.ExcludeHidden && FileUtil.IsHiddenFile(f))
				return false;
			if (Settings.InArchiveExtensions.Count > 0 &&
				!Settings.InArchiveExtensions.Contains(f.Extension))
				return false;
			if (Settings.OutArchiveExtensions.Count > 0 &&
				Settings.OutArchiveExtensions.Contains(f.Extension))
				return false;
			if (Settings.InArchiveFilePatterns.Count > 0 &&
				!Settings.InArchiveFilePatterns.Any(p => p.Match(f.Name).Success))
				return false;
			if (Settings.OutArchiveFilePatterns.Count > 0 &&
				Settings.OutArchiveFilePatterns.Any(p => p.Match(f.Name).Success))
				return false;
			return true;
		}

		public void StartTimer(string name)
		{
			var timer = new Stopwatch();
			timer.Start();
			Timers.Add(name, timer);
		}

		public void StopTimer(string name) {
			var timer = Timers[name];
			timer.Stop();
			PrintElapsed(name, timer.Elapsed);
		}

		public void PrintElapsed (string name, TimeSpan ts)
		{
			var elapsedTime =
				String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
							  ts.Hours, ts.Minutes, ts.Seconds,
							  ts.Milliseconds/10);
			Log(string.Format("Elapsed time for {0}: {1}", name, elapsedTime));
		}

		public IEnumerable<DirectoryInfo> GetSearchDirs(DirectoryInfo startDir)
		{
			var searchDirs = new List<DirectoryInfo>();
			if (IsSearchDirectory(startDir))
			{
				searchDirs.Add(startDir);
			}
			if (Settings.Recursive)
			{
				searchDirs.AddRange(RecGetSearchDirs(startDir));
			}
			return searchDirs;
		}

		private IEnumerable<DirectoryInfo> RecGetSearchDirs(DirectoryInfo dir)
		{
			IEnumerable<DirectoryInfo> searchDirs = new List<DirectoryInfo>();
			try
			{
				searchDirs = dir.EnumerateDirectories().Where(IsSearchDirectory);
				return searchDirs.Aggregate(searchDirs, (current, d) => current.Concat(RecGetSearchDirs(d)));
			}
			catch (IOException e)
			{
				if (Settings.Verbose)
					Log(String.Format("Error while accessing dir {0}: {1}",
						FileUtil.GetRelativePath(dir.FullName), e.Message));
			}
			return searchDirs;
		}
		private bool IsValidSearchFile(FileInfo f)
		{
			return
				(_fileUtil.IsArchiveFile(f) && Settings.SearchArchives && IsArchiveSearchFile(f))
				||
				(!Settings.ArchivesOnly && IsSearchFile(f));
		}

		private SearchFile SearchFileFromFileInfo(FileInfo f)
		{
			return new SearchFile(new List<string>(), f.DirectoryName, f.Name, _fileUtil.GetFileType(f));
		}

		private IEnumerable<SearchFile> GetSearchFilesForDir(DirectoryInfo dir)
		{
			if (Settings.Debug)
			{
				Log(string.Format("Getting search files under {0}",
					FileUtil.GetRelativePath(dir.FullName)));
			}
			IEnumerable<SearchFile> dirSearchFiles = new List<SearchFile>();
			try
			{
				dirSearchFiles = dir.EnumerateFiles().
					Where(IsValidSearchFile).
					Select((f,i) => SearchFileFromFileInfo(f));
			}
			catch (IOException e)
			{
				if (Settings.Verbose)
					Log(String.Format("Error while accessing dir {0}: {1}",
						FileUtil.GetRelativePath(dir.FullName), e.Message));
			}
			return dirSearchFiles;
		}

		public IEnumerable<SearchFile> GetSearchFiles(IEnumerable<DirectoryInfo> dirs)
		{
			var searchFiles = new List<SearchFile>();
			foreach (var d in dirs)
			{
				searchFiles.AddRange(GetSearchFilesForDir(d));
			}
			return searchFiles;
		}

		public void Search()
		{
			if (Settings.DoTiming)
			{
				StartTimer("GetSearchDirs");
			}
			var startDir = new DirectoryInfo(Settings.StartPath);
			var searchDirs = new List<DirectoryInfo>();
			searchDirs.AddRange(GetSearchDirs(startDir));
			if (Settings.DoTiming)
			{
				StopTimer("GetSearchDirs");
			}
			if (Settings.Verbose)
			{
				Log(string.Format("Directories to be searched ({0}):", searchDirs.Count));
				foreach (var d in searchDirs)
				{
					Log(FileUtil.GetRelativePath(d.FullName));
				}
				Log("");
			}

			if (Settings.DoTiming)
			{
				StartTimer("GetSearchFiles");
			}
			var searchFiles = GetSearchFiles(searchDirs);
			if (Settings.DoTiming)
			{
				StopTimer("GetSearchFiles");
			}
			if (Settings.Verbose)
			{
				Log(string.Format("\nFiles to be searched ({0}):", searchFiles.Count()));
				foreach (var f in searchFiles)
				{
					Log(FileUtil.GetRelativePath(f.FullName));
				}
				Log("");
			}

			if (Settings.DoTiming) {
				StartTimer("SearchFiles");
			}
			foreach (var f in searchFiles)
			{
				DoSearchFile(f);
			}
			if (Settings.DoTiming)
			{
				StopTimer("SearchFiles");
			}
		}

		public void DoSearchFile(SearchFile f)
		{
			if (f.Type == FileType.Text)
			{
				SearchTextFile(f);
			}
			else if (f.Type == FileType.Binary)
			{
				SearchBinaryFile(f);
			}
			else if (f.Type == FileType.Archive)
			{
				Log(string.Format("Skipping archive file {0}",
					FileUtil.GetRelativePath(f.FullName)));
			}
			else if (Settings.Verbose)
			{
				Log(string.Format("Skipping file {0}",
					FileUtil.GetRelativePath(f.FullName)));
			}
		}

		private void SearchTextFile(SearchFile f)
		{
			if (Settings.Verbose)
				Log(string.Format("Searching text file {0}",
					FileUtil.GetRelativePath(f.FullName)));
			if (Settings.MultiLineSearch)
				SearchTextFileContents(f);
			else
				SearchTextFileLines(f);
		}

		private void SearchTextFileContents(SearchFile f)
		{
			try
			{
				using (var sr = new StreamReader(f.FullName))
				{
					var contents = sr.ReadToEnd();
					var results = SearchContents(contents);
					foreach (SearchResult r in results)
					{
						r.File = f;
						AddSearchResult(r);
					}
				}
			}
			catch (IOException e)
			{
				Log(e.Message);
			}
		}

		private int CountNewlines(string text)
		{
			return text.Count(c => c == '\n');
		}

		private int GetStartLineIndex(string text, int matchIndex)
		{
			var startIndex = matchIndex;
			while (startIndex > 0 && text[startIndex-1] != '\n')
				startIndex--;
			return startIndex;
		}

		private int GetEndLineIndex(string text, int matchIndex)
		{
			var endIndex = matchIndex;
			while (endIndex < text.Length && text[endIndex] != '\r' &&
				text[endIndex] != '\n')
				endIndex++;
			return endIndex;
		}

		private IEnumerable<SearchResult> SearchContents(string contents)
		{
			var patternMatches = new Dictionary<Regex, int>();
			var results = new List<SearchResult>();

			foreach (var p in Settings.SearchPatterns)
			{
				var match = p.Match(contents);
				while (match.Success)
				{
					//TODO: add lineNum and line retrieval from contents
					var lineNum = CountNewlines(contents.Substring(0, match.Index)) + 1;
					var startIndex = GetStartLineIndex(contents, match.Index);
					var endIndex = GetEndLineIndex(contents, match.Index);
					var line = contents.Substring(startIndex, endIndex - startIndex);
					results.Add(new SearchResult(
						p,
						null,
						lineNum,
						match.Index - startIndex + 1,
						match.Index - startIndex + match.Length + 1,
						line,
						new List<string>(),
						new List<string>()));
					patternMatches[p] = 1;

					if (Settings.FirstMatch && patternMatches.ContainsKey(p))
					{
						break;
					}
					match = match.NextMatch();
				}
			}
			return results;
		}

		private void SearchTextFileLines(SearchFile f)
		{
			try
			{
				var enumerableLines = EnumerableStringFromFile(f);
				var results = SearchLines(enumerableLines);

				foreach (var r in results)
				{
					r.File = f;
					AddSearchResult(r);
				}
			}
			catch (IOException e)
			{
				Log(e.Message);
			}
		}

		private static IEnumerable<string> EnumerableStringFromFile(SearchFile f)
		{
			string line;
			//using (var file = System.IO.File.OpenText(fileName))
			using (var sr = new StreamReader(f.FullName))
			{
				// read each line, ensuring not null (EOF)
				while ((line = sr.ReadLine()) != null)
				{
					// return trimmed line
					yield return line;
				}
			}
		}

		private bool AnyMatchesAnyPattern(IEnumerable<string> strings,
			ICollection<Regex> patterns)
		{
			return strings.Any(s => MatchesAnyPattern(s, patterns));
		}

		private bool MatchesAnyPattern(string s, ICollection<Regex> patterns)
		{
			return !string.IsNullOrEmpty(s) && patterns.Any(p => p.Match(s).Success);
		}

		private bool LinesMatch(IEnumerable<string> lines,
			ICollection<Regex> inPatterns, ICollection<Regex> outPatterns)
		{
			return ((inPatterns.Count == 0 || AnyMatchesAnyPattern(lines, inPatterns))
				&& (outPatterns.Count == 0 || !AnyMatchesAnyPattern(lines, outPatterns)));
		}

		private bool LinesBeforeMatch(IEnumerable<string> linesBefore)
		{
			return LinesMatch(linesBefore, Settings.InLinesBeforePatterns,
				Settings.OutLinesBeforePatterns);
		}

		private bool LinesAfterMatch(IEnumerable<string> linesAfter)
		{
			return LinesMatch(linesAfter, Settings.InLinesAfterPatterns,
				Settings.OutLinesAfterPatterns);
		}

		private IEnumerable<SearchResult> SearchLines(IEnumerable<string> lines)
		{
			var patternMatches = new Dictionary<Regex, int>();
			var results = new List<SearchResult>();
			var lineNum = 0;
			var linesBefore = new Queue<string>();
			var linesAfter = new Queue<string>();
			var lineEnumerator = lines.GetEnumerator();

			while (lineEnumerator.MoveNext() || linesAfter.Count > 0)
			{
				lineNum++;
				var line = linesAfter.Count > 0 ? linesAfter.Dequeue() : lineEnumerator.Current;
				if (Settings.LinesAfter > 0)
				{
					while (linesAfter.Count < Settings.LinesAfter && lineEnumerator.MoveNext())
					{
						linesAfter.Enqueue(lineEnumerator.Current);
					}
				}

				if ((Settings.LinesBefore == 0 || linesBefore.Count == 0 || LinesBeforeMatch(linesBefore))
					&&
					(Settings.LinesAfter == 0 || linesAfter.Count == 0 || LinesAfterMatch(linesAfter)))
				{
					foreach (var p in Settings.SearchPatterns)
					{
						var matches = p.Matches(line);
						foreach (Match match in matches)
						{
							if (Settings.FirstMatch && patternMatches.ContainsKey(p))
							{
								continue;
							}
							results.Add(new SearchResult(p,
								null,
								lineNum,
								match.Index + 1,
								match.Index + match.Length + 1,
								line,
								new List<string>(linesBefore),
								new List<string>(linesAfter)));
							patternMatches[p] = 1;
						}
					}
				}
				if (Settings.LinesBefore > 0)
				{
					if (linesBefore.Count == Settings.LinesBefore)
					{
						linesBefore.Dequeue();
					}
					if (linesBefore.Count < Settings.LinesBefore)
					{
						linesBefore.Enqueue(line);
					}
				}
			}
			return results;
		}

		// TODO: switch to use SearchLines with buffering
		private void SearchBinaryFile(SearchFile f)
		{
			if (Settings.Verbose)
				Log(string.Format("Searching binary file {0}",
					FileUtil.GetRelativePath(f.FullName)));
			try
			{
				using (var sr = new StreamReader(f.FullName))
				{
					var contents = sr.ReadToEnd();
					foreach (var p in Settings.SearchPatterns.Where(p => p.Match(contents).Success)) {
						AddSearchResult(new SearchResult(p, f, 0, 0, 0, null,
							new List<string>(), new List<string>()));
					}
				}
			}
			catch (IOException e)
			{
				Log(e.Message);
			}
		}

		private void AddSearchResult(SearchResult searchResult)
		{
			Results.Add(searchResult);
		}

		public void PrintResults()
		{
			Log(string.Format("Search results ({0}):", Results.Count));
			foreach (var searchResult in Results)
			{
				Log(searchResult.ToString());
			}
		}

		public IEnumerable<DirectoryInfo> GetMatchingDirs()
		{
			return new List<DirectoryInfo>(
				Results.Select(r => r.File.FilePath).
				Distinct().Select(d => new DirectoryInfo(d)).
				OrderBy(d => d.FullName));
		}

		public void PrintMatchingDirs()
		{
			var matchingDirs = GetMatchingDirs();
			Log(string.Format("\nDirectories with matches ({0}):", matchingDirs.Count()));
			foreach (var d in matchingDirs)
			{
				Log(FileUtil.GetRelativePath(d.FullName));
			}
		}

		public IEnumerable<FileInfo> GetMatchingFiles()
		{
			return new List<FileInfo>(
				Results.Select(r => r.File.PathAndName).
				Distinct().Select(f => new FileInfo(f)).
				OrderBy(d => d.FullName));
		}

		public void PrintMatchingFiles()
		{
			var matchingFiles = GetMatchingFiles();
			Log(string.Format("\nFiles with matches ({0}):", matchingFiles.Count()));
			foreach (var f in matchingFiles)
			{
				Log(FileUtil.GetRelativePath(f.FullName));
			}
		}

		public IEnumerable<string> GetMatchingLines()
		{
			return Results.Select(r => r.Line.Trim()).ToList();
		}

		public void PrintMatchingLines()
		{
			var matchingLines = GetMatchingLines();
			Log(string.Format("\nLines with matches ({0}):", matchingLines.Count()));
			foreach (var m in matchingLines)
			{
				Log(m);
			}
		}
	}
}
