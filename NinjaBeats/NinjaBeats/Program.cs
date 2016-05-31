namespace NinjaBeats {
	using System;

#if WINDOWS
	public static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// STAThread due to the Windows Forms file dialog.
		/// </summary>
		[STAThread]
		private static void Main(string[] args) {
			using (var game = new BdsaGame()) game.Run();
		}
	}
#endif
}