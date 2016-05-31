namespace NinjaBeats {
	using System;

	/// <summary>
	/// A platform generator based on random toggling of platform position.
	/// </summary>
	/// <author>Per Mortensen, Jesper Bøg</author>
	public class RandomPlatformGenerator : PlatformGenerator {
		private const int NUMBER_OF_PLATFORMS = 100; // number of platforms to generate
		private readonly Random random; // the random number generator
		private int platformCount;

		/// <summary>
		/// Creates a RandomPlatformGenerator.
		/// </summary>
		public RandomPlatformGenerator(int screenWidth) {
			ScreenWidth = screenWidth;
			random = new Random();
			Level = new Level(this);
			SpawnPoint = screenWidth + Platform.Width;
		}

		/// <summary>
		/// Updates the current level, determines if it's time to end the level or switch the vertical position of the platforms.
		/// </summary>
		/// <author>Jesper Bøg</author>
		public override void Update() {
			if (platformCount >= NUMBER_OF_PLATFORMS) EndLevel();

			if (random.Next(0, 2) == 1) TogglePlatformPosition();

			if (LastPlatform.Position.X < SpawnPoint) {
				AddPlatform();
				platformCount++;
			}
		}
	}
}