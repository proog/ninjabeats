namespace NinjaBeats {
	using System.Collections.Generic;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input;

	/// <summary>
	/// The user-controlled protagonist in the game. The player is able to jump between
	/// floor and ceiling and shoot projectiles.
	/// </summary>
	/// <author>Jacob Rasmussen</author>
	public class Player : Sprite {
		// Speed in MidAir. Speed on floor/ceiling is the default Zero speed inherited from Sprite.
		public readonly Vector2 DefaultSpeed = new Vector2(0, 600);
		public readonly Vector2 SpawnSpeed = new Vector2(0, 200);

		public readonly List<Projectile> Projectiles = new List<Projectile>();

		// Player starts the game in MidAir.
		public PlayerState CurrentState = PlayerState.Midair;
		public bool IsRespawned;
		private static readonly Vector2 StartingPosition = new Vector2(50, 150);
		private readonly Vector2 moveDown = new Vector2(0, 1);
		private readonly Vector2 moveUp = new Vector2(0, -1);

		// Used to ignore held-down buttons.
		private KeyboardState previousKeyboardState;
		private MouseState previousMouseState;

		public Player(AnimatedTexture texture) : base(texture, StartingPosition) {
			Direction = moveDown;
			Speed = SpawnSpeed;
		}

		/// <summary>
		/// Responds to keyboardinput and updates the player's position accordingly. 
		/// </summary>
		/// <param name="keyboardState">The current keyboard state.</param>
		public void Jump(KeyboardState keyboardState) {
			if (keyboardState.IsKeyDown(Keys.Space) && !previousKeyboardState.IsKeyDown(Keys.Space) && CurrentState == PlayerState.Floor) {
				Direction = moveUp;
				Speed = DefaultSpeed;
				CurrentState = PlayerState.Midair;
			} else if (keyboardState.IsKeyDown(Keys.Space) && !previousKeyboardState.IsKeyDown(Keys.Space) && CurrentState == PlayerState.Ceiling) {
				Direction = moveDown;
				Speed = DefaultSpeed;
				CurrentState = PlayerState.Midair;
			}
		}

		/// <summary>
		/// Respawns the player at the starting position with a reduced fall speed.
		/// </summary>
		public void Respawn() {
			Position = StartingPosition;
			IsRespawned = true;
			Direction = moveDown;
			Speed = SpawnSpeed;
			CurrentState = PlayerState.Midair;
		}

		/// <summary>
		/// Shoots a projectile from the player's position to the mouse's position at the time of the click.
		/// </summary>
		/// <param name="mouseState">The current state of the mouse.</param>
		public void Shoot(MouseState mouseState) {
			if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed) {
				int mouseX = mouseState.X;
				int mouseY = mouseState.Y;

				var projectileDirection = new Vector2(mouseX - Position.X, mouseY - Position.Y);

				// At first we need a new projectile.
				bool needNew = true;
				foreach (var projectile in Projectiles) {
					if (!projectile.Visible) {
						// If we already have a projectile in our Projectile-List that is not visible, we do not need another.
						needNew = false;
						//Move it to the player's position.
						projectile.Position = Position;
						//Fire the projectile towards the click point.
						projectile.Fire(projectileDirection);
						break;
					}
				}

				// If we still need a new projectile, add it to our List.
				if (needNew) Projectiles.Add(new Projectile(ContentBank.SpriteProjectile, Position, projectileDirection));

				ContentBank.SoundShoot.Play();
			}
		}

		public override void Update(GameTime gameTime) {
			var keyboardState = Keyboard.GetState();
			var mouseState = Mouse.GetState();

			Jump(keyboardState);
			previousKeyboardState = keyboardState;

			Shoot(mouseState);
			previousMouseState = mouseState;

			foreach (var projectile in Projectiles) projectile.Update(gameTime);

			// Flip player texture if it's going upwards.
			Texture.Flipped = Direction.Y < 0;

			// Stop texture animation while in Midair.
			switch (CurrentState) {
				case PlayerState.Midair:
					Texture.Paused = true;
					break;
				default:
					Texture.Paused = false;
					break;
			}

			base.Update(gameTime);
		}

		/// <summary>
		/// Draw the sprite on the screen.
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch used to draw the sprite</param>
		public override void Draw(SpriteBatch spriteBatch) {
			foreach(var projectile in Projectiles) projectile.Draw(spriteBatch);

			base.Draw(spriteBatch);
		}
	}
}