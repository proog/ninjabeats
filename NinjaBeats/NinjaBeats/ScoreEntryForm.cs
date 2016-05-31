namespace NinjaBeats {
	using System;
	using System.Windows.Forms;

	/// <summary>
	/// A Windows Forms dialog allowing the user to change their high score name.
	/// </summary>
	/// <author>Per Mortensen</author>
	public partial class ScoreEntryForm : Form {
		public string HighScorerName { get; private set; }

		/// <summary>
		/// Constructs a dialog with a specified default name.
		/// </summary>
		/// <param name="defaultName">The default name, which will initially be displayed in the text box.</param>
		public ScoreEntryForm(string defaultName) {
			InitializeComponent();
			textBox1.Text = defaultName;
			HighScorerName = defaultName;
		}

		private void OkButtonClick(object sender, EventArgs e) {
			HighScorerName = textBox1.Text;
			Close();
			Dispose();
		}
	}
}