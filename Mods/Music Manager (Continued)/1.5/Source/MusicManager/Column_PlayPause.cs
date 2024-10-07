using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace MusicManager;

public class Column_PlayPause : SongTableColumn
{
	private readonly float _iconSize;

	public override bool Filtered { get; }

	public override string HeaderTooltip { get; }

	public Column_PlayPause(int width)
		: base(width)
	{
		_iconSize = (float)(width * 2) / 3f;
	}

	public override int Compare(SongDef a, SongDef b)
	{
		return 0;
	}

	public override void DrawCell(Rect canvas, SongDef song)
	{
		base.DrawCell(canvas, song);
		Utilities.DrawPlayButton(canvas, song, _iconSize);
	}

	public override void DrawHeader(Rect canvas, List<SongDef> songs)
	{
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
