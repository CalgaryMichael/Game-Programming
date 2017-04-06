// *******************************
// Calgary Seth Michael			**
// Maze Project					**
// April 2, 2016				**
// *******************************

using UnityEngine;
using System.Collections;


[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]
public class FPSInput : MonoBehaviour, IGameManager {
	// ===============================
	// ==	VARIABLE DECLARATIONS	==
	// ===============================

	// External Script Variables
	// *************************
	public ManagerStatus status { get; private set; }

	// Floats and Integers
	// *******************
	public float speed = 6.0f;
	public float gravity = -9.8f;

	// Miscellaneous
	// *************
	private CharacterController _charController;


	// =======================
	// ==	FUNCTION CALLS	==
	// =======================

	public void StartUp() {
		status = ManagerStatus.Started;
	}
		

	void Start() {
		_charController = GetComponent<CharacterController>();
	}

	
	void Update() {
		if (CameraController.playerMovable) {
			float deltaX = Input.GetAxis ("Horizontal") * speed;
			float deltaZ = Input.GetAxis ("Vertical") * speed;
			Vector3 movement = new Vector3 (deltaX, 0, deltaZ);
			movement = Vector3.ClampMagnitude (movement, speed);

			movement.y = gravity;

			movement *= Time.deltaTime;
			movement = transform.TransformDirection (movement);
			_charController.Move (movement);
		}
	}
}
