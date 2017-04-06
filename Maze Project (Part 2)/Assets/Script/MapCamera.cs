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

public class MapCamera : MonoBehaviour {
	// ===============================
	// ==	VARIABLE DECLARATIONS	==
	// ===============================

	// Cameras
	// *******
	private Camera _camera;


	// =======================
	// ==	FUNCTION CALLS	==
	// =======================

	void Start () {
		// initialize
		_camera = GetComponent<Camera> ();

		// find tangent
		float toa = (Mathf.Tan (30)) * -1 + (FormGrid.xz * 1.8f);

		// set camera position
		_camera.transform.position = new Vector3 (FormGrid.xz, toa, FormGrid.xz);
	}
}
