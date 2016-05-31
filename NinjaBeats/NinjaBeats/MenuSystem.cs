namespace NinjaBeats {
	using System.Collections.Generic;
	using System.IO;
	using System.Windows.Forms;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input;
	using Microsoft.Xna.Framework.Media;

	using Keys = Microsoft.Xna.Framework.Input.Keys;

	/// <summary>
	/// The event handler for system requests that the menu system does not have the rights to execute itself.
	/// </summary>
	/// <param name="sender">The sender of the request.</param>
	/// <param name="e">The event arguments containing the request.</param>
	public delegate void RequestEventHandler(object sender, SystemRequestEventArgs e);

	/// <summary>
	/// The menu subsystem of the game. This class manages all menus and transitions between them.
	/// </summary>
	/// <author>Per Mortensen</author>
	public class MenuSystem {
		// A range of menus
		private Menu menu;

		public string SelectedSongPath { get { return ((SelectSongMenu)menu).SelectedSongPath; } }

		/// <summary>
		/// The event for sending requests to the main game class.
		/// BdsaGame must attach itself to this event for the requests to have any impact.
		/// </summary>
		public event RequestEventHandler Request;

		/// <summary>
		/// Constructs the menu subsystem and initializes the main menu.
		/// </summary>
		public MenuSystem() {
			MainMenu();
		}

		/// <summary>
		/// Initializes the main menu. This is the first screen seen when starting the game.
		/// </summary>
		private void MainMenu() {
			var buttons = new List<Button> {
			                               	new Button(ContentBank.SpriteButton, new Vector2(800/2 - ContentBank.SpriteButton.Width/2, 400), "Play", MenuAction.SelectGameType, ContentBank.FontStandardLarge),
			                               	new Button(ContentBank.SpriteButton, new Vector2(100, 400), "High Scores", MenuAction.HighScores),
			                               	new Button(ContentBank.SpriteButton, new Vector2(500, 400), "Exit", MenuAction.Exit)
			                               };

			var background = new Background(ContentBank.BackgroundMainMenu, Vector2.Zero, Vector2.Zero);

			menu = new Menu(background, buttons);
			menu.MenuEvent += HandleMenuEvent;

			// if music is not playing, start it
			if(MediaPlayer.State != MediaState.Playing) {
				MediaPlayer.Stop();
				MediaPlayer.IsRepeating = true;
				MediaPlayer.Play(ContentBank.MusicMenu);
			}
		}

		/// <summary>
		/// Initializes the list of high scorers.
		/// </summary>
		/// <param name="newHighScorer">The most recent high scorer, if applicable, which will be highlighted in the list.
		/// If not applicable, a value of null means no highlighting.</param>
		private void Scoreboard(HighScorer newHighScorer) {
			var buttons = new List<Button> {
			                               	new Button(ContentBank.SpriteButton, new Vector2(800/2 - ContentBank.SpriteButton.Width/2, 400), "Main Menu", MenuAction.MainMenu)
			                               };

			var background = new Background(ContentBank.BackgroundHighScoresMenu, Vector2.Zero, Vector2.Zero);

			menu = newHighScorer != null ? new Scoreboard(background, buttons, newHighScorer) : new Scoreboard(background, buttons);
			menu.MenuEvent += HandleMenuEvent;
		}

		/// <summary>
		/// Initializes the screen where a user selects whether to play a random or a music generated level.
		/// </summary>
		private void SelectGameType() {
			var buttons = new List<Button> {
			                               	new Button(ContentBank.SpriteButton, new Vector2(200, 300), "Random", MenuAction.StartRandomGame),
			                               	new Button(ContentBank.SpriteButton, new Vector2(400, 300), "Music", MenuAction.SelectSong),
			                               	new Button(ContentBank.SpriteButton, new Vector2(800/2 - ContentBank.SpriteButton.Width/2, 400), "Back", MenuAction.MainMenu)
			                               };

			var background = new Background(ContentBank.BackgroundGameTypeMenu, Vector2.Zero, Vector2.Zero);

			menu = new Menu(background, buttons);
			menu.MenuEvent += HandleMenuEvent;
		}

		/// <summary>
		/// Initializes the screen where a user selects the song to use in a music generated level.
		/// </summary>
		private void SelectSong() {
			var buttons = new List<Button> {
			                               	new Button(ContentBank.SpriteButton, new Vector2(200, 300), "Load", MenuAction.LoadSong),
			                               	new Button(ContentBank.SpriteButton, new Vector2(400, 300), "Start", MenuAction.StartMusicGame),
			                               	new Button(ContentBank.SpriteButton, new Vector2(800/2 - ContentBank.SpriteButton.Width/2, 400), "Back", MenuAction.SelectGameType)
			                               };

			var background = new Background(ContentBank.BackgroundSongMenu, Vector2.Zero, Vector2.Zero);

			menu = new SelectSongMenu(background, buttons);
			menu.MenuEvent += HandleMenuEvent;
		}

		/// <summary>
		/// Initializes the credits screen, seen by pressing C from the main menu.
		/// </summary>
		private void CreditsMenu() {
			var buttons = new List<Button> { new Button(ContentBank.SpriteButton, new Vector2(10, 400), "Main Menu", MenuAction.MainMenu) };

			var background = new Background(ContentBank.BackgroundCreditsMenu, Vector2.Zero, Vector2.Zero);

			menu = new Credits(background, buttons);
			menu.MenuEvent += HandleMenuEvent;
		}

		/// <summary>
		/// Initializes the pause menu, seen by pressing the Escape key or unfocusing the window while in a level.
		/// </summary>
		public void PauseMenu() {
			MediaPlayer.Pause();

			var buttons = new List<Button> {
			                               	new Button(ContentBank.SpriteButton, new Vector2(200, 300), "Resume", MenuAction.Resume),
			                               	new Button(ContentBank.SpriteButton, new Vector2(400, 300), "Main Menu", MenuAction.MainMenu),
			                               	new Button(ContentBank.SpriteButton, new Vector2(800/2 - ContentBank.SpriteButton.Width/2, 400), "Exit", MenuAction.Exit)
			                               };

			var background = new Background(ContentBank.BackgroundPauseMenu, Vector2.Zero, Vector2.Zero);

			menu = new Menu(background, buttons);
			menu.MenuEvent += HandleMenuEvent;
		}

		/// <summary>
		/// Initializes the level complete screen, seen by completing a level.
		/// Here, the user can type in their name, see their final score and submit it to the scoreboard.
		/// </summary>
		/// <param name="scoreCounter">The score counter from the completed level, used for reading the score.</param>
		public void ScoreEntry(ScoreCounter scoreCounter) {
			MediaPlayer.Stop();
			MediaPlayer.Play(ContentBank.MusicMenu);

			var buttons = new List<Button> {
			                               	new Button(ContentBank.SpriteButton, new Vector2(800/2 - ContentBank.SpriteButton.Width/2, 300), "Change Name", MenuAction.EnterName),
			                               	new Button(ContentBank.SpriteButton, new Vector2(200, 400), "Submit", MenuAction.SubmitHighScore, ContentBank.FontStandardLarge),
			                               	new Button(ContentBank.SpriteButton, new Vector2(400, 400), "Skip", MenuAction.HighScores)
			                               };

			var background = new Background(ContentBank.BackgroundEnterNameMenu, Vector2.Zero, Vector2.Zero);

			menu = new ScoreEntry(background, buttons, scoreCounter.Score);
			menu.MenuEvent += HandleMenuEvent;
		}

		/// <summary>
		/// Fires a request using the Request event.
		/// </summary>
		/// <param name="sender">The sender of the request.</param>
		/// <param name="e">The event arguments, containing the request.</param>
		private void FireRequest(object sender, SystemRequestEventArgs e) {
			if (Request != null) Request(sender, e);
		}

		/// <summary>
		/// Handles menu events when menu buttons are clicked.
		/// Decides what to do according to the action specified in the event arguments.
		/// </summary>
		/// <param name="sender">The sender of the action (usually a button).</param>
		/// <param name="e">The event argument, containing the action.</param>
		private void HandleMenuEvent(object sender, MenuEventArgs e) {
			switch(e.Action) {
				case MenuAction.MainMenu:
					MainMenu();
					break;
				case MenuAction.Exit:
					FireRequest(this, new SystemRequestEventArgs(SystemRequest.ExitGame));
					break;
				case MenuAction.Resume:
					FireRequest(this, new SystemRequestEventArgs(SystemRequest.ResumeLevel));
					break;
				case MenuAction.SelectGameType:
					SelectGameType();
					break;
				case MenuAction.SelectSong:
					SelectSong();
					break;
				case MenuAction.StartMusicGame:
					if(!File.Exists(SelectedSongPath)) MessageBox.Show("You must select a song before starting the level.");
					else FireRequest(this, new SystemRequestEventArgs(SystemRequest.StartMusicLevel));
					break;
				case MenuAction.StartRandomGame:
					FireRequest(this, new SystemRequestEventArgs(SystemRequest.StartRandomLevel));
					break;
				case MenuAction.HighScores:
					Scoreboard(null);
					break;
				case MenuAction.SubmitHighScore:
					Scoreboard(((ScoreEntry)menu).HighScorer);
					break;
				case MenuAction.Credits:
					CreditsMenu();
					break;
			}
		}

		/// <summary>
		/// Updates the appropriate menu depending on the current state of the menu system.
		/// </summary>
		/// <param name="gameTime">A snapshot of timing values.</param>
		public void Update(GameTime gameTime) {
			menu.Update(gameTime);
			var keyboardState = Keyboard.GetState();
			if(keyboardState.IsKeyDown(Keys.C)) CreditsMenu();
		}

		/// <summary>
		/// Draws the appropriate menu and its components to the screen.
		/// </summary>
		/// <param name="spriteBatch">The sprite batch with which to draw the menu.</param>
		public void Draw(SpriteBatch spriteBatch) {
			menu.Draw(spriteBatch);
		}
	}
}