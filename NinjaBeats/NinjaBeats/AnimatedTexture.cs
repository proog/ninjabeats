namespace NinjaBeats {
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	/// <summary>
	/// A wrapper for a Texture2D that enables animation support using sprite sheets.
	/// </summary>
	/// <author>Per Mortensen</author>
	public class AnimatedTexture {
		private readonly Texture2D texture;
		private readonly float interval;
		private float elapsedTime;
		private int currentFrame;
		public readonly int Frames;
		public Color Color;
		public bool Flipped;
		public bool Paused;
		public int Width { get { return texture.Width; } }
		public int Height { get { return texture.Height; } }

		/// <summary>
		/// Constructs a non-animated AnimatedTexture that works much like a normal Texture2D object.
		/// </summary>
		/// <param name="texture"></param>
		public AnimatedTexture(Texture2D texture) {
			this.texture = texture;
			interval = 0;
			Frames = 1;
			Color = Color.White;
		}

		/// <summary>
		/// Constructs an animated AnimatedTexture.
		/// </summary>
		/// <param name="texture">The texture to draw. To animate correctly, it must contain all the animation frames horizontally and evenly distributed.</param>
		/// <param name="interval">The display time of each frame, in seconds.</param>
		/// <param name="frames">The number of frames in the texture.</param>
		public AnimatedTexture(Texture2D texture, float interval, int frames) : this(texture) {
			this.interval = interval;
			Frames = frames;
		}

		/// <summary>
		/// Updates the current frame according to the animation interval and the timing values provided.
		/// </summary>
		/// <param name="gameTime">A snapshot of timing values.</param>
		public void Update(GameTime gameTime) {
			if(Paused || Frames == 1)
				return;

			elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

			if(elapsedTime < interval)
				return;

			currentFrame++;
			currentFrame = currentFrame % Frames;
			elapsedTime -= interval;
		}

		/// <summary>
		/// Draws the current frame to screen at the specified position.
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch with which to draw the texture.</param>
		/// <param name="position">The position of the texture.</param>
		public void Draw(SpriteBatch spriteBatch, Vector2 position) {
			int frameWidth = texture.Width / Frames;
			int frameHeight = texture.Height;
			var crop = new Rectangle(frameWidth * currentFrame, 0, frameWidth, frameHeight);
			var effect = Flipped ? SpriteEffects.FlipVertically : SpriteEffects.None;

			spriteBatch.Draw(texture, position, crop, Color, 0, Vector2.Zero, 1, effect, 0);
		}
	}
}
