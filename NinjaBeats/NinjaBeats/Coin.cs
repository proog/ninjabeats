namespace NinjaBeats {
	using Microsoft.Xna.Framework;

	/// <summary>
	/// A coin that spawns when an enemy dies. Can be picked up by Player for 150 points.
	/// </summary>
	/// <author>Jacob Rasmussen</author>
	public class Coin : Sprite {
		public Coin(AnimatedTexture texture, Vector2 position) : base(texture, position) {
			Speed = new Vector2(250);
			Direction = new Vector2(-1,0);
			Visible = false;
		}

		/// <summary>
		/// Spawns the coin.
		/// </summary>
		/// <param name="position">The spawning position.</param>
		public void Spawn(Vector2 position) {
			Position = position;
			Visible = true;
		}

		public override void Update(GameTime gameTime) {
			CheckInsideBounds();
			if (!Visible) return;
			base.Update(gameTime);
		}

		/// <summary>
		/// Coin goes invisible when moving outside screen, making it able to be spawned again by another enemy.
		/// </summary>
		private void CheckInsideBounds() {
			if (BoundingBox.Right < 0) 
				Visible = false;
		}
	}
}
