namespace NinjaBeats {
	using System;

	using Microsoft.Xna.Framework;

	/// <summary>
	/// An enemy with a random speed and left-ish direction.
	/// </summary>
	/// <author>Jacob Rasmussen</author>
	public class Enemy : Sprite {
		// Bounce between the bounds.
		private const int MIN_Y_BOUNDS = 80; 
		private const int MAX_Y_BOUNDS = 400;
		private static readonly AnimatedTexture EnemyTexture = ContentBank.SpriteEnemy;

		// Starts outside the screen to vary the Y-coordinates at which they touch the viewport's right side.
		private static readonly Vector2 StartingPosition = new Vector2(900, 300);
		private readonly Random random;
		private int health;
		public bool IsKilled { get; private set; }

		/// <summary>
		/// Creates an invisible enemy at the starting position.
		/// </summary>
		public Enemy() : base(EnemyTexture, StartingPosition) {
			health = 2;
			random = new Random();
			Visible = false;
		}

		/// <summary>
		/// Bounces the enemy whenever it touches the minimum or maximum bounds. Sets the enemy to inactive if it moves outside the left side of the viewport.
		/// </summary>
		public void CheckInsideBounds() {
			if (BoundingBox.Top < MIN_Y_BOUNDS || BoundingBox.Bottom > MAX_Y_BOUNDS) Speed = new Vector2(Speed.X, -Speed.Y);

			if (BoundingBox.Right < 0) {
				Visible = false;
				health = 2;
				Position = StartingPosition;
			}
		}

		/// <summary>
		/// The enemy gets hit by a projectile and loses one health point. Goes invisible when it reaches 0.
		/// </summary>
		public void GetHit() {
			if (Visible) {
				health--;
				ContentBank.SoundEnemyHit.Play();
			}

			Visible = health > 0;

			// Used for coin-spawning.
			IsKilled = !Visible;
		}

		/// <summary>
		/// Spawns an enemy at the starting position with a random speed and direction.
		/// </summary>
		public void Spawn() {
			Position = StartingPosition;
			Visible = true;
			health = 2;

			// Creates a new vector pointing in a left-ish direction.
			var direction = new Vector2(-1, random.Next(0, 6) - 3);
			direction.Normalize();
			Direction = direction;
			Speed = new Vector2(random.Next(200, 400));
		}

		public override void Update(GameTime gameTime) {
			if (!Visible) return;
			CheckInsideBounds();
			base.Update(gameTime);
		}
	}
}