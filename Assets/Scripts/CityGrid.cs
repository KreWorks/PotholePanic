using UnityEngine;

public class CityGrid
{
	int cellSize; 
	Cell[,] grid;
	int gridSize;

	RoadRepository roadRepository;

	public CityGrid(int cellSize, int gridSize, RoadRepository roadRepository, GameObject roadParent)
	{
		this.cellSize = cellSize;
		this.gridSize = gridSize;
		this.roadRepository = roadRepository;

		grid = new Cell[gridSize, gridSize];

		for (int row = 0; row < grid.GetLength(0); row++)
		{
			for (int col = 0; col < grid.GetLength(1); col++)
			{
				grid[row, col] = SetRoadToGrid(row, col, roadParent); 
			}
		}
	}

	public Cell SetRoadToGrid(int i, int j, GameObject parent)
	{
		Vector3 position = new Vector3(i * cellSize, 0, j * cellSize) - ((gridSize - 1) / 2.0f) * new Vector3(cellSize, 0, cellSize);
		Quaternion rotation = GetRotation(i, j); 

		Cell gridCell = new Cell();
		GameObject prefab = null; 
		// Select each road type
		if (IsCornerRoadCell(i, j))
		{
			prefab = roadRepository.roadModelCollection.cornerRoadPrefab.prefab;
		}
		else if (IsThreeWayRoadCell(i, j))
		{
			prefab = roadRepository.roadModelCollection.threeWayRoadPrefab.prefab; 
		}
		else if (IsFourWayRoadCell(i, j))
		{
			prefab = roadRepository.roadModelCollection.fourWayRoadPrefab.prefab;
		}
		else if (IsStraightRoadCell(i, j))
		{
			prefab = roadRepository.roadModelCollection.straightRoadPrefab.prefab;
		}
		
		if(prefab != null) { 
			GameObject road = GameObject.Instantiate(prefab, position, rotation);
			road.transform.parent = parent.transform;
			road.transform.name = "Road (" + i + ", " + j + ")";
			gridCell.SetRoadModel(road);
		}
				
		return gridCell;
	}

	private Quaternion GetRotation(int x, int y)
	{
		Quaternion rotation = Quaternion.Euler(-90.0f, 0, 0);

		//Corners and Three ways
		if ((x == 0 && y == 0) || (y == 0 && x != 8))
		{
			rotation = Quaternion.Euler(-90.0f, 90.0f, 0);
		}
		else if((x == 0 && y == 8) || (x == 0 && y != 0))
		{
			rotation = Quaternion.Euler(-90.0f, 180.0f, 0);
		}
		else if((x ==8 && y == 0) || (x == 8 && y != 8))
		{
			rotation = Quaternion.Euler(-90.0f, 0, 0);
		}
		else if ((x == 8 && y == 8) || (y == 8 && x != 0))
		{
			rotation = Quaternion.Euler(-90.0f, -90.0f, 0);
		}
		//Straight roads
		else if (y == 3 || y == 5)
		{
			rotation = Quaternion.Euler(-90.0f, 90.0f, 0);
		}
		//Randomness to spawn pothole in different lane
		/*if (IsStraightRoadCell(x, y))
		{
			int random = x + y;
			random = Mathf.FloorToInt(Mathf.Pow(-1, random));
			rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y + 180 * random, rotation.eulerAngles.z);
			Debug.Log("x: " + x + " y: " + y + " random: " + random + " angle: " + rotation.eulerAngles.y + 180 * random);
		}*/

		return rotation; 
	}

	private bool OnRoadGrid(int x)
	{
		return (x == 0 || x == 3 || x == 5 || x == 8);
	}

	private bool IsCornerRoadCell(int x, int y)
	{
		return ((x == 0 || x == 8) && (y == 0 || y == 8));
	}

	private bool IsStraightRoadCell(int x, int y)
	{
		return (OnRoadGrid(x) && !OnRoadGrid(y)) || (!OnRoadGrid(x) && OnRoadGrid(y));
	}

	private bool IsThreeWayRoadCell(int x, int y)
	{
		return ((x == 3 || x == 5) && (y == 0 || y == 8) || (x == 0 || x == 8) && (y == 3 || y == 5));
	}

	private bool IsFourWayRoadCell(int x, int y)
	{
		return ((x == 3 || x == 5) && (y == 3 || y == 5));
	}
}
