namespace NinjaBeats {
	using Microsoft.Xna.Framework;

	/// <summary>
	/// A projectile that the player shoots using the mouse.
	/// </summary>
	/// <author>Jacob Rasmussen</author>
	public class Projectile : Sprite {
		public Projectile(AnimatedTexture texture, Vector2 position, Vector2 direction) : base(texture, position) {
			Speed = new Vector2(600);
			Fire(direction);
		}

		/// <summary>
		/// Fires the projectile.
		/// </summary>
		/// <param name="dir">The direction at which the projectile will be fired.</param>
		/// <author>Jacob Rasmussen</author>
		public void Fire(Vector2 dir) {
			dir.X -= BoundingBox.Width / 2f;
			dir.Y -= BoundingBox.Height / 2f;

			//Normalizes the vector to keep the speed of the projectile consistent.
			dir.Normalize();
			Direction = dir;
			Visible = true;
		}

		public override void Update(GameTime gameTime) {
			if (!Visible) return;
			base.Update(gameTime);
		}
	}
}