namespace NinjaBeats {
	using Microsoft.Xna.Framework.Audio;
	using Microsoft.Xna.Framework.Content;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Media;

	/// <summary>
	/// A class containing references to common game content.
	/// </summary>
	/// <author>Per Mortensen</author>
	public class ContentBank {
		// Background sprites
		private static Texture2D backgroundEnterNameMenu;
		private static Texture2D backgroundGameTypeMenu;
		private static Texture2D backgroundHighScoresMenu;
		private static Texture2D backgroundLevel;
		private static Texture2D backgroundMainMenu;
		private static Texture2D backgroundPauseMenu;
		private static Texture2D backgroundSongMenu;
		private static Texture2D backgroundCreditsMenu;

		// Standard sprites
		private static Texture2D spriteButton;
		private static Texture2D spriteCoin;
		private static Texture2D spriteCursor;
		private static Texture2D spriteEnemy;
		private static Texture2D spritePlatform;
		private static Texture2D spritePlatformFinish;
		private static Texture2D spritePlayer;
		private static Texture2D spriteProjectile;

		// Fonts
		public static SpriteFont FontDigital { get; private set; }
		public static SpriteFont FontDigitalLarge { get; private set; }
		public static SpriteFont FontStandard { get; private set; }
		public static SpriteFont FontStandardLarge { get; private set; }

		// Music and sound effects
		public static Song MusicLevel { get; private set; }
		public static Song MusicMenu { get; private set; }
		public static SoundEffect SoundButton { get; private set; }
		public static SoundEffect SoundEnemyHit { get; private set; }
		public static SoundEffect SoundShoot { get; private set; }
		public static SoundEffect SoundCoin { get; private set; }

		public static AnimatedTexture BackgroundEnterNameMenu {
			get { return new AnimatedTexture(backgroundEnterNameMenu); }
		}

		public static AnimatedTexture BackgroundGameTypeMenu {
			get { return new AnimatedTexture(backgroundGameTypeMenu); }
		}

		public static AnimatedTexture BackgroundHighScoresMenu {
			get { return new AnimatedTexture(backgroundHighScoresMenu); }
		}

		public static AnimatedTexture BackgroundLevel {
			get { return new AnimatedTexture(backgroundLevel); }
		}

		public static AnimatedTexture BackgroundMainMenu {
			get { return new AnimatedTexture(backgroundMainMenu); }
		}

		public static AnimatedTexture BackgroundPauseMenu {
			get { return new AnimatedTexture(backgroundPauseMenu); }
		}

		public static AnimatedTexture BackgroundSongMenu {
			get { return new AnimatedTexture(backgroundSongMenu); }
		}

		public static AnimatedTexture BackgroundCreditsMenu {
			get { return new AnimatedTexture(backgroundCreditsMenu); }
		}

		public static AnimatedTexture SpriteButton {
			get { return new AnimatedTexture(spriteButton); }
		}

		public static AnimatedTexture SpriteCoin {
			get { return new AnimatedTexture(spriteCoin, 0.15f, 2); }
		}

		public static AnimatedTexture SpriteCursor {
			get { return new AnimatedTexture(spriteCursor); }
		}

		public static AnimatedTexture SpriteEnemy {
			get { return new AnimatedTexture(spriteEnemy, 0.5f, 2); }
		}

		public static AnimatedTexture SpritePlatform {
			get { return new AnimatedTexture(spritePlatform); }
		}

		public static AnimatedTexture SpritePlatformFinish {
			get { return new AnimatedTexture(spritePlatformFinish); }
		}

		public static AnimatedTexture SpritePlayer {
			get { return new AnimatedTexture(spritePlayer, 0.1f, 2); }
		}

		public static AnimatedTexture SpriteProjectile {
			get { return new AnimatedTexture(spriteProjectile, 0.1f, 2); }
		}

		/// <summary>
		/// Loads all the required game content. After this call,
		/// all assigned field and properties can be used for retrieving game assets.
		/// </summary>
		/// <param name="contentManager">The content manager with which to load the content.</param>
		public static void LoadContent(ContentManager contentManager) {
			spritePlayer = contentManager.Load<Texture2D>("sprites/ninja");
			spriteEnemy = contentManager.Load<Texture2D>("sprites/enemy");
			spritePlatform = contentManager.Load<Texture2D>("sprites/platform");
			spritePlatformFinish = contentManager.Load<Texture2D>("sprites/platformfinish");
			spriteCursor = contentManager.Load<Texture2D>("sprites/crosshair");
			spriteProjectile = contentManager.Load<Texture2D>("sprites/shuriken");
			spriteButton = contentManager.Load<Texture2D>("sprites/button");
			spriteCoin = contentManager.Load<Texture2D>("sprites/coin");

			backgroundLevel = contentManager.Load<Texture2D>("backgrounds/futurefactory");
			backgroundPauseMenu = contentManager.Load<Texture2D>("menubackgrounds/paused");
			backgroundMainMenu = contentManager.Load<Texture2D>("menubackgrounds/mainmenu");
			backgroundGameTypeMenu = contentManager.Load<Texture2D>("menubackgrounds/selectgametype");
			backgroundEnterNameMenu = contentManager.Load<Texture2D>("menubackgrounds/levelcomplete");
			backgroundHighScoresMenu = contentManager.Load<Texture2D>("menubackgrounds/highscores");
			backgroundSongMenu = contentManager.Load<Texture2D>("menubackgrounds/selectsong");
			backgroundCreditsMenu = contentManager.Load<Texture2D>("menubackgrounds/credits");

			FontStandard = contentManager.Load<SpriteFont>("fonts/couriernew");
			FontStandardLarge = contentManager.Load<SpriteFont>("fonts/couriernewlarge");
			FontDigital = contentManager.Load<SpriteFont>("fonts/quartz");
			FontDigitalLarge = contentManager.Load<SpriteFont>("fonts/quartzlarge");

			SoundShoot = contentManager.Load<SoundEffect>("sounds/throw");
			SoundEnemyHit = contentManager.Load<SoundEffect>("sounds/enemyhit");
			SoundButton = contentManager.Load<SoundEffect>("sounds/click");
			SoundCoin = contentManager.Load<SoundEffect>("sounds/coin");

			MusicMenu = contentManager.Load<Song>("music/menu");
			MusicLevel = contentManager.Load<Song>("music/level");
		}

		/// <summary>
		/// Unloads all game content.
		/// </summary>
		/// <param name="contentManager">The content manager with which to unload the content.</param>
		public static void UnloadContent(ContentManager contentManager) {
			contentManager.Unload();
		}
	}
}