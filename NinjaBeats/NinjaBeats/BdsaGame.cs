namespace NinjaBeats {
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Media;

	/// <summary>
	/// This is the main type for our game
	/// </summary>
	/// <author>Jacob Rasmussen, Jesper Bøg and Per Mortensen</author>
	public class BdsaGame : Game {
		private readonly GraphicsDeviceManager graphics;
		private LevelSystem levelSystem;
		private MenuSystem menuSystem;
		private SpriteBatch spriteBatch;
		private GameState state;

		public BdsaGame() {
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.HotPink);
			spriteBatch.Begin();

			switch (state) {
				case GameState.Menu:
					IsMouseVisible = true;
					menuSystem.Draw(spriteBatch);
					break;
				case GameState.Level:
					IsMouseVisible = false;
					levelSystem.Draw(spriteBatch);
					break;
			}

			spriteBatch.End();
			base.Draw(gameTime);
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize() {
			state = GameState.Menu;
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of our content.
		/// </summary>
		protected override void LoadContent() {
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			ContentBank.LoadContent(Content);
			menuSystem = new MenuSystem();
			menuSystem.Request += HandleSystemRequest;

			levelSystem = new LevelSystem(graphics);
			levelSystem.Request += HandleSystemRequest;
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent() {
			ContentBank.UnloadContent(Content);
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime) {
			// enter pause menu if in a level and the user unfocuses the window
			if (!IsActive && state == GameState.Level) HandleSystemRequest(this, new SystemRequestEventArgs(SystemRequest.PauseLevel));

			// the game still receives click events when not focused! This check halts execution until focused
			if (!IsActive) return;

			switch (state) {
				case GameState.Menu:
					menuSystem.Update(gameTime);
					break;
				case GameState.Level:
					levelSystem.Update(gameTime);
					break;
			}

			base.Update(gameTime);
		}

		/// <summary>
		/// Handles requests from the menu and level subsystems
		/// </summary>
		/// <param name="sender">The sender</param>
		/// <param name="e">The system request event arguments</param>
		/// <author>Per Mortensen</author>
		private void HandleSystemRequest(object sender, SystemRequestEventArgs e) {
			switch (e.Request) {
				case SystemRequest.ResumeLevel:
					MediaPlayer.Resume();
					state = GameState.Level;
					break;
				case SystemRequest.StartRandomLevel:
					state = GameState.Level;
					levelSystem.InitializeRandomLevel();
					break;
				case SystemRequest.StartMusicLevel:
					state = GameState.Level;
					levelSystem.InitializeMusicLevel(menuSystem.SelectedSongPath);
					break;
				case SystemRequest.ExitGame:
					Exit();
					break;
				case SystemRequest.EndLevel:
					state = GameState.Menu;
					MediaPlayer.IsRepeating = true;
					menuSystem.ScoreEntry(levelSystem.ScoreCounter);
					break;
				case SystemRequest.PauseLevel:
					state = GameState.Menu;
					menuSystem.PauseMenu();
					break;
			}
		}
	}
}