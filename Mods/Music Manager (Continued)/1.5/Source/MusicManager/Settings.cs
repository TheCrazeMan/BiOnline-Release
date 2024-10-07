using UnityEngine;
using RimWorld;
using Verse;

namespace MusicManager;

public class Settings : ModSettings
{
	public bool Locked;

	public Vector2 WidgetPosition = new Vector2(GameComp_MusicManager.Size.x, 0f);

	public FloatRange SongIntervalPeace = new FloatRange(85f, 105f);

	public FloatRange SongIntervalWar = new FloatRange(2f, 5f);

	public WidgetAnchor WidgetAnchor = WidgetAnchor.TopRight;

	public virtual void DoWindowContents(Rect canvas)
	{
		Listing_Standard listing_Standard = new Listing_Standard();
		listing_Standard.Begin(canvas);
		listing_Standard.CheckboxLabeled(I18n.LockWidgetPosition, ref Locked, I18n.LockWidgetPositionTooltip);
		listing_Standard.Gap();
		listing_Standard.Label(I18n.SongIntervalPeace, -1f, I18n.SongIntervalPeace_Tip);
		Widgets.FloatRange(listing_Standard.GetRect(Text.LineHeight), 1, ref SongIntervalPeace, 0f, 360f, null, ToStringStyle.Integer, 0f, GameFont.Tiny);
		listing_Standard.Label(I18n.SongIntervalWar, -1f, I18n.SongIntervalWar_Tip);
		Widgets.FloatRange(listing_Standard.GetRect(Text.LineHeight), 2, ref SongIntervalWar, 0f, 360f, null, ToStringStyle.Integer, 0f, GameFont.Tiny);
		listing_Standard.Gap();
		if (listing_Standard.ButtonText(I18n.ResetCustomMetaData))
		{
			MusicManager.SongDatabase.ResetCustomMetaData();
		}        
        listing_Standard.End();
        Mod.WriteSettings();
    }

	public override void ExposeData()
	{
		Scribe_Values.Look(ref Locked, "locked", defaultValue: true);
		Scribe_Values.Look(ref WidgetPosition, "position", new Vector2(GameComp_MusicManager.Size.x, 0f));
		Scribe_Values.Look(ref WidgetAnchor, "anchor", WidgetAnchor.TopRight);
		Scribe_Values.Look(ref SongIntervalPeace, "SongIntervalPeace", new FloatRange(85f, 105f));
		Scribe_Values.Look(ref SongIntervalWar, "SongIntervalWar", new FloatRange(2f, 5f));
	}
}
