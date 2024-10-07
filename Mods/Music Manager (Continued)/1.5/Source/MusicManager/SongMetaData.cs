using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace MusicManager;

public class SongMetaData : IExposable
{
	public SongDef song;

	public List<Season> seasons;

	public bool tense;

	public TimeOfDay time;

	public bool disabled;

	private SongMetaData original;

	public bool HasCustomMetaData
	{
		get
		{
			GetCurrentMetaData();
			if (disabled)
			{
				return true;
			}
			if (tense != original.tense)
			{
				return true;
			}
			if (time == original.time && seasons?.Count == original.seasons?.Count)
			{
				if (seasons != null || original.seasons != null)
				{
					if (!seasons.Except(original.seasons).Any())
					{
						return original.seasons.Except(seasons).Any();
					}
					return true;
				}
				return false;
			}
			return true;
		}
	}

	public SongMetaData(SongDef song)
	{
		this.song = song;
		NoteOriginal(song);
	}

	public SongMetaData()
	{
	}

	public void NoteOriginal(SongDef song)
	{
		original = new SongMetaData
		{
			seasons = ((song.allowedSeasons == null) ? null : new List<Season>(song.allowedSeasons)),
			time = song.allowedTimeOfDay,
			tense = song.tense
		};
	}

	public void ApplyCustomMetaData()
	{
		song.allowedSeasons = ((seasons == null) ? null : new List<Season>(seasons));
		song.allowedTimeOfDay = time;
		song.tense = tense;
	}

	public void ApplyOriginalMetaData()
	{
		song.allowedSeasons = ((original.seasons == null) ? null : new List<Season>(original.seasons));
		song.allowedTimeOfDay = original.time;
		song.tense = original.tense;
		disabled = false;
	}

	public void GetCurrentMetaData()
	{
		seasons = ((song.allowedSeasons == null) ? null : new List<Season>(song.allowedSeasons));
		time = song.allowedTimeOfDay;
		tense = song.tense;
	}

	public void ExposeData()
	{
		if (Scribe.mode == LoadSaveMode.Saving)
		{
			GetCurrentMetaData();
		}
		Scribe_Defs.Look(ref song, "SongDef");
		Scribe_Collections.Look(ref seasons, "Seasons", LookMode.Undefined);
		Scribe_Values.Look(ref tense, "Tense", defaultValue: false);
		Scribe_Values.Look(ref time, "TimeOfDay", TimeOfDay.Night);
		Scribe_Values.Look(ref disabled, "Disabled", defaultValue: false);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			NoteOriginal(song);
			ApplyCustomMetaData();
		}
	}
}
