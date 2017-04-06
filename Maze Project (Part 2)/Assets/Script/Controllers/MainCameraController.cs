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

public class MainCameraController : MonoBehaviour, IGameManager {
	// ===============================
	// ==	VARIABLE DECLARATIONS	==
	// ===============================

	// Instantiation Handlers
	// **********************
	[SerializeField] private Camera _camera;
	public static GameObject _playerCam;

	// External Script Variables
	// *************************
	public ManagerStatus status { get; private set; }

	// Vector 3
	// ********
	private Vector3 endPos;				// determines the location of the player camera
	private Vector3 startPos;			// determines the location of the main camera (before Lerp)
	public static Vector3 target;		// determines the center of the maze

	// Floats and Integers
	// *******************
	private float timeSpent = 0;		// determines how much time has elapsed since beginning Lerp
	private float rate = 0;				// determines the rate at which the camera will move in Lerp


	// =======================
	// ==	FUNCTION CALLS	==
	// =======================

	void Awake () {
		// find tangent
		float toa = (Mathf.Tan (30)) * -1 + (FormGrid.xz * 1.8f);

		// set main camera position
		_camera.transform.position = new Vector3 (FormGrid.xz, toa, FormGrid.xz);
	}


	public void StartUp() {
		// get player camera
		_playerCam = GameObject.Find("playerCamera");

		// get pos of player camera
		endPos = _playerCam.transform.position;
		startPos = _camera.transform.position;

		Debug.Log ("Camera starting...");
		status = ManagerStatus.Started;
	}


	void Update () {
		rate = 0;

		// Lerp camera based at the speed of "rate"
		// to the position of the player camera while
		// looking at the center of the maze.
		if (status == ManagerStatus.Started) {
			timeSpent += Time.deltaTime;
			rate = timeSpent / 4;
			_camera.transform.position = Vector3.Lerp (startPos, endPos, rate);
			_camera.transform.LookAt (target);

			// Set playerMoveable to true; allows game to
			// move on to next game state.
			if (_camera.transform.position == endPos) {
				CameraController.playerMovable = true;
				TimeController._start = true;
			}
		}
	}
}
