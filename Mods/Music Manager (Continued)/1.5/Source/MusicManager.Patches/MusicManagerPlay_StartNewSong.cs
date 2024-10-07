using HarmonyLib;
using RimWorld;

namespace MusicManager.Patches;

[HarmonyPatch(typeof(MusicManagerPlay), "StartNewSong")]
public class MusicManagerPlay_StartNewSong
{
	public static bool Prefix()
	{
		return !MusicManager.IsPaused;
	}

	public static void Postfix()
	{
		MusicManager.AudioSource.time = 0f;
	}
}
