namespace NinjaBeats {
	using System.Collections.Generic;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	/// <summary>
	/// A level in the game world is a collection of platforms and a background
	/// </summary>
	/// <author>Jacob Rasmussen, Per Mortensen, Jesper Bøg</author>
	public class Level {
		public readonly Background Background;
		public readonly List<Coin> Coins;
		public readonly List<Enemy> Enemies;
		public readonly Queue<Platform> Platforms;
		private const int NUMBER_OF_ENEMIES = 10;
		private readonly PlatformGenerator generator;
		private double enemySpawningThreshold; // Used to spawn enemies at a set interval.

		public Level(PlatformGenerator generator) {
			this.generator = generator;
			Background = new Background(ContentBank.BackgroundLevel, new Vector2(-1, 0), new Vector2(160, 0));
			Platforms = new Queue<Platform>();
			Enemies = new List<Enemy>();
			Coins = new List<Coin>();

			for (int i = 0; i < NUMBER_OF_ENEMIES; i++) Enemies.Add(new Enemy());
		}

		public void Draw(SpriteBatch spriteBatch) {
			Background.Draw(spriteBatch);

			foreach (var enemy in Enemies) enemy.Draw(spriteBatch);
			foreach (var coin in Coins) coin.Draw(spriteBatch);
			foreach (var platform in Platforms) platform.Draw(spriteBatch);
		}

		///<author>Jacob Rasmussen</author>
		public virtual void Update(GameTime gameTime) {
			generator.Update();

			Background.Update(gameTime);

			// Spawns an enemy from the list roughly every 2 seconds.
			if (gameTime.TotalGameTime.TotalSeconds > enemySpawningThreshold) {
				Enemies.Find(x => !x.Visible).Spawn();
				enemySpawningThreshold = gameTime.TotalGameTime.TotalSeconds + 2d;
			}

			foreach (var enemy in Enemies) enemy.Update(gameTime);
			foreach (var coin in Coins) coin.Update(gameTime);
			foreach (var platform in Platforms) platform.Update(gameTime);
		}
	}
}