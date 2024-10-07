using HarmonyLib;
using RimWorld;

namespace MusicManager.Patches;

[HarmonyPatch(typeof(MusicManagerPlay), "ForcePlaySong")]
public class MusicManagerPlay_ForceStartSong
{
	public static void Prefix()
	{
		
			MusicManager.Resume();
		
	}
}
