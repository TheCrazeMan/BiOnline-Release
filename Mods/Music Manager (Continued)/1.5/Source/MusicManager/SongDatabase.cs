using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Verse;

namespace MusicManager;

public class SongDatabase : IExposable
{
	private Dictionary<SongDef, SongMetaData> _database = new Dictionary<SongDef, SongMetaData>();

	private List<SongMetaData> _databaseWorkingList = new List<SongMetaData>();

	public const string RootElement = "SongDatabase";

	public SongMetaData this[SongDef song]
	{
		get
		{
			if (_database.TryGetValue(song, out var value))
			{
				return value;
			}
			_database.Add(song, value = new SongMetaData(song));
			return value;
		}
	}

	public static string SongDatabasePath => Path.Combine(GenFilePaths.ConfigFolderPath, "SongDatabase.xml");

	public void Initialize()
	{
		if (File.Exists(SongDatabasePath))
		{
			try
			{
				Scribe.loader.InitLoading(SongDatabasePath);
				ExposeData();
			}
			catch (Exception arg)
			{
				Verse.Log.Error($"Exception loading song metadata database:\n{arg}");
			}
			finally
			{
				Scribe.loader.FinalizeLoading();
			}
			try
			{
				Scribe.loader.crossRefs.ResolveAllCrossReferences();
			}
			catch (Exception arg2)
			{
				Verse.Log.Error($"Exception resolving references for song metadata database:\n{arg2}");
			}
			try
			{
				Scribe.loader.initer.DoAllPostLoadInits();
			}
			catch (Exception arg3)
			{
				Verse.Log.Error($"Exception initializing song metadata database:\n{arg3}");
			}
		}
		foreach (SongDef item in DefDatabase<SongDef>.AllDefsListForReading)
		{
			if (!_database.ContainsKey(item))
			{
				_database.Add(item, new SongMetaData(item));
			}
		}
	}

	public void WriteMetaData()
	{
		try
		{
			Scribe.saver.InitSaving(SongDatabasePath, "SongDatabase");
			ExposeData();
		}
		catch (Exception arg)
		{
			Verse.Log.Error($"Exception writing song metadata database:\n{arg}");
		}
		finally
		{
			Scribe.saver.FinalizeSaving();
		}
	}

	public List<SongMetaData> PrunedDatabase()
	{
		_ = _database.Count;
		return (from p in _database
			where p.Value.HasCustomMetaData
			select p.Value).ToList();
	}

	public void ResetCustomMetaData()
	{
		foreach (KeyValuePair<SongDef, SongMetaData> item in _database)
		{
			item.Value.ApplyOriginalMetaData();
		}
		WriteMetaData();
	}

	public void ExposeData()
	{
		if (Scribe.mode == LoadSaveMode.Saving)
		{
			_databaseWorkingList = PrunedDatabase();
		}
		Scribe_Collections.Look(ref _databaseWorkingList, "Database", LookMode.Deep);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			_database = _databaseWorkingList.ToDictionary((SongMetaData m) => m.song, (SongMetaData m) => m);
		}
	}
}
