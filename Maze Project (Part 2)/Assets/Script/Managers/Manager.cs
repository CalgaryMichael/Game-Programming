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
using System.Collections.Generic;

[RequireComponent(typeof(FormGrid))]
[RequireComponent(typeof(FormMaze))]
[RequireComponent(typeof(MainCameraController))]

public class Manager : MonoBehaviour {
	// ===============================
	// ==	VARIABLE DECLARATIONS	==
	// ===============================

	// External Script Variable
	// ************************
	public static FormGrid _grid { get; private set; }
	public static FormMaze _maze { get; private set; }
	public static MainCameraController _camera { get; private set; }

	// Stacks / Lists
	// **************
	private List<IGameManager> _startSequence;


	// =======================
	// ==	FUNCTION CALLS	==
	// =======================

	void Awake() {
		_grid = GetComponent<FormGrid> ();
		_maze = GetComponent<FormMaze> ();
		_camera = GetComponent<MainCameraController> ();

		// add components to a List
		_startSequence = new List<IGameManager> ();
		_startSequence.Add (_grid);
		_startSequence.Add (_maze);
		_startSequence.Add (_camera);

		StartCoroutine (StartUpManagers ());
	}


	private IEnumerator StartUpManagers () {
		int numModules = _startSequence.Count;
		int numReady = 0;

		// Set Game Mode to form grid
		_grid.StartUp ();

		yield return null;

		while (numReady < numModules) {
			numReady = 1;

			// Set Game Mode to form maze
			if (_grid.status == ManagerStatus.Started) {
				_maze.StartUp ();
				numReady++;

				// Set game mode to main camera Lerp
				if (_maze.status == ManagerStatus.Started) {
					_camera.StartUp ();
					numReady++;
				}
			}
			

			yield return null;
		}

		Debug.Log ("All managers started up");
	}
}
