using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace MusicManager;

public class Column_Tense : SongTableColumn
{
	private bool? _filterTense;

	public bool? FilterTense
	{
		get
		{
			return _filterTense;
		}
		set
		{
			if (_filterTense != value)
			{
				_filterTense = value;
				Window_MusicManager.SetDirty();
			}
		}
	}

	public override bool Filtered => FilterTense.HasValue;

	public override string HeaderTooltip => I18n.TenseColumn + "\n\n" + I18n.TenseColumn_Tip;

	public Column_Tense(int width)
		: base(width)
	{
	}

	public override int Compare(SongDef a, SongDef b)
	{
		return a.tense.CompareTo(b.tense);
	}

	public override void DrawCell(Rect canvas, SongDef song)
	{
		base.DrawCell(canvas, song);
		TooltipHandler.TipRegion(canvas, () => Tooltip(song), song.GetHashCode() ^ canvas.GetHashCode());
		if (Utilities.DrawButton(canvas, song.tense ? Resources.Explosion : Resources.Dove, IconSize.x))
		{
			song.tense = !song.tense;
			Window_MusicManager.SetDirty();
		}
	}

	public string Tooltip(SongDef song)
	{
		return I18n.AllowedTense(song.tense);
	}

	public override void DrawHeader(Rect canvas, List<SongDef> songs)
	{
		if (Utilities.Shift())
		{
			if (songs.All((SongDef s) => s.tense))
			{
				if (Utilities.DrawButton(canvas, Resources.Explosion, IconSize.x))
				{
					foreach (SongDef song in songs)
					{
						song.tense = false;
					}
					Window_MusicManager.SetDirty();
				}
			}
			else if (songs.All((SongDef s) => !s.tense))
			{
				if (Utilities.DrawButton(canvas, Resources.Dove, IconSize.x))
				{
					foreach (SongDef song2 in songs)
					{
						song2.tense = true;
					}
					Window_MusicManager.SetDirty();
				}
			}
			else
			{
				Rect rect = new Rect(Vector2.zero, IconSize).CenteredIn(canvas);
				GUI.DrawTextureWithTexCoords(rect.LeftHalf(), Resources.Dove, new Rect(0f, 0f, 0.5f, 1f));
				GUI.DrawTextureWithTexCoords(rect.RightHalf(), Resources.Explosion, new Rect(0.5f, 0f, 0.5f, 1f));
				if (Utilities.DrawButton(rect))
				{
					foreach (SongDef item in songs.Where((SongDef s) => !s.tense))
					{
						item.tense = true;
					}
					Window_MusicManager.SetDirty();
				}
			}
		}
		else if (Utilities.Alt())
		{
			Rect rect2 = new Rect(Vector2.zero, IconSize).CenteredIn(canvas);
			if (FilterTense.HasValue && FilterTense.Value)
			{
				GUI.DrawTexture(rect2, Resources.Explosion);
				if (Utilities.DrawButton(canvas))
				{
					FilterTense = false;
				}
			}
			else if (FilterTense.HasValue && !FilterTense.Value)
			{
				GUI.DrawTexture(rect2, Resources.Dove);
				if (Utilities.DrawButton(canvas))
				{
					FilterTense = null;
				}
			}
			else
			{
				GUI.DrawTextureWithTexCoords(rect2.LeftPart(0.5f), Resources.Dove, new Rect(0f, 0f, 0.5f, 1f));
				GUI.DrawTextureWithTexCoords(rect2.RightPart(0.5f), Resources.Explosion, new Rect(0.5f, 0f, 0.5f, 1f));
				if (Utilities.DrawButton(canvas))
				{
					FilterTense = true;
				}
			}
		}
		else if (Utilities.DrawButton(canvas, Resources.Explosion, IconSize.x))
		{
			Window_MusicManager.SortBy = this;
		}
		DrawOverlay(canvas);
		TooltipHandler.TipRegion(canvas, () => HeaderTooltip, GetHashCode());
	}

	public override bool Filter(SongDef song)
	{
		if (FilterTense.HasValue)
		{
			return song.tense == FilterTense;
		}
		return true;
	}
}
