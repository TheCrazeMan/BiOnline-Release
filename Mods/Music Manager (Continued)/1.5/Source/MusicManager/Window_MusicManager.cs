using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace MusicManager;

public class Window_MusicManager : Window
{
	public const int ColWidth = 24;

	public const int RowHeight = 24;

	public const int TimeWidth = 80;

	public static Column_Name NameColumn = new Column_Name(0);

	public static Vector2 WindowSize = new Vector2(800f, 640f);

	public static List<SongTableColumn> Columns = new List<SongTableColumn>
	{
		new Column_Gap(6),
		new Column_PlayPause(24),
		NameColumn,
		new Column_Length(80),
		new Column_Gap(6),
		new Column_Tense(24),
		new Column_Gap(6),
		new Column_TimeOfDay(24),
		new Column_Gap(6),
		new Column_Seasons(96),
		new Column_Gap(6)
	};

	private const int _margin = 6;

	private static List<SongDef> _filteredSongs = new List<SongDef>();

	private static SongTableColumn _sortBy;

	private static bool dirty = true;

	public Vector2 songListScrollPos = Vector2.zero;

	public static List<SongDef> FilteredSongs
	{
		get
		{
			if (dirty)
			{
				_filteredSongs = MusicManager.Songs.Where((SongDef s) => Columns.All((SongTableColumn c) => c.Filter(s))).ToList();
				if (_sortBy == null)
				{
					return _filteredSongs;
				}
				if (SortDescending)
				{
					_filteredSongs.SortStable((SongDef a, SongDef b) => _sortBy.Compare(b, a));
				}
				else
				{
					_filteredSongs.SortStable(_sortBy.Compare);
				}
			}
			return _filteredSongs;
		}
	}

	public static Window_MusicManager Instance { get; private set; }

	public static SongTableColumn SortBy
	{
		get
		{
			return _sortBy;
		}
		set
		{
			if (_sortBy == value)
			{
				if (SortDescending)
				{
					_sortBy = null;
					SortDescending = false;
				}
				else
				{
					SortDescending = true;
				}
			}
			else
			{
				_sortBy = value;
				SortDescending = false;
			}
			SetDirty();
		}
	}

	public static bool SortDescending { get; private set; }

	public override Vector2 InitialSize => WindowSize;

	protected override float Margin => 6f;

	public Window_MusicManager()
	{
		Instance = this;
		closeOnAccept = true;
		closeOnCancel = true;
		closeOnClickedOutside = true;
	}

	public static void SetDirty()
	{
		dirty = true;
	}

	public override void DoWindowContents(Rect canvas)
	{
		canvas = canvas.ContractedBy(Margin);
		Rect canvas2 = canvas.TopPartPixels(50f);
		Rect canvas3 = canvas.VerticalMidPartPixels(50f, 50f);
		Rect canvas4 = canvas.BottomPartPixels(50f);
		DrawTitle(canvas2);
		DrawSongList(canvas3);
		DrawSongDetails(canvas4, MusicManager.CurrentSong);
	}

	public void DrawSongDetails(Rect canvas, SongDef song)
	{
		Rect rect = canvas.TopPartPixels(42f);
		Rect rect2 = rect.LeftPartPixels((canvas.width - 100f) / 2f);
		Rect rect3 = rect.HorizontalMidPartPixels(rect2.width, rect2.width);
		Rect rect4 = rect.RightPartPixels(rect2.width);
		Rect canvas2 = canvas.BottomPartPixels(8f);
		if (song == null)
		{
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect2, "---");
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect4, "-:-- / -:--");
		}
		else
		{
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect2, song.Name());
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect4, MusicManager.AudioSource.time.ToStringTime() + " / " + song.clip.length.ToStringTime());
			Utilities.DrawButton(rect3.LeftPartPixels(24f).VerticalMidPartPixels(24f), Resources.Previous, MusicManager.Previous);
			Utilities.DrawPlayButton(rect3.HorizontalMidPartPixels(48f), song, 42f);
			Utilities.DrawButton(rect3.RightPartPixels(24f).VerticalMidPartPixels(24f), Resources.Next, MusicManager.Next);
			Utilities.DrawSeekBar(canvas2);
		}
		Text.Anchor = TextAnchor.UpperLeft;
	}

	public void DrawSongList(Rect canvas)
	{
		Vector2 min = canvas.min;
		List<SongDef> filteredSongs = FilteredSongs;
		foreach (SongTableColumn column in Columns)
		{
			Rect canvas2 = new Rect(min.x, min.y, column.Width, 24f);
			column.DrawHeader(canvas2, filteredSongs);
			min.x += column.Width;
		}
		Rect rect = new Rect(0f, 0f, canvas.width, filteredSongs.Count * 24);
		if (rect.height > canvas.height - 24f)
		{
			rect.width -= 16f;
		}
		NameColumn.Width -= Columns.Sum((SongTableColumn c) => c.Width) - (int)rect.width;
		Widgets.BeginScrollView(canvas.BottomPartPixels(canvas.height - 24f), ref songListScrollPos, rect);
		GUI.DrawTexture(rect, Resources.SlightlyDarkBackgroundColor);
		bool flag = true;
		min = Vector2.zero;
		foreach (SongDef item in filteredSongs)
		{
			Rect position = new Rect(min.x, min.y, rect.width, 24f);
			if (flag)
			{
				GUI.DrawTexture(position, Resources.SlightlyDarkBackgroundColor);
			}
			flag = !flag;
			foreach (SongTableColumn column2 in Columns)
			{
				Rect canvas3 = new Rect(min.x, min.y, column2.Width, 24f);
				column2.DrawCell(canvas3, item);
				min.x += column2.Width;
			}
			min.x = 0f;
			min.y += 24f;
		}
		Widgets.EndScrollView();
	}

	public void DrawTitle(Rect canvas)
	{
		Text.Font = GameFont.Medium;
		Widgets.Label(canvas, I18n.MusicManager);
		Text.Font = GameFont.Small;
		if (Widgets.ButtonImage(canvas.RightPartPixels(24f).TopPartPixels(24f), Resources.Cog, true))
		{
			Find.WindowStack.Add(new Dialog_ModSettings(MusicManager.Instance));
		}
	}

	public override void PreClose()
	{
		base.PreClose();
		MusicManager.SongDatabase.WriteMetaData();
	}
}
