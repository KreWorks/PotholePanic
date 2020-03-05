using UnityEngine;

public class Cell 
{
	GameObject road = null;
	bool hasPothole  = false; 

	public bool HasPothole { get => hasPothole; }

	public void SetRoadModel(GameObject road)
	{
		this.road = road; 
	}

	public void SpawnPothole(GameObject pothole)
	{
		if (this.road != null)
		{
			this.road = pothole;
			this.hasPothole = true;
		}
	}

	public void RepairPothole(GameObject road)
	{
		this.road = road;
		this.hasPothole = false;
	}

	public bool IsRoad()
	{
		return road != null;
	}
}
