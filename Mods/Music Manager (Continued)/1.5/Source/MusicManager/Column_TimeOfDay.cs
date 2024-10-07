using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace MusicManager;

public class Column_TimeOfDay : SongTableColumn
{
	private TimeOfDay _filterTime = TimeOfDay.Any;

	public TimeOfDay FilterTime
	{
		get
		{
			return _filterTime;
		}
		set
		{
			if (value != _filterTime)
			{
				_filterTime = value;
				Window_MusicManager.SetDirty();
			}
		}
	}

	public override bool Filtered => FilterTime != TimeOfDay.Any;

	public override string HeaderTooltip => I18n.TimeOfDayColumn + "\n\n" + I18n.TimeOfDayColumn_Tip;

	public Column_TimeOfDay(int width)
		: base(width)
	{
	}

	public override int Compare(SongDef a, SongDef b)
	{
		return a.allowedTimeOfDay.CompareTo(b.allowedTimeOfDay);
	}

	public override void DrawCell(Rect canvas, SongDef song)
	{
		base.DrawCell(canvas, song);
		DrawTimeOfDayIcon(new Rect(Vector2.zero, IconSize).CenteredIn(canvas), song.allowedTimeOfDay);
		TooltipHandler.TipRegion(canvas, () => Tooltip(song), song.GetHashCode() ^ canvas.GetHashCode());
		if (Utilities.DrawButton(canvas))
		{
			song.allowedTimeOfDay = (TimeOfDay)((int)(song.allowedTimeOfDay + 1) % 3);
		}
	}

	public string Tooltip(SongDef song)
	{
		return I18n.AllowedTimeOfDay(song.allowedTimeOfDay);
	}

	public override void DrawHeader(Rect canvas, List<SongDef> songs)
	{
		Rect canvas2 = new Rect(Vector2.zero, IconSize).CenteredIn(canvas);
		if (Utilities.Shift())
		{
			TimeOfDay timeOfDay = (songs.All((SongDef s) => s.allowedTimeOfDay == TimeOfDay.Day) ? TimeOfDay.Day : ((!songs.All((SongDef s) => s.allowedTimeOfDay == TimeOfDay.Night)) ? TimeOfDay.Any : TimeOfDay.Night));
			DrawTimeOfDayIcon(canvas2, timeOfDay);
			if (Utilities.DrawButton(canvas))
			{
				timeOfDay = (TimeOfDay)((int)(timeOfDay + 1) % 3);
				foreach (SongDef song in songs)
				{
					song.SetAllowed(timeOfDay);
				}
			}
		}
		else if (Utilities.Alt())
		{
			DrawTimeOfDayIcon(canvas2, FilterTime);
			if (Utilities.DrawButton(canvas))
			{
				FilterTime = (TimeOfDay)((int)(FilterTime + 1) % 3);
			}
		}
		else if (Utilities.DrawButton(canvas, Resources.Night, IconSize.x))
		{
			Window_MusicManager.SortBy = this;
		}
		DrawOverlay(canvas);
		TooltipHandler.TipRegion(canvas, () => HeaderTooltip, GetHashCode());
	}

	public override bool Filter(SongDef song)
	{
		if (FilterTime != TimeOfDay.Any && song.allowedTimeOfDay != TimeOfDay.Any)
		{
			return FilterTime == song.allowedTimeOfDay;
		}
		return true;
	}

	private void DrawTimeOfDayIcon(Rect canvas, TimeOfDay time)
	{
		switch (time)
		{
		case TimeOfDay.Any:
			GUI.DrawTextureWithTexCoords(canvas.LeftHalf(), Resources.Day, new Rect(0f, 0f, 0.5f, 1f));
			GUI.DrawTextureWithTexCoords(canvas.RightHalf(), Resources.Night, new Rect(0.5f, 0f, 0.5f, 1f));
			break;
		case TimeOfDay.Day:
			GUI.DrawTexture(canvas, Resources.Day);
			break;
		case TimeOfDay.Night:
			GUI.DrawTexture(canvas, Resources.Night);
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}
}
