namespace NinjaBeats {
	using System.Collections.Generic;
	using System.IO;
	using System.Windows.Forms;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	/// <summary>
	/// A menu allowing the user to select a song for use in a music generated level.
	/// </summary>
	/// <author>Per Mortensen</author>
	public class SelectSongMenu : Menu {
		private OpenFileDialog dialog; // The file browser dialog

		/// <summary>
		/// The full path to the selected file.
		/// </summary>
		public string SelectedSongPath { get; private set; }

		/// <summary>
		/// Constructs this menu with a background and a list of buttons.
		/// </summary>
		/// <param name="background">The background of the menu.</param>
		/// <param name="buttons">The clickable buttons in the menu.</param>
		public SelectSongMenu(Background background, List<Button> buttons) : base(background, buttons) {
			SelectedSongPath = "No song selected";
		}

		/// <summary>
		/// Brings up the file browser dialog for selecting a song and saves the choice.
		/// </summary>
		public void LoadSong() {
			dialog = new OpenFileDialog { Filter = "Supported Files|*.mp3;*.wma" };

			if(dialog.ShowDialog() == DialogResult.OK) SelectedSongPath = dialog.FileName;
		}

		/// <summary>
		/// Draws the menu and the file name of the selected song.
		/// </summary>
		/// <param name="spriteBatch">The sprite batch with which the menu will be drawn.</param>
		public override void Draw(SpriteBatch spriteBatch) {
			base.Draw(spriteBatch);

			var str = new FileInfo(SelectedSongPath).Name;
			var font = ContentBank.FontStandardLarge;
			var position = font.MeasureString(str);
			spriteBatch.DrawString(font, str, new Vector2(800 / 2 - position.X / 2, 200), Color.White);
		}

		protected override void FireEvent(object sender, MenuEventArgs e) {
			if(e.Action == MenuAction.LoadSong)
				LoadSong();
			else
				base.FireEvent(sender, e);
		}
	}
}