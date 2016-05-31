namespace NinjaBeats {
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	/// <summary>
	/// A button primarily for use in menus.
	/// </summary>
	/// <author>Per Mortensen</author>
	public class Button : Sprite {
		public readonly MenuAction Action;
		private readonly SpriteFont font;
		private readonly string text;
		private readonly Vector2 textPosition;

		/// <summary>
		/// Constructs a graphical button with the specified values, using a default font.
		/// </summary>
		/// <param name="texture">The texture of the button.</param>
		/// <param name="position">The position of the button.</param>
		/// <param name="text">The text of the button. This will be written centered on top of the texture.</param>
		/// <param name="action">The menu action used in the event handling system when the button is clicked.</param>
		public Button(AnimatedTexture texture, Vector2 position, string text, MenuAction action) : base(texture, position) {
			this.text = text;
			Action = action;
			font = ContentBank.FontStandard;

			textPosition = CalculateTextPosition(text);
		}

		/// <summary>
		/// Constructs a graphical button with the specified values, using a specified font.
		/// </summary>
		/// <param name="texture">The texture of the button.</param>
		/// <param name="position">The position of the button.</param>
		/// <param name="text">The text of the button. This will be written centered on top of the texture.</param>
		/// <param name="action">The menu action used in the event handling system when the button is clicked.</param>
		/// <param name="font">The font with which to render the text.</param>
		public Button(AnimatedTexture texture, Vector2 position, string text, MenuAction action, SpriteFont font) : base(texture, position) {
			this.text = text;
			Action = action;
			this.font = font;

			textPosition = CalculateTextPosition(text);
		}

		/// <summary>
		/// Draws the button by first drawing the texture, then rendering the text on top.
		/// </summary>
		/// <param name="spriteBatch">The sprite batch with which to draw the button.</param>
		public override void Draw(SpriteBatch spriteBatch) {
			base.Draw(spriteBatch);
			spriteBatch.DrawString(font, text, textPosition, Color.White);
		}

		/// <summary>
		/// Calculates the correct position for centering the text on the button texture.
		/// </summary>
		/// <param name="str">The string to center.</param>
		/// <returns>A vector specifying the absolute position of the text.</returns>
		private Vector2 CalculateTextPosition(string str) {
			var textSize = font.MeasureString(str);
			var textPos = new Vector2 {
			                          	X = Position.X + Texture.Width/2f - textSize.X/2f, Y = Position.Y + Texture.Height/2f - textSize.Y/2f
			                          };

			return textPos;
		}
	}
}