namespace NinjaBeats {
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	/// <summary>
	/// A sprite to be drawn on the screen with a direction, speed and position.
	/// </summary>
	/// <author>Jacob Rasmussen, Per Mortensen</author>
	public class Sprite {
		protected readonly AnimatedTexture Texture;
		private Vector2 direction;
		private Vector2 position;
		private Vector2 speed;
		public bool Visible { get; set; }

		/// <summary>
		/// A box surrounding the sprite. Mostly used for collision detection.
		/// </summary>
		public Rectangle BoundingBox {
			get { return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width/Texture.Frames, Texture.Height); }
		}

		/// <summary>
		/// Optional tinting of the sprite.
		/// </summary>
		public Color Color {
			get { return Texture.Color; }
			set { Texture.Color = value; }
		}

		public Vector2 Direction {
			get { return direction; }
			set { direction = new Vector2(value.X, value.Y); }
		}

		public Vector2 Position {
			get { return position; }
			set { position = new Vector2(value.X, value.Y); }
		}

		public Vector2 Speed {
			get { return speed; }
			set { speed = new Vector2(value.X, value.Y); }
		}

		/// <summary>
		/// Create a sprite object.
		/// </summary>
		/// <param name="texture">Texture of the sprite</param>
		/// <param name="position">The initial position of the sprite</param>
		public Sprite(AnimatedTexture texture, Vector2 position) {
			Texture = texture;
			Position = position;
			Visible = true;
		}

		public virtual void Update(GameTime gameTime) {
			Position += Speed*Direction*(float)gameTime.ElapsedGameTime.TotalSeconds;
			Texture.Update(gameTime);
		}

		/// <summary>
		/// Draw the sprite on the screen.
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch used to draw the sprite</param>
		public virtual void Draw(SpriteBatch spriteBatch) {
			if(Visible) Texture.Draw(spriteBatch, Position);
		}
	}
}