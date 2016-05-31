namespace NinjaBeats {
	using System.Collections.Generic;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	/// <summary>
	/// A scrolling background graphic.
	/// </summary>
	/// <author>Per Mortensen</author>
	public class Background {
		private readonly Vector2 scrollingDirection;
		private readonly Vector2 scrollingSpeed;
		private readonly List<Sprite> sprites;

		/// <summary>
		/// Constructs a scrolling background with the specified looping texture, direction and speed.
		/// </summary>
		/// <param name="texture">The texture of the background, which will be looped indefinitely.</param>
		/// <param name="direction">The scrolling direction of the background.</param>
		/// <param name="speed">The scrolling speed of the background.</param>
		public Background(AnimatedTexture texture, Vector2 direction, Vector2 speed) {
			sprites = new List<Sprite> {
			                           	// place background1 at (0, 0)
										new Sprite(texture, Vector2.Zero),
										// place background 2 directly after background 1 on the X axis
			                           	new Sprite(texture, new Vector2(texture.Width, 0))
			                           };

			scrollingDirection = direction;
			scrollingSpeed = speed;
		}

		/// <summary>
		/// Updates the position of the background sprites.
		/// </summary>
		/// <param name="gameTime">A snapshot of timing values.</param>
		public void Update(GameTime gameTime) {
			foreach (var sprite in sprites) {
				if (sprite.Position.X < -sprite.BoundingBox.Width) sprite.Position = new Vector2(sprite.BoundingBox.Width, sprite.Position.Y);

				sprite.Position = new Vector2(
					sprite.Position.X + (scrollingDirection.X*scrollingSpeed.X*(float)gameTime.ElapsedGameTime.TotalSeconds),
					sprite.Position.Y + (scrollingDirection.Y*scrollingSpeed.Y*(float)gameTime.ElapsedGameTime.TotalSeconds));

				sprite.Update(gameTime);
			}
		}

		/// <summary>
		/// Draws the background sprites to the screen.
		/// </summary>
		/// <param name="spriteBatch">The sprite batch with which to draw.</param>
		public void Draw(SpriteBatch spriteBatch) {
			foreach(var sprite in sprites) sprite.Draw(spriteBatch);
		}
	}
}