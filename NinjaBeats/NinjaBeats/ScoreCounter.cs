namespace NinjaBeats {
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	/// <summary>
	/// Continually counts the score of the player during a level.
	/// </summary>
	/// <author>Per Mortensen</author>
	public class ScoreCounter {
		// Bonuses and penalties
		public const int COIN_SCORE = 150;
		public const int DEATH_PENALTY = -400;
		public const int KILL_SCORE = 100;

		/// <summary>
		/// The current score contained in this score counter.
		/// </summary>
		public int Score { get; private set; }

		private const float INCREMENTAL_SCORE_DISPLAY_TIME = 3;
		private readonly Vector2 totalScorePosition;
		private bool displayIncrementalScore;
		private float elapsedTime;
		private Vector2 incrementalScorePosition;
		private int lastScore;

		/// <summary>
		/// Constructs a score counter with a starting score.
		/// </summary>
		/// <param name="initialScore">The score from which the score counter will count.</param>
		public ScoreCounter(int initialScore) {
			Score = initialScore;
			displayIncrementalScore = false;
			totalScorePosition = new Vector2(10, 10);
		}

		/// <summary>
		/// Updates the score.
		/// </summary>
		/// <param name="gameTime">A snapshot of timing values.</param>
		public void Update(GameTime gameTime) {
			elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
			Score++;

			if (elapsedTime < INCREMENTAL_SCORE_DISPLAY_TIME) return;

			displayIncrementalScore = false;
			elapsedTime -= INCREMENTAL_SCORE_DISPLAY_TIME;
		}

		/// <summary>
		/// Adds a specified bonus or penalty to the running score and displays it on the screen.
		/// </summary>
		/// <param name="addScore">The points to add.</param>
		/// <param name="scorePosition">The position of the bonus/penalty on-screen text.</param>
		public void UpdateScore(int addScore, Vector2 scorePosition) {
			Score += addScore;
			lastScore = addScore;
			incrementalScorePosition = scorePosition;
			displayIncrementalScore = true;
		}

		/// <summary>
		/// Draws the running score and any bonus/penalty to the screen.
		/// </summary>
		/// <param name="spriteBatch">The sprite batch with which the scores will be rendered.</param>
		public void Draw(SpriteBatch spriteBatch) {
			if(displayIncrementalScore) {
				string newScoreString;
				Color color;
				if(lastScore > 0) {
					newScoreString = "+" + lastScore + " points";
					color = Color.Green;
				}
				else {
					newScoreString = lastScore + " points";
					color = Color.Red;
				}

				spriteBatch.DrawString(ContentBank.FontDigital, newScoreString, incrementalScorePosition, color);
			}

			spriteBatch.DrawString(ContentBank.FontDigital, "Score: " + Score, totalScorePosition, Color.Black);
		}
	}
}