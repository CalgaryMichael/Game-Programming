// *******************************
// Calgary Seth Michael			**
// Maze Project					**
// April 2, 2016				**
// *******************************

/*
 * KNOWN BUGS
 * ----------
 * Debug Log will return Error 
 * messages referring to line
 * 40. This is because the 
 * component that it is referencing
 * is not active until FormEnd()
 * function has been called from
 * the FormGrid script.
 * 
 */

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	// ===============================
	// ==	VARIABLE DECLARATIONS	==
	// ===============================

	// Camera Controller
	// *****************
	[SerializeField] private Camera _mainCam;
	[SerializeField] private Camera _mapCam;
	[SerializeField] private MouseLook _player;
	private Camera _playerCam;

	// Boolean
	// *******
	public static bool playerMovable = false;		// determines whether the player is in active position
	private bool mapActive = false;					// determines whether the map camera is currently displayed on screen

	// =======================
	// ==	FUNCTION CALLS	==
	// =======================

	void Update () {
		_playerCam = MainCameraController._playerCam.GetComponent<Camera> ();

		// Initialize cameras to be inactive
		// if game state is not in player state
		if (!playerMovable) {
			_mapCam.enabled = false;
			_playerCam.enabled = false;
			_player.enabled = false;
			
		} else if (playerMovable) {
			
			// control map camera
			// ******************
			if (Input.GetKeyDown("m") && !mapActive) {
				_mapCam.enabled = true;
				mapActive = true;

			} else if (Input.GetKeyDown("m") && mapActive) {
				_mapCam.enabled = false;
				mapActive = false;
			}	

			// set player
			// **********
			_mainCam.gameObject.tag = "Untagged";
			_playerCam.gameObject.tag = "MainCamera";

			_player.enabled = true;
			_playerCam.enabled = true;
			_mainCam.enabled = false;
		}
	}
}
