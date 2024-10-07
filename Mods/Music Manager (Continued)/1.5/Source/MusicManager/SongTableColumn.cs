using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace MusicManager;

public abstract class SongTableColumn
{
	public virtual Vector2 IconSize { get; set; } = new Vector2(20f, 20f);


	public virtual int Width { get; set; }

	public abstract bool Filtered { get; }

	public abstract string HeaderTooltip { get; }

	protected SongTableColumn(int width)
	{
		Width = width;
	}

	public abstract int Compare(SongDef a, SongDef b);

	public virtual void DrawCell(Rect canvas, SongDef song)
	{
		GUI.color = Color(song);
	}

	public abstract void DrawHeader(Rect canvas, List<SongDef> songs);

	public abstract bool Filter(SongDef song);

	public virtual Color Color(SongDef song)
	{
		return UnityEngine.Color.white;
	}

	public virtual void DrawOverlay(Rect canvas)
	{
		if (Window_MusicManager.SortBy == this)
		{
			GUI.DrawTextureWithTexCoords(canvas.RightPartPixels(12f).BottomPartPixels(12f), Window_MusicManager.SortDescending ? Resources.ArrowDown : Resources.ArrowUp, new Rect(0f, 0f, 12f / (float)Resources.ArrowDown.width, 12f / (float)Resources.ArrowDown.height).CenteredIn(new Rect(0f, 0f, 1f, 1f)));
		}
		if (Filtered)
		{
			GUI.DrawTexture(canvas.RightPartPixels(12f).TopPartPixels(12f), Resources.Funnel);
		}
	}
}
