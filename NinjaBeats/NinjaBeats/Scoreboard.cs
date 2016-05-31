namespace NinjaBeats {
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	/// <summary>
	/// A list of the 10 highest scoring high scorers.
	/// </summary>
	/// <author>Per Mortensen</author>
	public class Scoreboard : Menu {
		/// <summary>
		/// The file to read and write high scorers to.
		/// </summary>
		private const string HIGH_SCORE_FILE = "highscores.txt";

		private const int INITIAL_POSITION_Y = 100;
		private readonly HighScorer newestHighScorer;
		private List<HighScorer> highScorers;

		/// <summary>
		/// Constructs a scoreboard with the specified background and buttons.
		/// </summary>
		/// <param name="background">The background of the scoreboard.</param>
		/// <param name="buttons">The buttons in the scoreboard.</param>
		public Scoreboard(Background background, List<Button> buttons) : base(background, buttons) {
			ReadScores();
		}

		/// <summary>
		/// Constructs a scoreboard with the specified background, buttons and high scorer to highlight.
		/// </summary>
		/// <param name="background">The background of the scoreboard.</param>
		/// <param name="buttons">The buttons in the scoreboard.</param>
		/// <param name="newHighScorer">The high scorer to highlight in the list. This could be the most recent high scorer.</param>
		public Scoreboard(Background background, List<Button> buttons, HighScorer newHighScorer) : base(background, buttons) {
			ReadScores();

			newestHighScorer = newHighScorer;
			highScorers.Add(newHighScorer);

			WriteScores();
			ReadScores();
		}

		/// <summary>
		/// Display the top ten high scorers.
		/// </summary>
		/// <param name="spriteBatch">The sprite batch with which to render the list of high scorers.</param>
		private void DisplayTopTen(SpriteBatch spriteBatch) {
			var position = new Vector2(0, INITIAL_POSITION_Y);

			if(highScorers.Count == 0) {
				const string noHighScorers = "There are no high scorers yet!";
				position.Y += 20;
				position.X = 800 / 2 - ContentBank.FontStandard.MeasureString(noHighScorers).X / 2;
				spriteBatch.DrawString(ContentBank.FontStandard, noHighScorers, position, Color.White);
				return;
			}

			foreach (var highScorer in highScorers.OrderByDescending(hs => hs.Score).Take(10)) {
				position.Y += 20;
				position.X = 800/2 - ContentBank.FontStandard.MeasureString(highScorer.Name).X;

				var color = Color.White;
				if (newestHighScorer != null && newestHighScorer.Name.Equals(highScorer.Name) && newestHighScorer.Score == highScorer.Score) color = Color.Red;

				spriteBatch.DrawString(ContentBank.FontStandard, highScorer.Name + " : " + highScorer.Score, position, color);
			}
		}

		/// <summary>
		/// Read the high scores file to the highScorers list.
		/// </summary>
		private void ReadScores() {
			// if the highscores file doesn't exist, create it
			if (!File.Exists(HIGH_SCORE_FILE)) File.CreateText(HIGH_SCORE_FILE).Close();

			var reader = new StreamReader(HIGH_SCORE_FILE);
			highScorers = new List<HighScorer>();

			string line;
			while ((line = reader.ReadLine()) != null) {
				int delimiter = line.LastIndexOf(";");
				string name = line.Substring(0, delimiter);
				int score;
				try {
					score = int.Parse(line.Substring(delimiter + 1));
				} catch(FormatException) {
					continue;
				}

				highScorers.Add(new HighScorer(name, score));
			}
			reader.Close();
		}

		/// <summary>
		/// Write the 10 highest scoring entries in the highScorers list to the high scores file.
		/// </summary>
		private void WriteScores() {
			var writer = new StreamWriter(HIGH_SCORE_FILE);
			foreach (var highScorer in highScorers.OrderByDescending(hs => hs.Score).Take(10)) writer.WriteLine(highScorer.Name + ";" + highScorer.Score);
			writer.Close();
		}

		/// <summary>
		/// Draw the menu and the list of top 10 high scorers to the screen.
		/// </summary>
		/// <param name="spriteBatch">The sprite batch with which to draw the menu and text.</param>
		public override void Draw(SpriteBatch spriteBatch) {
			base.Draw(spriteBatch);
			DisplayTopTen(spriteBatch);
		}
	}
}