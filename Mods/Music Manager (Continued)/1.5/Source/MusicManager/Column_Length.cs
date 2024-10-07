using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace MusicManager;

public class Column_Length : SongTableColumn
{
	public override bool Filtered => false;

	public override string HeaderTooltip => I18n.LengthColumn + "\n\n" + I18n.LengthColumn_Tip;

	public Column_Length(int width)
		: base(width)
	{
	}

	public override int Compare(SongDef a, SongDef b)
	{
		return a.clip.length.CompareTo(b.clip.length);
	}

	public override void DrawCell(Rect canvas, SongDef song)
	{
		base.DrawCell(canvas, song);
		Text.Anchor = TextAnchor.MiddleRight;
		Widgets.Label(canvas, song.clip.length.ToStringTime().Italic());
		Text.Anchor = TextAnchor.UpperLeft;
	}

	public override void DrawHeader(Rect canvas, List<SongDef> songs)
	{
		if (Utilities.DrawButton(canvas))
		{
			Window_MusicManager.SortBy = this;
		}
		DrawOverlay(canvas);
		TooltipHandler.TipRegion(canvas, () => HeaderTooltip, GetHashCode());
	}

	public override Color Color(SongDef song)
	{
		if (song != MusicManager.CurrentSong)
		{
			return UnityEngine.Color.white;
		}
		return GenUI.MouseoverColor;
	}

	public override bool Filter(SongDef song)
	{
		return true;
	}
}
