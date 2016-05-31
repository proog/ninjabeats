namespace NinjaBeats {
	using Microsoft.Xna.Framework.Media;

	/// <summary>
	/// A platform generator which generates platforms based on the currently playing music.
	/// </summary>
	/// <author>Jesper Bøg</author>
	public class MusicPlatformGenerator : PlatformGenerator {
		private readonly BeatDetector bd;

		/// <summary>
		/// Sets up the music platform generator.
		/// </summary>
		/// <param name="screenWidth">The width of the current game window.</param>
		/// <author>Jesper Bøg</author>
		public MusicPlatformGenerator(int screenWidth) {
			ScreenWidth = screenWidth;
			bd = new BeatDetector();
			PlatformType = PlatformType.Floor;
			Level = new Level(this);
			SpawnPoint = screenWidth + Platform.Width;
		}

		/// <summary>
		/// Updates the current level and beat detector and determines if platform position should be switched or if the level should end.
		/// </summary>
		/// <author>Jesper Bøg</author>
		public override void Update() {
			bd.Update();

			if (bd.IsBeat()) TogglePlatformPosition();

			if (LastPlatform.Position.X < SpawnPoint && MediaPlayer.State == MediaState.Playing) AddPlatform();

			if (MediaPlayer.State == MediaState.Stopped) EndLevel();
		}
	}
}