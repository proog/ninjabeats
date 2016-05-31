namespace NinjaBeats {
	using System.Collections.Generic;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input;

	/// <summary>
	/// The event handler for menu actions, mostly used when buttons are clicked.
	/// </summary>
	/// <param name="sender">The sender of the action.</param>
	/// <param name="e">The event arguments containing the action.</param>
	public delegate void MenuEventHandler(object sender, MenuEventArgs e);

	/// <summary>
	/// A single "menu screen" containing buttons and a background.
	/// </summary>
	/// <author>Per Mortensen</author>
	public class Menu {
		private readonly Background background;
		private readonly List<Button> buttons;
		private MouseState previousMouseState;

		public event MenuEventHandler MenuEvent;

		/// <summary>
		/// Constructs a menu using the specified background and buttons.
		/// </summary>
		/// <param name="background">The background of the menu.</param>
		/// <param name="buttons">The clickable buttons to draw in the menu.</param>
		public Menu(Background background, List<Button> buttons) {
			this.background = background;
			this.buttons = buttons;
		}

		/// <summary>
		/// Updates the background, buttons and checks for mouse clicks on buttons.
		/// </summary>
		/// <param name="gameTime">A snapshot of timing values.</param>
		public virtual void Update(GameTime gameTime) {
			var mouseState = Mouse.GetState();
			if (mouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed) {
				int mouseX = mouseState.X;
				int mouseY = mouseState.Y;

				foreach (var button in buttons) {
					if (button.BoundingBox.Contains(mouseX, mouseY)) {
						ContentBank.SoundButton.Play();
						FireEvent(button, new MenuEventArgs(button.Action));
						break;
					}
				}
			}

			previousMouseState = mouseState;
			background.Update(gameTime);

			foreach (var button in buttons) button.Update(gameTime);
		}

		/// <summary>
		/// Fires the menu event, usually when a button is clicked.
		/// </summary>
		/// <param name="sender">The sender of the action.</param>
		/// <param name="e">The event arguments containing the action.</param>
		protected virtual void FireEvent(object sender, MenuEventArgs e) {
			if (MenuEvent != null) MenuEvent(sender, e);
		}

		/// <summary>
		/// Draws the menu to the screen.
		/// </summary>
		/// <param name="spriteBatch">The sprite batch with which to draw the menu and its components.</param>
		public virtual void Draw(SpriteBatch spriteBatch) {
			background.Draw(spriteBatch);

			foreach(var button in buttons) button.Draw(spriteBatch);
		}
	}
}