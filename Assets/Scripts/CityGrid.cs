using UnityEngine;

public class CityGrid
{
	int cellSize; 
	Cell[,] grid;
	int width, length;

	public CityGrid(int cellSize, int width, int length)
	{
		this.cellSize = cellSize;
		this.width = width;
		this.length = length;

		grid = new Cell[width, length];

		for (int row = 0; row < grid.GetLength(0); row++)
		{
			for (int col = 0; col < grid.GetLength(1); col++)
			{
				grid[row, col] = new Cell();
			}
		}
	}

	public void SetRoadsToGrid(GameObject road)
	{
	}

	public Vector3 CalculateGridPosition(Vector3 inputposition)
	{
		int x = Mathf.FloorToInt((float)inputposition.x / cellSize);
		int z = Mathf.FloorToInt((float)inputposition.z / cellSize);

		return new Vector3(x * cellSize, 0, z * cellSize);
	}

	private Vector2Int CalculateGridIndex(Vector3 gridPosition)
	{
		return new Vector2Int((int)(gridPosition.x / cellSize), (int)(gridPosition.z / cellSize));
	}

	private bool CheckIndexValidity(Vector2Int cellIndex)
	{
		return (cellIndex.x >= 0 && cellIndex.x < grid.GetLength(1) && cellIndex.y >= 0 && cellIndex.y < grid.GetLength(0));
	}
}
