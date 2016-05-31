namespace NinjaBeats {
	/// <summary>
	/// An abstract class for level generators, e.g. randomly generated, music-based, hardcoded levels, etc.
	/// </summary>
	/// <author>Jesper Bøg and Per Mortensen</author>
	public abstract class PlatformGenerator {
		protected Platform LastPlatform;
		protected Level Level;
		protected PlatformType PlatformType;
		protected int ScreenWidth;
		protected int SpawnPoint;

		/// <summary>
		/// Specialized update method for new generators, this is where the generation logic is placed.
		/// </summary>
		public abstract void Update();

		/// <summary>
		/// Generates the starting platforms.
		/// </summary>
		/// <returns>A List of the generated platforms</returns>
		/// <author>Jesper Bøg</author>
		public virtual Level Generate() {
			LastPlatform = new Platform(0, PlatformType);
			Level.Platforms.Enqueue(LastPlatform);

			while (LastPlatform.Position.X < SpawnPoint) AddPlatform();

			return Level;
		}

		/// <summary>
		/// Adds a platform to the generated level
		/// </summary>
		/// <author>Jesper Bøg</author>
		protected void AddPlatform() {
			if (Level.Platforms.Count > ScreenWidth/Platform.Width + 3) Level.Platforms.Dequeue();
			LastPlatform = new Platform(LastPlatform.Position.X + Platform.Width, PlatformType);
			Level.Platforms.Enqueue(LastPlatform);
		}

		/// <summary>
		/// Signals the end of a level, places finish-platforms in both the top and the bottom of the level.
		/// </summary>
		/// <author>Jesper Bøg</author>
		protected void EndLevel() {
			Level.Platforms.Enqueue(new Platform(LastPlatform.Position.X + Platform.Width, PlatformType.FinishCeiling));
			LastPlatform = new Platform(LastPlatform.Position.X + Platform.Width, PlatformType.FinishFloor);
			Level.Platforms.Enqueue(LastPlatform);
		}

		/// <summary>
		/// Toggles the vertical position of the platforms.
		/// </summary>
		/// <author>Jesper Bøg</author>
		protected void TogglePlatformPosition() {
			PlatformType = PlatformType == PlatformType.Floor ? PlatformType.Ceiling : PlatformType.Floor;
		}
	}
}