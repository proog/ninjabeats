namespace NinjaBeats {
	/// <summary>
	/// A representation of a single high scorer with a name and a recorded score.
	/// </summary>
	/// <author>Per Mortensen</author>
	public class HighScorer {
		public readonly string Name;
		public readonly int Score;

		public HighScorer(string name, int score) {
			Name = name;
			Score = score;
		}
	}
}