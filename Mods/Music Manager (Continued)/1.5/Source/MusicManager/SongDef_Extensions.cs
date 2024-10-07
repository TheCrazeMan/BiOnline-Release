using Verse;

namespace MusicManager;

public static class SongDef_Extensions
{
	public static bool Enabled(this SongDef song)
	{
		return !MusicManager.SongDatabase[song].disabled;
	}
}
