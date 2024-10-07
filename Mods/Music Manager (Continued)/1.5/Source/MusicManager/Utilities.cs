using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RimWorld;
using UnityEngine;
using Verse;

namespace MusicManager;

public static class Utilities
{
	private static readonly Regex _songPathNameRegex = new Regex("[^\\/]+$");

	public static List<Season> NewAllSeasonsList => new List<Season>
	{
		Season.Spring,
		Season.Summer,
		Season.Fall,
		Season.Winter,
		Season.PermanentSummer,
		Season.PermanentWinter
	};

	public static bool Allowed(this SongDef song, Season season)
	{
		return song.allowedSeasons.Allowed(season);
	}

	public static bool Allowed(this List<Season> seasons, Season season)
	{
		if (seasons.NullOrEmpty() || season == Season.Undefined)
		{
			return true;
		}
		switch (season)
		{
		case Season.Spring:
		case Season.Fall:
			return seasons.Contains(season);
		case Season.Summer:
		case Season.PermanentSummer:
			if (!seasons.Contains(Season.Summer))
			{
				return seasons.Contains(Season.PermanentSummer);
			}
			return true;
		case Season.Winter:
		case Season.PermanentWinter:
			if (!seasons.Contains(Season.Winter))
			{
				return seasons.Contains(Season.PermanentWinter);
			}
			return true;
		case Season.Undefined:
			return true;
		default:
			throw new ArgumentOutOfRangeException("season", season, null);
		}
	}

	public static bool Allowed(this SongDef song, TimeOfDay time)
	{
		if (song.allowedTimeOfDay != TimeOfDay.Any && time != TimeOfDay.Any)
		{
			return time == song.allowedTimeOfDay;
		}
		return true;
	}

	public static bool Alt()
	{
		if (!Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.AltGr))
		{
			return Input.GetKey(KeyCode.RightAlt);
		}
		return true;
	}

	public static string Bold(this string msg)
	{
		return "<b>" + msg + "</b>";
	}

	public static Rect CenteredIn(this Rect canvas, Rect other)
	{
		canvas.center = other.center;
		return canvas;
	}

	public static bool DrawButton(Rect canvas, Texture icon, float? iconSize = null)
	{
		if (!iconSize.HasValue)
		{
			iconSize = (int)Mathf.Min(canvas.width, canvas.height);
		}
		GUI.DrawTexture(new Rect(0f, 0f, iconSize.Value, iconSize.Value).CenteredOnXIn(canvas).CenteredOnYIn(canvas), icon);
		return DrawButton(canvas);
	}

	public static bool DrawButton(Rect canvas)
	{
		Widgets.DrawHighlightIfMouseover(canvas);
		return Widgets.ButtonInvisible(canvas);
	}

	public static void DrawButton(Rect canvas, Texture icon, Action action, float? iconSize = null)
	{
		if (DrawButton(canvas, icon, iconSize))
		{
			action();
		}
	}

	public static void DrawPlayButton(Rect canvas, SongDef song = null, float size = 24f)
	{
		if (song == null || MusicManager.AudioSource.clip == song.clip)
		{
			if (MusicManager.AudioSource.isPlaying && !MusicManager.IsPaused)
			{
				DrawButton(canvas, Resources.Pause, MusicManager.Pause, size);
				return;
			}
			if (MusicManager.IsPaused)
			{
				DrawButton(canvas, Resources.Play, MusicManager.Resume, size);
				return;
			}
			DrawButton(canvas, Resources.Play, delegate
			{
				//MusicManager.Play();
                MusicManager.Play(song);
            }, size);
		}
		else
		{
			DrawButton(canvas, (size > 24f) ? Resources.PlayXL : Resources.Play, delegate
			{
				MusicManager.Play(song);
			}, size);
		}
	}

	public static void DrawSeekBar(Rect canvas)
	{
		float pct = MusicManager.AudioSource.time / MusicManager.AudioSource.clip.length;
		GUI.DrawTexture(canvas, Resources.SeekBackgroundTexture);
		GUI.DrawTexture(canvas.LeftPart(pct), Resources.SeekForegroundTexture);
		Widgets.DrawHighlightIfMouseover(canvas);
		if (Event.current.type != EventType.Repaint && Input.GetMouseButton(0) && Mouse.IsOver(canvas))
		{
			float num = Mathf.Clamp01((Event.current.mousePosition.x - canvas.xMin) / canvas.width);
			MusicManager.AudioSource.time = num * MusicManager.AudioSource.clip.length;
			MusicManager.Seeking = true;
			Event.current.Use();
		}
	}

	public static Rect HorizontalMidPartPixels(this Rect canvas, float width)
	{
		float num = (canvas.width - width) / 2f;
		return canvas.HorizontalMidPartPixels(num, num);
	}

	public static Rect HorizontalMidPartPixels(this Rect canvas, float leftMargin, float rightMargin)
	{
		return canvas.RightPartPixels(canvas.width - leftMargin).LeftPartPixels(canvas.width - leftMargin - rightMargin);
	}

	public static string Italic(this string msg)
	{
		return "<i>" + msg + "</i>";
	}

	public static string Name(this SongDef def)
	{
		if (!def.label.NullOrEmpty())
		{
			return def.label.CapitalizeFirst();
		}
		Match match = _songPathNameRegex.Match(def.clipPath);
		if (!match.Success)
		{
			return def.clipPath;
		}
		return match.Groups[0].Value.Replace('_', ' ');
	}

	public static List<Season> SetAllowed(this List<Season> seasons, Season season, bool allowed)
	{
		if (seasons == null)
		{
			seasons = NewAllSeasonsList;
		}
		if (allowed && !seasons.Contains(season))
		{
			seasons.Add(season);
			if (season == Season.Summer && !seasons.Contains(Season.PermanentSummer))
			{
				seasons.Add(Season.PermanentSummer);
			}
			if (season == Season.Winter && !seasons.Contains(Season.PermanentWinter))
			{
				seasons.Add(Season.PermanentWinter);
			}
		}
		if (!allowed)
		{
			if (seasons.Contains(season))
			{
				seasons.Remove(season);
			}
			if (season == Season.Summer && seasons.Contains(Season.PermanentSummer))
			{
				seasons.Remove(Season.PermanentSummer);
			}
			if (season == Season.Winter && seasons.Contains(Season.PermanentWinter))
			{
				seasons.Remove(Season.PermanentWinter);
			}
		}
		if (!seasons.Any())
		{
			seasons = NewAllSeasonsList;
		}
		Window_MusicManager.SetDirty();
		return seasons;
	}

	public static void SetAllowed(this SongDef song, Season season, bool allowed)
	{
		song.allowedSeasons = song.allowedSeasons.SetAllowed(season, allowed);
	}

	public static void SetAllowed(this SongDef song, TimeOfDay time)
	{
		if (song.allowedTimeOfDay != time)
		{
			song.allowedTimeOfDay = time;
			Window_MusicManager.SetDirty();
		}
	}

	public static bool Shift()
	{
		if (!Input.GetKey(KeyCode.LeftShift))
		{
			return Input.GetKey(KeyCode.RightShift);
		}
		return true;
	}

	public static string ToStringTime(this float time)
	{
		int num = Mathf.FloorToInt(time / 60f);
		int num2 = Mathf.RoundToInt(time % 60f);
		return $"{num}:{num2:D2}";
	}

	public static Rect VerticalMidPartPixels(this Rect canvas, float height)
	{
		float num = (canvas.height - height) / 2f;
		return canvas.VerticalMidPartPixels(num, num);
	}

	public static Rect VerticalMidPartPixels(this Rect canvas, float topMargin, float bottomMargin)
	{
		return canvas.TopPartPixels(canvas.height - bottomMargin).BottomPartPixels(canvas.height - topMargin - bottomMargin);
	}

	public static Rect WidthContractedBy(this Rect canvas, float margin)
	{
		canvas.xMin += margin;
		canvas.xMax -= margin;
		return canvas;
	}
}
