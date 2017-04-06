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

public class FormGrid : MonoBehaviour, IGameManager {
	// ===============================
	// ==	VARIABLE DECLARATIONS	==
	// ===============================

	// Instantiation Handlers
	// **********************
	[SerializeField] private GameObject wallPrefab;
	[SerializeField] private GameObject postPrefab;
	[SerializeField] private GameObject character;
	[SerializeField] private GameObject exit;
	private GameObject _player;
	private GameObject _wallVer;
	private GameObject _wallHor;
	private GameObject _post;
	private GameObject _exit;
	public GameObject Parent;

	// External Script Variables
	// *************************
	public ManagerStatus status { get; private set; }
	private FormMaze _maze;							// to call functions from FormMaze

	// Floats and Integers
	// *******************
	public static int SIZE = 10;					// determines size of grid
	private int middle = 0;							// to find the center of the maze
	public float length = 4f;						// size of wall
	public float origin = 0.5f;						// starting place for maze
	private float wallBuff = 0;						// to align walls to touch posts
	public static float xz = 0;						// determines the x/z coord. for main and map cameras

	// Structures
	// **********
	public struct Cell {
		public GameObject northWall;
		public GameObject eastWall;
		public GameObject postNE;
		public int row;
		public int col;
	}

	// Arrays
	// ******
	private Cell[,] grid = new Cell[SIZE + 2, SIZE + 2];		// to keep track of the grid


	// =======================
	// ==	FUNCTION CALLS	==
	// =======================

	public void StartUp() {
		// initialize "_maze"
		_maze = GetComponent<FormMaze> ();
	
		xz = (((SIZE * length) + SIZE) / 2) + 0.5f;
		wallBuff = ((length / 2) + 1);

		// construct the grid
		GridConstructor ();

		// find center
		if (SIZE % 2 != 0) {
			middle = (SIZE / 2) + 1;
		} else {
			middle = (SIZE / 2);
		}

		// pass the center to 
		MainCameraController.target = GetPost (middle, middle);

		status = ManagerStatus.Initializing;
	}


	void Update () {
		// construct maze after StartUp() has been called
		if (status == ManagerStatus.Initializing) {
			_maze.MazeConstructor (grid);
		}
	}


	// GRID CONSTRUCTOR
	// ****************
	// instantiates posts, horizontal
	// and vertical walls. After creating
	// one of each, calles BuildArray to
	// add those items to the "grid" array.
	public void GridConstructor () {
		// i = col
		// j = row
		for (int i = 0; i < (SIZE + 1); i++) {
			for (int j = 0; j < (SIZE + 1); j++) {

				// create posts
				_post = Instantiate (postPrefab) as GameObject;
				_post.transform.parent = Parent.transform;
				_post.transform.position = new Vector3 (((j * (length + 1)) + origin), 2,
					((i * (length + 1)) + origin));
			
				// create horizontal walls
				if (j < SIZE) {
					_wallHor = Instantiate (wallPrefab) as GameObject;
					_wallHor.transform.parent = Parent.transform;
					_wallHor.transform.position = new Vector3 (((j * (length + 1)) + wallBuff), 2,
						((i * (length + 1)) + origin));
					_wallHor.transform.localScale = new Vector3 (length, 4, 1);
				}

				// create vertical walls
				if (i < SIZE) {
					_wallVer = Instantiate (wallPrefab) as GameObject;
					_wallVer.transform.parent = Parent.transform;
					_wallVer.transform.position = new Vector3 (((j * (length + 1)) + origin), 2,
						((i * (length + 1)) + wallBuff));
					_wallVer.transform.rotation = Quaternion.Euler (new Vector3 (0, 90, 0));
					_wallVer.transform.localScale = new Vector3 (length, 4, 1);
				}

				// create array
				BuildArray (_wallHor, _wallVer, _post, i, j);
			}
		}
	}


	// BUILD ARRAY
	// ***********
	// adds the north wall, east wall,
	// and post to the "grid" array.
	// These items are added to the
	// row/col that are passed to it.
	void BuildArray (GameObject wallN, GameObject wallE, GameObject wallPost, int row, int col) {
		grid [row, (col + 1)].northWall = wallN;
		grid [(row + 1), col].eastWall = wallE;
		grid [row, col].postNE = wallPost;
		grid [row, col].row = row;
		grid [row, col].col = col;
	}


	// GET WALL NORTH
	// **************
	// a getter function that
	// returns the Vector 3 location
	// of the north wall in "row" and
	// "col" that is passed to it.
	public Vector3 GetWallN (int row, int col) {
		Vector3 pos = grid [row, col].northWall.gameObject.transform.position;
		return pos;
	}


	// GET WALL EAST
	// *************
	// a getter function that
	// returns the Vector 3 location
	// of the east wall in "row" and
	// "col" that is passed to it.
	public Vector3 GetWallE (int row, int col) {
		Vector3 pos = grid [row, col].eastWall.gameObject.transform.position;
		return pos;
	}


	// GET POST
	// ********
	// a getter function that
	// returns the Vector 3 location
	// of the post in "row" and "col"
	// that is passed to it.
	public Vector3 GetPost (int row, int col) {
		Vector3 pos = grid [row, col].postNE.gameObject.transform.position;
		return pos;
  	}


	// FORM END
	// ********
	// instantiates beginning/end
	// sections. It spawns the character
	// in the middle of the begin section
	// and points it at the center of the
	// maze. The Exit collider is also
	// spawned from this script.
	//
	// Function is called from FormMaze.
	// This is to make sure that the ends
	// are placed after the maze has been
	// constructed.
	public void EndConstructor(int randStart, int randEnd) {
		Vector3 posEndWall = GetWallE (randEnd, (SIZE));
		Vector3 posEndPost = GetPost (randEnd, SIZE);
		Vector3 posStartPost = GetPost (randStart, 0);

		Vector3 front = GetWallE(randStart, 0);
		Vector3 back;

		// BEGINNING SECTION
		// ****************************

		// create beginning top wall
		_wallHor = Instantiate (wallPrefab) as GameObject;
		_wallHor.transform.localScale = new Vector3 (length, 4, 1);
		_wallHor.transform.position = new Vector3 (((length / 2) * -1f), 2f, posStartPost.z);
		_wallHor.transform.parent = Parent.transform;
		back = _wallHor.transform.position;

		// create beginning top post
		_post = Instantiate (postPrefab) as GameObject;
		_post.transform.position = new Vector3 (((length + origin) * -1f), 2f, posStartPost.z);
		_post.transform.parent = Parent.transform;

		// create beginning back wall
		_wallVer = Instantiate (wallPrefab) as GameObject;
		_wallVer.transform.localScale = new Vector3 (length, 4, 1);
		_wallVer.transform.rotation = Quaternion.Euler (new Vector3 (0, 90, 0));
		_wallVer.transform.position = new Vector3 (((length + origin) * -1), 2f, (posStartPost.z - origin - (length / 2)));
		_wallVer.transform.parent = Parent.transform;

		// create beginning bottom post
		_post = Instantiate (postPrefab) as GameObject;
		_post.transform.position = new Vector3 (((length + origin) * -1f), 2f, (posStartPost.z - (length + 1)));
		_post.transform.parent = Parent.transform;

		// create beginning bottom wall
		_wallHor = Instantiate (wallPrefab) as GameObject;
		_wallHor.transform.localScale = new Vector3 (length, 4, 1);
		_wallHor.transform.position = new Vector3 (((length / 2) * -1f), 2f, (posStartPost.z - (length + 1)));
		_wallHor.transform.parent = Parent.transform;

		// ENDING SECTION
		// ****************************

		// create ending top wall
		_wallHor = Instantiate (wallPrefab) as GameObject;
		_wallHor.transform.localScale = new Vector3 (length, 4, 1);
		_wallHor.transform.position = new Vector3 ((posEndPost.x + origin + (length / 2)), 2f, posEndPost.z);
		_wallHor.transform.parent = Parent.transform;

		// create ending top post
		_post = Instantiate (postPrefab) as GameObject;
		_post.transform.position = new Vector3 ((posEndPost.x + 1 + length), 2f, posEndPost.z);
		_post.transform.parent = Parent.transform;

		// create ending back wall
		_wallVer = Instantiate (wallPrefab) as GameObject;
		_wallVer.transform.localScale = new Vector3 (length, 4, 1);
		_wallVer.transform.rotation = Quaternion.Euler (new Vector3 (0, 90, 0));
		_wallVer.transform.position = new Vector3 ((posEndPost.x + 1 + length), 2f, (posEndPost.z - origin - (length / 2)));
		_wallVer.transform.parent = Parent.transform;

		// create ending bottom post
		_post = Instantiate (postPrefab) as GameObject;
		_post.transform.position = new Vector3 ((posEndPost.x + 1 + length), 2f, (posEndPost.z - (length + 1)));
		_post.transform.parent = Parent.transform;

		// create ending bottom wall
		_wallHor = Instantiate (wallPrefab) as GameObject;
		_wallHor.transform.localScale = new Vector3 (length, 4, 1);
		_wallHor.transform.position = new Vector3 ((posEndPost.x + origin + (length / 2)), 2f, (posEndPost.z - (length + 1)));
		_wallHor.transform.parent = Parent.transform;

		// CHARACTER SPAWN
		// ****************************

		Vector3 pos = new Vector3 ((back.x - front.x), 1f, front.z);
		_player = Instantiate (character) as GameObject;
		_player.transform.position = pos;
		_player.transform.LookAt (MainCameraController.target);

		// EXIT COLLIDER
		// ****************************

		_exit = Instantiate (exit) as GameObject;
		_exit.transform.localScale = new Vector3 (length, 4, 1);
		_exit.transform.position = posEndWall;
		_exit.transform.rotation = Quaternion.Euler (new Vector3 (0, 90, 0));
	
		// After the sections have been placed,
		// the game will be set to continue
		// to the next state.
		Debug.Log ("Form Grid finished...");
		status = ManagerStatus.Started;
	}
}
