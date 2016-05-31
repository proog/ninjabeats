namespace NinjaBeats {
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Input;

	/// <summary>
	/// A sprite that follows the position of the mouse, thus acting as a mouse cursor.
	/// Used as a set of crosshairs in levels.
	/// </summary>
	/// <author>Per Mortensen</author>
	public class MouseCursor : Sprite {
		public MouseCursor(AnimatedTexture texture, Vector2 position) : base(texture, position) {}

		/// <summary>
		/// Updates the current position of the sprite to match the mouse cursor position.
		/// </summary>
		/// <param name="gameTime">A snapshot of timing values.</param>
		public override void Update(GameTime gameTime) {
			var mouseState = Mouse.GetState();
			Position = new Vector2(mouseState.X - Texture.Width/2, mouseState.Y - Texture.Height/2);
		}
	}
}