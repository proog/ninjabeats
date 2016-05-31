namespace NinjaBeats {
	using System.Collections.Generic;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	/// <summary>
	/// The credits screen.
	/// </summary>
	/// <author>Per Mortensen</author>
	public class Credits : Menu {
		private const string TITLE = "NinjaBeats";
		private const string ART_DIRECTION = "Art Direction:\nJacob Rasmussen\nCasper Hansen\n\n";
		private const string GAME_PROGRAMMING = "Game Programming:\nJesper Bøg\nJacob Rasmussen\nPer Mortensen\n\n";
		private const string AUDIO_PROGRAMMING = "Audio Programming:\nJesper Bøg\n\n";
		private const string SOUND_DIRECTION = "Sound Direction:\nPer Mortensen\n\n";
		private const string MUSIC = "Music:\nReyn Ouwehand\nJeroen Tel\n\n";
		private const string INSPIRED_BY = "Inspired by:\nYoo Ninja\nBeat Hazard\n\n";
		private const string COPYRIGHT = "Copyright © 2011 Team NinjaBeats\n\n";
		
		private const string DISPLAY = ART_DIRECTION + GAME_PROGRAMMING + AUDIO_PROGRAMMING + SOUND_DIRECTION + MUSIC + INSPIRED_BY + COPYRIGHT;
		private Vector2 titlePosition = new Vector2(300, 500);
		private Vector2 creditsPosition = new Vector2(300, 600);
		private Vector2 creditsSize = ContentBank.FontStandard.MeasureString(DISPLAY);

		public Credits(Background background, List<Button> buttons) : base(background, buttons) { }

		public override void Update(GameTime gameTime) {
			titlePosition.Y--;
			creditsPosition.Y--;

			if (creditsPosition.Y <= -creditsSize.Y) {
				titlePosition = new Vector2(300, 500);
				creditsPosition = new Vector2(300, 600);
			}

			base.Update(gameTime);
		}

		public override void Draw(SpriteBatch spriteBatch) {
			base.Draw(spriteBatch);
			spriteBatch.DrawString(ContentBank.FontStandardLarge, TITLE, titlePosition, Color.White);
			spriteBatch.DrawString(ContentBank.FontStandard, DISPLAY, creditsPosition, Color.White);
		}
	}
}