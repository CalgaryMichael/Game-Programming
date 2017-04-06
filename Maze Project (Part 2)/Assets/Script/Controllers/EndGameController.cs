// *******************************
// Calgary Seth Michael			**
// Maze Project					**
// April 2, 2016				**
// *******************************

/*
 * KNOWN BUGS
 * ----------
 * N/A
 * 
 */

using UnityEngine;
using System.Collections;

public class EndGameController : MonoBehaviour {

	// function stops TimeController's timer
	// when collision is detected.
	void OnTriggerEnter (Collider _collider) {
		TimeController._stop = true;
	}
}
