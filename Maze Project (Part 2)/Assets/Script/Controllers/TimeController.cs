// *******************************
// Calgary Seth Michael			**
// Maze Project					**
// April 2, 2016				**
// *******************************

/*
 * Script determines when to start game, and
 * how many elapsed seconds the player has
 * run the instance.
 * 
 * Script uses System.Diagnostics for the
 * StopWatch function
 * 
 * Run time and Start time are both displayed
 * in the upper left hand corner of the player's
 * screen.
 * 
 * KNOWN BUGS
 * ----------
 * N/A
 * 
 */

using UnityEngine;
using System.Collections;

public class TimeController : MonoBehaviour {
	// ===============================
	// ==	VARIABLE DECLARATIONS	==
	// ===============================

	// Timer
	// *****
	System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();

	// Boolean
	// *******
	public bool _running = false;			// determines whether the timer is in a running state
	public static bool _start = false;		// determines whether the timer has begun
	public static bool _stop = false;		// determines whether the timer has stopped


	// =======================
	// ==	FUNCTION CALLS	==
	// =======================

	void OnGUI () {
		if (_start || _stop) {
			if (_stop) {
				GUI.contentColor = Color.red;
			} else {
				GUI.contentColor = Color.black;	
			}

			GUI.Label (new Rect (10,5,1000,50), "Time: "+timer.Elapsed);	
		}
	}


	void Update() {

		// "_running" is used to determine whether
		// the stopwatch has been started or not.
		// If the stopwatch has beem started,
		// it will not stop until triggered by 
		// PlayerController script
		if (_start && !_running) {
			timer.Start ();
			_running = true;
		} else if (_stop && _running){
			timer.Stop ();
		}
	}
}
