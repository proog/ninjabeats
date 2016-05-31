namespace NinjaBeats {
	/// <summary>
	/// The current state of the game as a whole.
	/// </summary>
	/// <author>Jacob Rasmussen, Per Mortensen, Jesper Bøg</author>
	public enum GameState {
		Menu,
		Level
	}

	/// <summary>
	/// The current positioning state of the Player.
	/// </summary>
	/// <author>Jacob Rasmussen</author>
	public enum PlayerState {
		Floor,
		Ceiling,
		Midair
	}

	/// <summary>
	/// The action of a clicked button in the menu.
	/// </summary>
	/// <author>Per Mortensen</author>
	public enum MenuAction {
		Resume,
		Exit,
		MainMenu,
		SelectGameType,
		StartRandomGame,
		SelectSong,
		LoadSong,
		StartMusicGame,
		HighScores,
		EnterName,
		SubmitHighScore,
		Credits
	}

	/// <summary>
	/// A request for the topmost system, BdsaGame, to perform a certain
	/// action that the menu system does not have access to directly.
	/// </summary>
	/// <author>Per Mortensen</author>
	public enum SystemRequest {
		StartRandomLevel,
		StartMusicLevel,
		ExitGame,
		ResumeLevel,
		EndLevel,
		PauseLevel,
		ShowCredits
	}
}