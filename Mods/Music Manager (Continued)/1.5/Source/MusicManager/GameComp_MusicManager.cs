using System;
using UnityEngine;
using Verse;

namespace MusicManager;

public class GameComp_MusicManager : GameComponent
{
	public const int Margin = 6;

	public const int RowHeight = 24;

	public const int SeekBarHeight = 6;

	public const int WidgetWidth = 150;

	private static bool _dragging;

	private static Vector2 _mousePos = Vector2.zero;

	public static bool Active => GenScene.InPlayScene;

	public static Vector2 Position
	{
		get
		{
			Vector2 widgetPosition = MusicManager.Settings.WidgetPosition;
			return MusicManager.Settings.WidgetAnchor switch
			{
				WidgetAnchor.TopLeft => widgetPosition, 
				WidgetAnchor.TopRight => new Vector2(Screen.x - widgetPosition.x, widgetPosition.y), 
				WidgetAnchor.BottomLeft => new Vector2(widgetPosition.x, Screen.y - widgetPosition.y), 
				WidgetAnchor.BottomRight => Screen - widgetPosition, 
				_ => throw new ArgumentOutOfRangeException(), 
			};
		}
		set
		{
			Vector2 vector = new Vector2(Mathf.Clamp(value.x, 0f, Screen.x - Size.x), Mathf.Clamp(value.y, 0f, Screen.y - Size.y));
			if (vector.x + Size.x / 2f > Screen.x / 2f)
			{
				if (vector.y + Size.y / 2f > Screen.y / 2f)
				{
					MusicManager.Settings.WidgetAnchor = WidgetAnchor.BottomRight;
					MusicManager.Settings.WidgetPosition = Screen - vector;
				}
				else
				{
					MusicManager.Settings.WidgetAnchor = WidgetAnchor.TopRight;
					MusicManager.Settings.WidgetPosition = new Vector2(Screen.x - vector.x, vector.y);
				}
			}
			else if (vector.y + Size.y / 2f > Screen.y / 2f)
			{
				MusicManager.Settings.WidgetAnchor = WidgetAnchor.BottomLeft;
				MusicManager.Settings.WidgetPosition = new Vector2(vector.x, Screen.y - vector.y);
			}
			else
			{
				MusicManager.Settings.WidgetAnchor = WidgetAnchor.TopLeft;
				MusicManager.Settings.WidgetPosition = vector;
			}
		}
	}

	public static Vector2 Screen => new Vector2(UI.screenWidth, UI.screenHeight);

	public static Vector2 Size => new Vector2(150f, 54f);

	private static bool Dragging
	{
		get
		{
			return _dragging;
		}
		set
		{
			_dragging = value;
			if (!_dragging)
			{
				MusicManager.Settings.Write();
			}
		}
	}

	public GameComp_MusicManager()
	{
	}

	public GameComp_MusicManager(Game game)
	{
	}

	private static void HandleDragging(Rect canvas)
	{
		Widgets.DrawHighlightIfMouseover(canvas);
		if (Mouse.IsOver(canvas) && Event.current.type == EventType.MouseDown)
		{
			Dragging = true;
			_mousePos = Event.current.mousePosition;
			Event.current.Use();
		}
		if (Dragging && Event.current.type == EventType.MouseUp)
		{
			Dragging = false;
			Event.current.Use();
		}
		if (Dragging && Event.current.type == EventType.MouseDrag)
		{
			Position += Event.current.mousePosition - _mousePos;
			_mousePos = Event.current.mousePosition;
			Event.current.Use();
		}
	}

	public override void GameComponentOnGUI()
	{
		if (Active)
		{
			base.GameComponentOnGUI();
			Rect rect = new Rect(Position, Size);
			Rect rect2 = rect.TopPartPixels(24f);
			if (MusicManager.CurrentSong != null)
			{
				Text.Anchor = TextAnchor.MiddleLeft;
				Widgets.Label(rect2.WidthContractedBy(6f), MusicManager.CurrentSong.Name());
				Text.Anchor = TextAnchor.UpperLeft;
				rect2.y += 24f;
				Utilities.DrawSeekBar(rect2.TopPartPixels(6f).WidthContractedBy(6f));
				rect2.y += 6f;
			}
			DrawButtons(rect2);
			if (!MusicManager.Settings.Locked)
			{
				HandleDragging(rect);
			}
		}
	}

	public override void GameComponentUpdate()
	{
		if (Active)
		{
			base.GameComponentUpdate();
			if (MusicManager.Seeking && MusicManager.LastSeek > 0.1f)
			{
				MusicManager.Resume();
				MusicManager.Seeking = false;
			}
		}
	}

	private void DrawButtons(Rect canvas)
	{
		Rect canvas2 = canvas.RightPart(0.25f);
		Utilities.DrawButton(canvas2, Resources.List, delegate
		{
			Find.WindowStack.Add(new Window_MusicManager());
		}, 24f);
		canvas2.x -= canvas2.width;
		Utilities.DrawButton(canvas2, Resources.Next, MusicManager.Next, 24f);
		canvas2.x -= canvas2.width;
		Utilities.DrawPlayButton(canvas2);
		canvas2.x -= canvas2.width;
		Utilities.DrawButton(canvas2, Resources.Previous, MusicManager.Previous, 24f);
	}
}
