using UnityEngine;
using Verse;

namespace MusicManager;

[StaticConstructorOnStartup]
public class Resources
{
	public static Texture2D DarkBackgroundColor = SolidColorMaterials.NewSolidColorTexture(new Color(0f, 0f, 0f, 0.8f));

	public static Texture2D List = ContentFinder<Texture2D>.Get("UI/Icons/list");

	public static Texture2D Next = ContentFinder<Texture2D>.Get("UI/Icons/next");

	public static Texture2D Pause = ContentFinder<Texture2D>.Get("UI/Icons/pause");

	public static Texture2D Play = ContentFinder<Texture2D>.Get("UI/Icons/play");

	public static Texture2D Previous = ContentFinder<Texture2D>.Get("UI/Icons/previous");

	public static Texture2D SeekBackgroundTexture = SolidColorMaterials.NewSolidColorTexture(0f, 0f, 0f, 0.3f);

	public static Texture2D SeekForegroundTexture = SolidColorMaterials.NewSolidColorTexture(Color.white);

	public static Texture2D SlightlyDarkBackgroundColor = SolidColorMaterials.NewSolidColorTexture(new Color(0f, 0f, 0f, 0.3f));

	public static Texture2D Stop = ContentFinder<Texture2D>.Get("UI/Icons/stop");

	public static Texture2D Cog = ContentFinder<Texture2D>.Get("UI/Icons/cog");

	public static Texture2D SummerOn = ContentFinder<Texture2D>.Get("UI/Icons/summer-on");

	public static Texture2D SummerOff = ContentFinder<Texture2D>.Get("UI/Icons/summer-off");

	public static Texture2D WinterOn = ContentFinder<Texture2D>.Get("UI/Icons/winter-on");

	public static Texture2D WinterOff = ContentFinder<Texture2D>.Get("UI/Icons/winter-off");

	public static Texture2D SpringOn = ContentFinder<Texture2D>.Get("UI/Icons/spring-on");

	public static Texture2D SpringOff = ContentFinder<Texture2D>.Get("UI/Icons/spring-off");

	public static Texture2D FallOn = ContentFinder<Texture2D>.Get("UI/Icons/fall-on");

	public static Texture2D FallOff = ContentFinder<Texture2D>.Get("UI/Icons/fall-off");

	public static Texture2D Explosion = ContentFinder<Texture2D>.Get("UI/Icons/explosion");

	public static Texture2D Dove = ContentFinder<Texture2D>.Get("UI/Icons/dove");

	public static Texture2D Day = ContentFinder<Texture2D>.Get("UI/Icons/day");

	public static Texture2D Night = ContentFinder<Texture2D>.Get("UI/Icons/night");

	public static Texture2D ArrowDown = ContentFinder<Texture2D>.Get("UI/Icons/arrow-down");

	public static Texture2D ArrowUp = ContentFinder<Texture2D>.Get("UI/Icons/arrow-up");

	public static Texture2D Funnel = ContentFinder<Texture2D>.Get("UI/Icons/funnel");

	public static Texture2D PlayXL = ContentFinder<Texture2D>.Get("UI/Icons/play-xl");

	public static Texture2D PauseXL = ContentFinder<Texture2D>.Get("UI/Icons/pause-xl");
}
