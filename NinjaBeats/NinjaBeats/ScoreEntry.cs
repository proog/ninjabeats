namespace NinjaBeats {
	using System.Collections.Generic;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	/// <summary>
	/// A menu that allows the user to see their post-game score and submit it, as well as changing their recorded name.
	/// </summary>
	/// <author>Per Mortensen</author>
	public class ScoreEntry : Menu {
		private const string DEFAULT_NAME = "Average Joe";
		private readonly int newScore;
		private ScoreEntryForm entryForm;
		public HighScorer HighScorer { get; private set; }

		/// <summary>
		/// Constructs this menu with a background, a list of buttons and the total score that the player obtained.
		/// </summary>
		/// <param name="background">The background of the menu.</param>
		/// <param name="buttons">The clickable buttons in the menu.</param>
		/// <param name="newScore">The score which can be submitted to the scoreboard along with the player's name.</param>
		public ScoreEntry(Background background, List<Button> buttons, int newScore) : base(background, buttons) {
			this.newScore = newScore;
			HighScorer = new HighScorer(DEFAULT_NAME, newScore);
		}

		/// <summary>
		/// Brings up a Windows Forms dialog allowing the user to change their recorded name.
		/// </summary>
		public void ShowForm() {
			entryForm = new ScoreEntryForm(HighScorer.Name);
			entryForm.ShowDialog();
			HighScorer = new HighScorer(entryForm.HighScorerName, newScore);
		}

		/// <summary>
		/// Updates the menu if the name entry form is not showing.
		/// </summary>
		/// <param name="gameTime">A snapshot of timing values.</param>
		public override void Update(GameTime gameTime) {
			// Only update the underlying menu if the entry form is not showing. Otherwise, clicks are captured!
			if (entryForm == null || !entryForm.Visible) base.Update(gameTime);
		}

		/// <summary>
		/// Draws the menu and its components to the screen.
		/// </summary>
		/// <param name="spriteBatch">The sprite batch with which the menu will be drawn.</param>
		public override void Draw(SpriteBatch spriteBatch) {
			base.Draw(spriteBatch);

			var nameFont = ContentBank.FontStandardLarge;
			var scoreFont = ContentBank.FontDigitalLarge;
			var namePosition = nameFont.MeasureString(HighScorer.Name);
			var scorePosition = scoreFont.MeasureString(HighScorer.Score.ToString());
			spriteBatch.DrawString(nameFont, HighScorer.Name, new Vector2(800 / 2 - namePosition.X / 2, 200), Color.White);
			spriteBatch.DrawString(scoreFont, HighScorer.Score.ToString(), new Vector2(800 / 2 - scorePosition.X / 2, 250), Color.White);
		}

		protected override void FireEvent(object sender, MenuEventArgs e) {
			if(e.Action == MenuAction.EnterName)
				ShowForm();
			else
				base.FireEvent(sender, e);
		}
	}
}