namespace NinjaBeats {
	using Microsoft.Xna.Framework;

	/// <summary>
	/// A platform is an object in the game world on which the player can run.
	/// </summary>
	/// <author>Jesper Bøg</author>
	public class Platform : Sprite {
		public readonly PlatformType Type;

		private const int CEILING_HEIGHT = 50; // Vertical position of ceiling platforms

		private const int FLOOR_HEIGHT = 400; // Vertical position of floor platforms

		private const int SCROLLING_DIRECTION = -1; // Direction of the platforms' movement

		private const int SCROLLING_SPEED = 250; // Horizontal speed of the platforms

		private static readonly AnimatedTexture DefaultTexture = ContentBank.SpritePlatform;

		private static readonly AnimatedTexture FinishTexture = ContentBank.SpritePlatformFinish;

		public static readonly int Width = DefaultTexture.Width;

		/// <summary>
		/// Constructs a new platform.
		/// </summary>
		/// <param name="position">Where, horizontally, the platform should be spawned.</param>
		/// <param name="type">Which type of platform this is; Ceiling, Floor, FinishCeiling or FinishFloor</param>
		/// <author>Jesper Bøg</author>
		public Platform(float position, PlatformType type)
			: base(
				type == PlatformType.Ceiling || type == PlatformType.Floor ? DefaultTexture : FinishTexture,
				new Vector2(
					position, type == PlatformType.Floor || type == PlatformType.FinishFloor ? FLOOR_HEIGHT : CEILING_HEIGHT)) {
			Direction = new Vector2(SCROLLING_DIRECTION, 0);
			Speed = new Vector2(SCROLLING_SPEED, 0);
			Type = type;
		}

		public override void Update(GameTime gametime) {
			Visible = BoundingBox.Right > 0 && BoundingBox.Left < 800;
			base.Update(gametime);
		}
	}

	/// <summary>
	/// The type of a platform.
	/// </summary>
	public enum PlatformType {
		Floor,
		Ceiling,
		FinishFloor,
		FinishCeiling
		}
	}
