using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using RimWorld;
using UnityEngine;
using Verse;

namespace MusicManager;

public class MusicManager : Mod
{
	private static readonly MethodInfo _appropriateNowMethodInfo;

	private static readonly AccessTools.FieldRef<MusicManagerPlay, AudioSource> _audioSourceRef;

	//private static readonly AccessTools.FieldRef<MusicManagerPlay, SongDef> _lastStartedSongRef;
    private static readonly AccessTools.FieldRef<MusicManagerPlay, SongDef> _currentSongRef;

    public static SongDatabase SongDatabase;

	private static bool _seeking;

	private static float _lastSeek;

	public static List<SongDef> AppropriateSongs => Songs.Where(AppropriateNow).ToList();

	public static AudioSource AudioSource => _audioSourceRef(Find.MusicManagerPlay);

	public static SongDef CurrentSong
	{
		get
		{
			AudioSource audioSource = AudioSource;
			if (!(((object)audioSource != null) ? (!audioSource.isPlaying) : (!IsPaused)))
			{
				return _currentSongRef(Find.MusicManagerPlay);
			}
			return null;
		}
	}

	public static MusicManager Instance { get; private set; }

	public static bool IsPaused { get; private set; }

	public static Settings Settings { get; private set; }

	public static List<SongDef> Songs => DefDatabase<SongDef>.AllDefsListForReading;

	public static bool Seeking
	{
		get
		{
			return _seeking;
		}
		set
		{
			if (value)
			{
				Pause();
				LastSeek = Time.time;
			}
			_seeking = value;
		}
	}

	public static float LastSeek
	{
		get
		{
			return Time.time - _lastSeek;
		}
		set
		{
			_lastSeek = value;
		}
	}

	static MusicManager()
	{
		_appropriateNowMethodInfo = AccessTools.Method(typeof(MusicManagerPlay), "AppropriateNow");
		_audioSourceRef = AccessTools.FieldRefAccess<AudioSource>(typeof(MusicManagerPlay), "audioSource");		
        //_lastStartedSongRef = AccessTools.FieldRefAccess<SongDef>(typeof(MusicManagerPlay), "lastStartedSong");
        _currentSongRef = AccessTools.FieldRefAccess<SongDef>(typeof(MusicManagerPlay), "currentSong");
        if (_appropriateNowMethodInfo == null)
		{
			Log.Error("AppropriateNow MethodInfo not found");
		}
		if (_audioSourceRef == null)
		{
			Log.Error("AudioSource Ref null!");
		}
		if (_currentSongRef == null)
		{
			Log.Error("LastStartedSong Ref null!");
		}
	}

	public MusicManager(ModContentPack content)
		: base(content)
	{
		Settings = GetSettings<Settings>();
		ApplySongInterval();
		Instance = this;
		SongDatabase = new SongDatabase();
		LongEventHandler.ExecuteWhenFinished(SongDatabase.Initialize);
		new Harmony("Fluffy.GameComp_MusicManager").PatchAll();
	}

	public static bool AppropriateNow(SongDef song)
	{
		return (bool)_appropriateNowMethodInfo.Invoke(Find.MusicManagerPlay, new object[1] { song });
	}

	public static void Next()
	{
		//Play();
		Find.MusicManagerPlay.StartNewSong();

    }

    public static void Pause()
	{
		AudioSource.Pause();
		IsPaused = true;
	}

	public static void Play([CanBeNull]SongDef song = null)
	{
		Find.MusicManagerPlay.ForcePlaySong(song, false);        
        AudioSource.time = 0f;
		Messages.Message("Now playing: " + (CurrentSong?.Name() ?? "NONE"), MessageTypeDefOf.SilentInput);
	}

	public static void Previous()
	{

        AudioSource.time = 0f;

        //if (AudioSource.time < 5f)
		//{
            //Play();
         //   Find.MusicManagerPlay.StartNewSong();
        //}
		//else
		//{
		//	AudioSource.time = 0f;
		//}
	}

	public static void Resume()
	{
		if (MusicManager.IsPaused)
		{
			AudioSource.UnPause();
			IsPaused = false;
		}
		else
		{
            Find.MusicManagerPlay.StartNewSong();
        }
	}

	public override void DoSettingsWindowContents(Rect inRect)
	{
		base.DoSettingsWindowContents(inRect);
		GetSettings<Settings>().DoWindowContents(inRect);
	}

	public override string SettingsCategory()
	{
		return I18n.MusicManager;
	}

	public override void WriteSettings()
	{
		base.WriteSettings();
		ApplySongInterval();
	}

	public void ApplySongInterval()
	{
		typeof(MusicManagerPlay).GetField("SongIntervalTension", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, Settings.SongIntervalWar);
		typeof(MusicManagerPlay).GetField("SongIntervalRelax", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, Settings.SongIntervalPeace);
		if (Current.Root is Root_Play root_Play)
		{
			typeof(MusicManagerPlay).GetField("nextSongStartTime", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(root_Play.musicManagerPlay, 0);
		}
	}
}
