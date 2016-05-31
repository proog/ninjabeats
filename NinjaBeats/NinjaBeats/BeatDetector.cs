using System;
using Microsoft.Xna.Framework.Media;

namespace NinjaBeats {
	/// <summary>
	/// Detects beats in the currently playing music.
	/// </summary>
	/// <author>Jesper Bøg</author>
	class BeatDetector {
		// Visualisation data used for analysis.
		private readonly VisualizationData visData;

		// Frequency bands within which to look.
		private readonly int lowLow;
		private readonly int lowHi;
		private readonly int midLow;
		private readonly int midHi;
		private readonly int highLow;
		private readonly int highHi;

		// Logging of the last second of music.
		private int logIndex;
		private readonly float[] lowLog;
		private readonly float[] midLog;
		private readonly float[] highLog;

		// Miscellaneous local variables
		private float lowCur;
		private float lowAvg;
		private float lowVar;
		private float lowDev;
		private float midCur;
		private float midAvg;
		private float midVar;
		private float midDev;
		private float highCur;
		private float highAvg;
		private float highVar;
		private float highDev;

		private bool lowBeat;
		private bool midBeat;
		private bool highBeat;

		/// <summary>
		/// Initializes the beat detector, so it's ready to start analyzing data from the media player.
		/// </summary>
		/// <author>Jesper Bøg</author>
		public BeatDetector() {
			// Initialize the VD, so we can actually use it :P
			visData = new VisualizationData();

			// Which bands are we interested in?
			lowLow = 0;        // Cirka 20hz
			lowHi = 81;        // Cirka 180hz
				// http://www.avsforum.com/avs-vb/showthread.php?t=1284031
				// Most bass/kick drums are between 30 and 175 hertz, apparently.
			midLow = 119;      // Cirka 300hz
			midHi = 126;       // Cirka 600hz
				// http://www.easyeartraining.com/2010/03/23/percussion-frequencies-part-1-drums/
				// Probably about where those "mid-toms" are.
			highLow = 222;     // Cirka 8000hz
			highHi = 230;      // Cirka 10000hz
				// http://www.digitalprosound.com/2002/03_mar/tutorials/mixing_excerpt1.htm
				// Hi hats and cymbals.
			// Hertz -> ArrayIndex calculated using: (log(x) - 1,30103)/0,01171875 = y, where x = frequency and y = bucket.

			// Initialize the log
			logIndex = 0;
			lowLog = new float[60];
			lowLog[59] = -1f;          // So we can check if the log has been filled
			midLog = new float[60];
			highLog = new float[60];
		}

		/// <summary>
		/// Updates the beat detectors data with the newest available data from the media player.
		/// </summary>
		/// <author>Jesper Bøg</author>
		public void Update() {
			if (MediaPlayer.State != MediaState.Playing) return;
				
			// Get the current music data
			MediaPlayer.GetVisualizationData(visData);

			// Get the parts we're interested in
			lowCur = GetPower(lowLow, lowHi);
			midCur = GetPower(midLow, midHi);
			highCur = GetPower(highLow, highHi);

			// Add the data to the log
			lowLog[logIndex] = lowCur;
			midLog[logIndex] = midCur;
			highLog[logIndex] = highCur;

			// If we've filled the log, start calculating.
			if (IsReady()) {
				// Determine averages, variance and deviance for the selected frequency bands over the last second.
				lowAvg = AveragePower(lowLog);
				lowVar = PowerVariance(lowLog, lowAvg);
				lowDev = PowerDeviance(lowVar);
				midAvg = AveragePower(midLog);
				midVar = PowerVariance(midLog, midAvg);
				midDev = PowerDeviance(midVar);
				highAvg = AveragePower(highLog);
				highVar = PowerVariance(highLog, highAvg);
				highDev = PowerDeviance(highVar);
			}

			// Finally, increment the log counter.
			logIndex++;
			if (logIndex > 59) {
				logIndex = 0;
			}
		}

		/// <summary>
		/// Checks if there is a beat in the music right at this instant.
		/// </summary>
		/// <returns>True if there's a beat, false if not.</returns>
		/// <author>Jesper Bøg</author>
		// This method is intentionally left "odd".
		// The final constant of each if-statement and the boolean expression in the final if,
		// are the values that need fiddling to improve beat detection.
		public bool IsBeat() {
			if (IsReady()) {
				lowBeat = false;
				midBeat = false;
				highBeat = false;

				if (lowCur - lowAvg > lowDev * 1.1) lowBeat = true;
				if (midCur - midAvg > midDev * 1.1) midBeat = true;
				if (highCur - highAvg > highDev * 1.1) highBeat = true;
				if (lowBeat || midBeat || highBeat) return true;
			}
			return false;
		}

		/// <summary>
		/// Is the beat detector ready to start calculating things?
		/// </summary>
		/// <author>Jesper Bøg</author>
		private bool IsReady() {
			return lowLog[59] >= 0;
		}

		/// <summary>
		/// Calculates the average amplitude of the frequencies between the specified bands.
		/// </summary>
		/// <author>Jesper Bøg</author>
		private float GetPower(int firstBand, int lastBand) {
			float power = 0;
			for (int i = firstBand; i <= lastBand; i++) {
				power += visData.Frequencies[i];
			}
			return power / (lastBand - firstBand);
		}

		/// <summary>
		/// Calculates the average power level of a given log.
		/// </summary>
		/// <author>Jesper Bøg</author>
		private static float AveragePower(float[] log) {
			float average = 0;
			foreach (float f in log) average += f;
			return average / log.Length;
		}

		/// <summary> 
		/// Calculates the variance of power levels in a given log.
		/// </summary>
		/// <author>Jesper Bøg</author>
		private static float PowerVariance(float[] log, float average) {
			float variance = 0;
			foreach (float f in log) {
				float difference = f - average;
				variance += difference * difference;
			}
			return variance / log.Length;
		}

		/// <summary>
		/// Determines the standard deviation of a given log.
		/// </summary>
		/// <author>Jesper Bøg</author>
		private static float PowerDeviance(float variance) {
			return (float)Math.Sqrt(variance);
		}
	}
}
