namespace NinjaBeats {
	using System.Reflection;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input;
	using Microsoft.Xna.Framework.Media;

	/// <summary>
	/// Assembles a level with a combination of a LevelGenerator, a background, and enemies.
	/// </summary>
	/// <author>Jacob Rasmussen, Per Mortensen, Jesper Bøg</author>
	public class LevelSystem {
		private readonly GraphicsDeviceManager graphics;
		private MouseCursor cursor;
		private Level level;
		private Player player;
		public ScoreCounter ScoreCounter { get; private set; }
		public event RequestEventHandler Request;

		/// <summary>
		/// Constructs a level system.
		/// </summary>
		/// <param name="graphics">The graphics device manager needed by this class.</param>
		public LevelSystem(GraphicsDeviceManager graphics) {
			this.graphics = graphics;
		}

		/// <summary>
		/// Assembles a level from a PlatformGenerator. This does not do anything now,
		/// but can be expanded to add extra elements to a level.
		/// </summary>
		/// <param name="generator">The generator generating the platforms in the level.</param>
		/// <returns>A finished level.</returns>
		public Level Assemble(PlatformGenerator generator) {
			var newLevel = generator.Generate();

			return newLevel;
		}

		/// <summary>
		/// Initializes a music based level so that it is ready to be played.
		/// </summary>
		/// <param name="songPath">The path to the audio file used for generation of platforms. It will also play during the level.</param>
		/// <author>Per Mortensen, Jesper Bøg</author>
		public void InitializeMusicLevel(string songPath) {
			InitializeLevel();

			// ugly workaround due to incompatibility between MediaPlayer and Uri - this should probably be filed as a bug to the XNA developers
			// taken from http://stackoverflow.com/questions/5813657/xna-4-song-fromuri-containing-spaces
			var ctor = typeof(Song).GetConstructor(
				BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(string), typeof(string), typeof(int) }, null);
			var song = (Song)ctor.Invoke(new object[] { "backgroundmusic", songPath, 0 });

			MediaPlayer.IsRepeating = false;
			MediaPlayer.IsVisualizationEnabled = true;
			MediaPlayer.Play(song);

			var generator = new MusicPlatformGenerator(graphics.GraphicsDevice.Viewport.Width);
			level = Assemble(generator);
		}

		/// <summary>
		/// Initializes a randomly generated level so that it is ready to be played.
		/// </summary>
		/// <author>Per Mortensen, Jesper Bøg</author>
		public void InitializeRandomLevel() {
			InitializeLevel();

			var generator = new RandomPlatformGenerator(graphics.GraphicsDevice.Viewport.Width);
			level = Assemble(generator);

			MediaPlayer.IsRepeating = true;
			MediaPlayer.Play(ContentBank.MusicLevel);
		}

		/// <summary>
		/// Updates the game logic such as moving sprites, checking for collisions and updating the score.
		/// </summary>
		/// <param name="gameTime">A snapshot of timing values. Many sprites use this to position themselves correctly.</param>
		public void Update(GameTime gameTime) {
			level.Update(gameTime);
			player.Update(gameTime);
			CheckPlayerWallCollision();
			CheckPlayerOutOfBounds();
			CheckProjectileOutOfBounds();
			CheckPlayerCoinCollision();
			CheckProjectileWallCollision();
			CheckEnemyProjectileCollision();
			CheckPlayerEnemyCollision();
			CheckPlayerIsRespawned();
			cursor.Update(gameTime);
			ScoreCounter.Update(gameTime);

			var keyboardState = Keyboard.GetState();
			// enter pause menu if in a level and the user presses esc or unfocuses the window
			if (keyboardState.IsKeyDown(Keys.Escape)) FireRequest(this, new SystemRequestEventArgs(SystemRequest.PauseLevel));
		}

		/// <summary>
		/// Fires a system request to the main game class.
		/// </summary>
		/// <param name="sender">The sender of the request.</param>
		/// <param name="e">The event arguments containing the request.</param>
		protected void FireRequest(object sender, SystemRequestEventArgs e) {
			if (Request != null) Request(sender, e);
		}

		/// <summary>
		/// Checks for collisions between an Enemy and a projectile.
		/// </summary>
		/// <author>Jacob Rasmussen</author>
		private void CheckEnemyProjectileCollision() {
			foreach (var enemy in level.Enemies) {
				if (!enemy.Visible) continue;

				foreach (var projectile in player.Projectiles) {
					if (!projectile.BoundingBox.Intersects(enemy.BoundingBox) || !projectile.Visible) continue;

					// Projectile goes invisible and is free to reuse.
					projectile.Visible = false;

					// Enemy gets hit and has its health reduced by one. Enemy isKilled if health = 0.
					enemy.GetHit();

					if (enemy.IsKilled) {
						UpdateScore(ScoreCounter.KILL_SCORE);

						// We need a new coin.
						bool needCoin = true;
						foreach (var coin in level.Coins) {
							if (!coin.Visible) {
								// If there is already an invisible coin in our List, we do not need a new one.
								needCoin = false;
								//Respawn the coin.
								coin.Spawn(enemy.Position);
								break;
							}
						}

						// If we still need a new coin, add one to the List.
						if (needCoin) {
							var coin = new Coin(ContentBank.SpriteCoin, enemy.Position);
							level.Coins.Add(coin);
							coin.Visible = true;
						}
					}
					break;
				}
			}
		}

		/// <summary>
		/// Checks for collisions between a player and a coin.
		/// </summary>
		/// <author>Jacob Rasmussen</author>
		private void CheckPlayerCoinCollision() {
			foreach (var coin in level.Coins) {
				if (!coin.Visible || !coin.BoundingBox.Intersects(player.BoundingBox)) continue;
				coin.Visible = false;
				UpdateScore(ScoreCounter.COIN_SCORE);
				ContentBank.SoundCoin.Play();
			}
		}

		/// <summary>
		/// Checks for a collision between a player and an enemy.
		/// </summary>
		/// <author>Jacob Rasmussen</author>
		private void CheckPlayerEnemyCollision() {
			foreach (var enemy in level.Enemies) {
				// Player is unable to get hit by an enemy immediately after a respawn.
				if (!enemy.Visible || !enemy.BoundingBox.Intersects(player.BoundingBox) || player.IsRespawned) continue;

				// Enemy also takes damage on collision.
				enemy.GetHit();

				// Player respawns from a single hit.
				player.Respawn();
				UpdateScore(ScoreCounter.DEATH_PENALTY);
				break;
			}
		}

		/// <summary>
		/// Checks if the player is respawned. This makes the player stop falling when reaching the lower platforms' height.
		/// Only when a collision with a platform happens the player is once again able to jump and die.
		/// This check makes sure that it is not possible to repeatedly fall down after a respawn.
		/// </summary>
		/// <author>Jacob Rasmussen</author>
		private void CheckPlayerIsRespawned() {
			if (player.IsRespawned && player.Position.Y + player.BoundingBox.Height - 1 >= 400) player.Speed = new Vector2(player.Speed.X, 0);
		}

		/// <summary>
		/// Checks whether or not the player is inside the viewport bounds. If the player falls outside, it respawns and Death_Penalty is subtracted from the score.
		/// </summary>
		/// <author>Jacob Rasmussen</author>
		private void CheckPlayerOutOfBounds() {
			if (player.Position.Y >= -player.BoundingBox.Height && player.Position.Y <= graphics.GraphicsDevice.Viewport.Bounds.Height + player.BoundingBox.Height) return;
			player.Respawn();
			UpdateScore(ScoreCounter.DEATH_PENALTY);
		}

		/// <summary>
		/// Checks for a collision between a player and a platform.
		/// </summary>
		/// <author>Jacob Rasmussen</author>
		private void CheckPlayerWallCollision() {
			// This check is used to let the player fall up/down automatically when a platform is removed beneath/above the player 
			// and makes sure it is not possible to jump back up(PlayerState.Midair). 
			if (player.CurrentState == PlayerState.Floor || player.CurrentState == PlayerState.Ceiling) {
				player.CurrentState = PlayerState.Midair;
				player.Speed = player.DefaultSpeed;
			}

			// If, however, a platform is still present, a collision happens and the player is placed above/below the platform and the state is set accordingly.
			const int ForgivenessFactor = 25; // Used to adjust how precise the player must land on the platforms to avoid falling down. The higher, the more forgiving.
			foreach (var platform in level.Platforms) {
				if (!player.BoundingBox.Intersects(platform.BoundingBox)) continue;

				switch (platform.Type) {
					case PlatformType.Ceiling:
						if (player.BoundingBox.Top > platform.BoundingBox.Bottom - ForgivenessFactor) {
							player.Position = new Vector2(player.Position.X, platform.BoundingBox.Bottom);
							player.CurrentState = PlayerState.Ceiling;
						}
						break;
					case PlatformType.Floor:
						if (player.BoundingBox.Bottom < platform.BoundingBox.Top + ForgivenessFactor) {
							player.Position = new Vector2(player.Position.X, platform.BoundingBox.Top - player.BoundingBox.Height);
							player.CurrentState = PlayerState.Floor;

							// Player becomes vulnerable after a respawn when colliding with a floor platform.
							player.IsRespawned = false;
						}
						break;
					case PlatformType.FinishFloor:
						FireRequest(this, new SystemRequestEventArgs(SystemRequest.EndLevel));
						break;
					case PlatformType.FinishCeiling:
						FireRequest(this, new SystemRequestEventArgs(SystemRequest.EndLevel));
						break;
				}
			}
		}

		/// <summary>
		/// Checks for collisions between a projectile and a platform.
		/// </summary>
		/// <author>Jacob Rasmussen</author>
		private void CheckProjectileWallCollision() {
			foreach (var platform in level.Platforms) {
				foreach (var projectile in player.Projectiles) {
					if (!projectile.BoundingBox.Intersects(platform.BoundingBox)) continue;
					projectile.Visible = false;
					break;
				}
			}
		}

		/// <summary>
		/// Sets the projectiles' Visible to false and makes them ready for reuse if they are outside the viewport.
		/// </summary>
		/// <author>Jacob Rasmussen</author>
		private void CheckProjectileOutOfBounds() {
			foreach (var projectile in player.Projectiles) {
				if (projectile.BoundingBox.Left > graphics.GraphicsDevice.Viewport.Bounds.Right || projectile.BoundingBox.Right < 0
				    || projectile.BoundingBox.Top > graphics.GraphicsDevice.Viewport.Bounds.Bottom
				    || projectile.BoundingBox.Bottom < 0) projectile.Visible = false;
			}
		}

		/// <summary>
		/// Performs common level initialization, used by both music based and randomly generated levels.
		/// </summary>
		private void InitializeLevel() {
			MediaPlayer.Stop();
			player = new Player(ContentBank.SpritePlayer);
			cursor = new MouseCursor(ContentBank.SpriteCursor, new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
			ScoreCounter = new ScoreCounter(0);
		}

		/// <summary>
		/// Updates the score counter.
		/// </summary>
		/// <param name="addScore">The points to be added.</param>
		/// <author>Per Mortensen</author>
		private void UpdateScore(int addScore) {
			var position = new Vector2(player.Position.X + 100, player.Position.Y - 20);
			ScoreCounter.UpdateScore(addScore, position);
		}

		/// <summary>
		/// Draws the level and all the game elements to the screen.
		/// </summary>
		/// <param name="spriteBatch">The sprite batch with which to draw the level.</param>
		public void Draw(SpriteBatch spriteBatch) {
			level.Draw(spriteBatch);

			//Tints the player's color to red after a respawn.
			player.Color = player.IsRespawned ? Color.Red : Color.White;
			player.Draw(spriteBatch);
			cursor.Draw(spriteBatch);
			ScoreCounter.Draw(spriteBatch);
		}
	}
}