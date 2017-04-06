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

public class FormMaze : MonoBehaviour, IGameManager {
	// ===============================
	// ==	VARIABLE DECLARATIONS	==
	// ===============================

	// External Script Variables
	// *************************
	public ManagerStatus status { get; private set; }
	public FormGrid _grid { get; private set; }

	// Stacks / Lists
	// **************
	public Stack<FormGrid.Cell> gridStack = new Stack<FormGrid.Cell>();		// keeps track of which neighbors are unvisited
	public List<FormGrid.Cell> neighborList = new List<FormGrid.Cell>();	// stacks which cells have been visited

	// Booleans
	// ********
	private bool mazeStart = false;					// to determine whether the maze has been built
	private bool setArea = false;					// to determine whether the end sections have been built

	// Floats and Integers
	// *******************
	private int randCellVert = 0;
	private int randCellHor = 0;
	private int randStart = 0;
	private int randEnd = 0;


	// =======================
	// ==	FUNCTION CALLS	==
	// =======================

	public void StartUp() {
		Debug.Log ("FormMaze initializing...");
	}


	void Start () {
		// Initialize Variables
		// ********************
		_grid = GetComponent<FormGrid> ();
		randCellHor = Random.Range (1, (FormGrid.SIZE));
		randCellVert = Random.Range (1, (FormGrid.SIZE));
		randStart = Random.Range (1, FormGrid.SIZE);
		randEnd = Random.Range (1, FormGrid.SIZE);
	}
		

	// MAZE CONSTRUCTOR
	// ****************
	// handles the function calls that check
	// neighbors, and select which neighbor to
	// continue to. Algorithm allows for the maze
	// to be built in a random formation each
	// execution
	public void MazeConstructor (FormGrid.Cell[,] grid) {
		FormGrid.Cell _cell;

		// initialize Stack with random starting point
		if (gridStack.Count < 1 && !mazeStart) {
			gridStack.Push (grid [randCellHor, randCellVert]);
			mazeStart = true;
		}

		// after initialization, check the neighbors of
		// the cell on top of the stack. 
		if (gridStack.Count > 0) {
			_cell = gridStack.Peek ();
			CheckNeighbor (grid, _cell);

			// If there is no neighbor, pop the stack.
			if (neighborList.Count < 1) {
				gridStack.Pop ();

			// If there is a neighbor, pass the grid
			// and the current cell to SelectNeighbor.
			} else {
				SelectNeighbor (grid, _cell);
			}
				
			// clear the list of the neighbors
			// for the current cell
			neighborList.Clear ();
		} else if (!setArea) {
			// build the end sections
			_grid.EndConstructor (randStart, randEnd);

			// destroy the walls of the end sections
			Destroy (grid [randStart, 0].eastWall);
			Destroy (grid [randEnd, FormGrid.SIZE].eastWall);

			// continue game to next state
			Debug.Log ("Form Maze finished...");
			status = ManagerStatus.Started;
			setArea = true;
		}
	}


	// CHECK NEIGHBOR
	// **************
	// function is passed the array and the cell
	// to be checked. It calls CheckVisited to see
	// if the neighbor of the cell has been visited.
	// The function then determines whether to add
	// it to the list.
	void CheckNeighbor (FormGrid.Cell[,] grid, FormGrid.Cell _cell) {
		bool checkW = CheckVisited (grid, (_cell.row - 1), _cell.col);
		bool checkE = CheckVisited (grid, (_cell.row + 1), _cell.col);
		bool checkS = CheckVisited(grid, _cell.row, (_cell.col - 1));
		bool checkN = CheckVisited(grid, _cell.row, (_cell.col + 1));

		// check W neighbor
		if (!checkW) {
			neighborList.Add (grid [(_cell.row - 1), _cell.col]);
		}

		// check E neighbor
		if (!checkE) {
			neighborList.Add (grid [(_cell.row + 1), _cell.col]);
		}

		// check S neighbor
		if (!checkS) {
			neighborList.Add (grid [_cell.row, (_cell.col - 1)]);
		}

		// check N neighbor
		if (!checkN) {
			neighborList.Add (grid [_cell.row, (_cell.col + 1)]);
		}

		//Debug.Log (neighborList.Count);
	}


	// CHECK VISITED
	// *************
	// function tests if the given row/col
	// of the array have been visited. The
	// function then returns true/false
	// depending on the results of the test.
	private bool CheckVisited (FormGrid.Cell[,] grid, int row, int col) {
		bool visited = false;

		// test to see if the N / E walls are present
		if (grid[row, col].northWall != null && grid[row, col].eastWall != null) {

			// test to see if the S walls are missing
			if (grid [(row - 1), col].northWall == null) {
				visited = true;

			// test to see if the W walls are missing
			} else if (grid [row, (col - 1)].eastWall == null) {
				visited = true;
			}

		} else {
			visited = true;
		}

		return visited;
	}


	// SELECT NEIGHBOR
	// ***************
	// function picks a random cell from
	// the neighborList. It then determines
	// which wall to destroy for the previous
	// cell to access the new cell. The function
	// then pushes the new cell on top of the stack.
	void SelectNeighbor (FormGrid.Cell[,] grid, FormGrid.Cell _cell) {
		int randNeighbor = Random.Range (0, neighborList.Count);

		FormGrid.Cell _nextCell = neighborList [randNeighbor];

		// if North neighbor
		if (_nextCell.row > _cell.row) {
			Destroy (grid [_cell.row, _cell.col].northWall);

		// if South neighbor
		} else if (_nextCell.row < _cell.row) {
			Destroy (grid [(_cell.row - 1), _cell.col].northWall);

		// if East Neighbor
		} else if (_nextCell.col > _cell.col) {
			Destroy (grid [_cell.row, _cell.col].eastWall);

		// if West Neighbor
		} else if (_nextCell.col < _cell.col) {
			Destroy (grid [_cell.row, (_cell.col - 1)].eastWall);
		}

		gridStack.Push (_nextCell);
	}
}
