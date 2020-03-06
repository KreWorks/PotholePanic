using UnityEngine;

public class GridStructure
{
	int cellSize; 
	Cell[,] grid;
	int gridSize; 

	public int CellSize { get => cellSize; }
	public int GridSize { get => gridSize; }

	public GridStructure(int cellSize, int gridSize)
	{
		this.cellSize = cellSize;
		this.gridSize = gridSize;

		grid = new Cell[gridSize, gridSize];

		for (int row = 0; row < grid.GetLength(0); row++)
		{
			for (int col = 0; col < grid.GetLength(1); col++)
			{
				grid[row, col] = new Cell();
			}
		}
	}

	public int GetGridLength(int dimension)
	{
		return grid.GetLength(dimension);
	}
	
	public void PlaceRoadToGrid(int x, int y, GameObject roadObejct, bool isPothole, bool firstSet = false)
	{
		if (firstSet)
		{
			grid[x, y].SetRoadModel(roadObejct);
		}
		grid[x,y].SetRoadObject(roadObejct, isPothole);
	}

	public GameObject GetRoadObjectFromGird(int x, int y)
	{
		return grid[x, y].GetRoadObject();
	}

	public RoadType GetRoadType(int i, int j)
	{
		RoadType roadType = RoadType.NotRoad; 

		// Select each road type
		if (IsCornerRoadCell(i, j))
		{
			roadType = RoadType.Corner;
		}
		else if (IsThreeWayRoadCell(i, j))
		{
			roadType = RoadType.ThreeWay;
		}
		else if (IsFourWayRoadCell(i, j))
		{
			roadType = RoadType.FourWay;
		}
		else if (IsStraightRoadCell(i, j))
		{
			roadType = RoadType.Straight;
		}

		return roadType;
	}

	public bool CanSpawnPothole(int x, int y)
	{
		return IsStraightRoadCell(x, y) && !grid[x, y].HasPothole;
	}

	public Quaternion GetRotation(int x, int y)
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
		if (IsStraightRoadCell(x, y))
		{
			int random = x + y;
			random = Mathf.FloorToInt(Mathf.Pow(-1, random));
			if (random == -1)
			{
				rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y + 180, rotation.eulerAngles.z);
			}
		}

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

public enum RoadType
{
	NotRoad,
	Straight, 
	Corner, 
	ThreeWay,
	FourWay
}