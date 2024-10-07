using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace MusicManager;

public class Column_Seasons : SongTableColumn
{
	private List<Season> _filterSeasons = Utilities.NewAllSeasonsList;

	private bool _filtered;

	public List<Season> FilterSeasons
	{
		get
		{
			return _filterSeasons;
		}
		set
		{
			if (value.NullOrEmpty())
			{
				_filterSeasons = Utilities.NewAllSeasonsList;
			}
			else if (value.Except(_filterSeasons).Any() || _filterSeasons.Except(value).Any())
			{
				_filterSeasons = value;
				_filtered = Utilities.NewAllSeasonsList.Except(FilterSeasons).Any();
				Window_MusicManager.SetDirty();
			}
		}
	}

	public override bool Filtered => _filtered;

	public override string HeaderTooltip => I18n.SeasonsColumn + "\n\n" + I18n.SeasonsColumn_Tip;

	public Column_Seasons(int width)
		: base(width)
	{
	}

	public override int Compare(SongDef a, SongDef b)
	{
		if (a?.allowedSeasons != null || b?.allowedSeasons != null)
		{
			if (a?.allowedSeasons != null)
			{
				if (b?.allowedSeasons != null)
				{
					return a.allowedSeasons.Sum((Season s) => (int)(s + 10)) - b.allowedSeasons.Sum((Season s) => (int)(s + 10));
				}
				return 1;
			}
			return -1;
		}
		return 0;
	}

	public override void DrawCell(Rect canvas, SongDef song)
	{
		base.DrawCell(canvas, song);
		Rect cell = canvas.RightPartPixels(canvas.width / 4f);
		TooltipHandler.TipRegion(cell, () => Tooltip(Season.Winter, song), song.GetHashCode() ^ cell.GetHashCode());
		DrawSongSeason(ref cell, ref song.allowedSeasons, Season.Winter, Resources.WinterOn, Resources.WinterOff);
		TooltipHandler.TipRegion(cell, () => Tooltip(Season.Fall, song), song.GetHashCode() ^ cell.GetHashCode());
		DrawSongSeason(ref cell, ref song.allowedSeasons, Season.Fall, Resources.FallOn, Resources.FallOff);
		TooltipHandler.TipRegion(cell, () => Tooltip(Season.Summer, song), song.GetHashCode() ^ cell.GetHashCode());
		DrawSongSeason(ref cell, ref song.allowedSeasons, Season.Summer, Resources.SummerOn, Resources.SummerOff);
		TooltipHandler.TipRegion(cell, () => Tooltip(Season.Spring, song), song.GetHashCode() ^ cell.GetHashCode());
		DrawSongSeason(ref cell, ref song.allowedSeasons, Season.Spring, Resources.SpringOn, Resources.SpringOff);
	}

	public string Tooltip(Season season, SongDef song)
	{
		return I18n.Season(Season.Winter) + "\n\n" + I18n.AllowedSeasons(song.allowedSeasons);
	}

	public override void DrawHeader(Rect canvas, List<SongDef> songs)
	{
		Rect cell = canvas.RightPartPixels(canvas.width / 4f);
		if (Utilities.Shift())
		{
			DrawMassAssignSeason(ref cell, songs, Season.Winter, Resources.WinterOn, Resources.WinterOff);
			DrawMassAssignSeason(ref cell, songs, Season.Fall, Resources.FallOn, Resources.FallOff);
			DrawMassAssignSeason(ref cell, songs, Season.Summer, Resources.SummerOn, Resources.SummerOff);
			DrawMassAssignSeason(ref cell, songs, Season.Spring, Resources.SpringOn, Resources.SpringOff);
		}
		else if (Utilities.Alt())
		{
			List<Season> seasons = new List<Season>(FilterSeasons);
			DrawSongSeason(ref cell, ref seasons, Season.Winter, Resources.WinterOn, Resources.WinterOff);
			DrawSongSeason(ref cell, ref seasons, Season.Fall, Resources.FallOn, Resources.FallOff);
			DrawSongSeason(ref cell, ref seasons, Season.Summer, Resources.SummerOn, Resources.SummerOff);
			DrawSongSeason(ref cell, ref seasons, Season.Spring, Resources.SpringOn, Resources.SpringOff);
			FilterSeasons = seasons;
		}
		else
		{
			Rect position = new Rect(Vector2.zero, IconSize).CenteredIn(cell);
			GUI.DrawTexture(position, Resources.WinterOn);
			position.x -= cell.width;
			GUI.DrawTexture(position, Resources.FallOn);
			position.x -= cell.width;
			GUI.DrawTexture(position, Resources.SummerOn);
			position.x -= cell.width;
			GUI.DrawTexture(position, Resources.SpringOn);
			if (Utilities.DrawButton(canvas))
			{
				Window_MusicManager.SortBy = this;
			}
		}
		DrawOverlay(canvas);
		TooltipHandler.TipRegion(canvas, () => HeaderTooltip, GetHashCode());
	}

	public void DrawMassAssignSeason(ref Rect cell, List<SongDef> songs, Season season, Texture2D iconOn, Texture2D iconOff)
	{
		if (songs.All((SongDef s) => s.Allowed(season)))
		{
			if (Utilities.DrawButton(cell, iconOn, IconSize.x))
			{
				foreach (SongDef song in songs)
				{
					song.SetAllowed(season, allowed: false);
				}
			}
		}
		else if (songs.Any((SongDef s) => s.Allowed(season)))
		{
			Rect rect = new Rect(Vector2.zero, IconSize).CenteredIn(cell);
			GUI.DrawTextureWithTexCoords(rect.LeftHalf(), iconOff, new Rect(0f, 0f, 0.5f, 1f));
			GUI.DrawTextureWithTexCoords(rect.RightHalf(), iconOn, new Rect(0.5f, 0f, 0.5f, 1f));
			if (Utilities.DrawButton(cell))
			{
				foreach (SongDef item in songs.Where((SongDef s) => !s.Allowed(season)))
				{
					item.SetAllowed(season, allowed: true);
				}
			}
		}
		else if (Utilities.DrawButton(cell, iconOff, IconSize.x))
		{
			foreach (SongDef song2 in songs)
			{
				song2.SetAllowed(season, allowed: true);
			}
		}
		cell.x -= cell.width;
	}

	public void DrawSongSeason(ref Rect cell, ref List<Season> seasons, Season season, Texture2D iconOn, Texture2D iconOff)
	{
		bool flag = seasons.Allowed(season);
		bool flag2 = flag;
		if (Utilities.DrawButton(cell, flag ? iconOn : iconOff, IconSize.x))
		{
			flag2 = !flag2;
		}
		if (flag2 != flag)
		{
			seasons = seasons.SetAllowed(season, flag2);
		}
		cell.x -= cell.width;
	}

	public override bool Filter(SongDef song)
	{
		if (!song.allowedSeasons.NullOrEmpty())
		{
			return song.allowedSeasons.Any((Season s) => FilterSeasons.Contains(s));
		}
		return true;
	}
}
