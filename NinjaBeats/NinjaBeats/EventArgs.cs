namespace NinjaBeats {
	using System;

	/// <summary>
	/// Event arguments with an action to be performed by the menu subsystem.
	/// </summary>
	/// <author>Per Mortensen</author>
	public class MenuEventArgs : EventArgs {
		public readonly MenuAction Action;

		public MenuEventArgs(MenuAction action) {
			Action = action;
		}
	}

	/// <summary>
	/// Event arguments with an request for an action, which only the main game class can perform.
	/// </summary>
	/// <author>Per Mortensen</author>
	public class SystemRequestEventArgs : EventArgs {
		public readonly SystemRequest Request;

		public SystemRequestEventArgs(SystemRequest request) {
			Request = request;
		}
	}
}