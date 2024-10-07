using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace MusicManager;

public class Column_Name : SongTableColumn
{
	private string _filter = string.Empty;

	public string FilterQuery
	{
		get
		{
			return _filter;
		}
		set
		{
			string text = value.ToUpperInvariant();
			if (!(text == _filter))
			{
				_filter = text;
				Window_MusicManager.SetDirty();
			}
		}
	}

	public override bool Filtered => !FilterQuery.NullOrEmpty();

	public override string HeaderTooltip => I18n.NameColumn + "\n\n" + I18n.NameColumn_Tip;

	public Column_Name(int width)
		: base(width)
	{
	}

	public override int Compare(SongDef a, SongDef b)
	{
		return string.Compare(a.Name(), b.Name(), StringComparison.Ordinal);
	}

	public override Color Color(SongDef song)
	{
		if (song != MusicManager.CurrentSong)
		{
			return UnityEngine.Color.white;
		}
		return GenUI.MouseoverColor;
	}

	public override void DrawCell(Rect canvas, SongDef song)
	{
		base.DrawCell(canvas, song);
		Text.Anchor = TextAnchor.MiddleLeft;
		Widgets.Label(canvas, song.Name());
		Text.Anchor = TextAnchor.UpperLeft;
	}

	public override void DrawHeader(Rect canvas, List<SongDef> songs)
	{
		Widgets.Label(canvas, I18n.NameColumn);
		if (Utilities.DrawButton(canvas))
		{
			Window_MusicManager.SortBy = this;
		}
		DrawOverlay(canvas);
		TooltipHandler.TipRegion(canvas, () => HeaderTooltip, GetHashCode());
	}

	public override bool Filter(SongDef song)
	{
		if (!FilterQuery.NullOrEmpty())
		{
			return song.Name().ToUpperInvariant().Contains(FilterQuery);
		}
		return true;
	}
}
