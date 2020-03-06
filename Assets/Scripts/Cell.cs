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

	public void SetRoadObject(GameObject roadObject, bool isPothole)
	{
		if (IsRoad())
		{
			this.road = roadObject;
			this.hasPothole = isPothole;
		}
	}

	public GameObject GetRoadObject()
	{
		return road;
	}

	public bool IsRoad()
	{
		return road != null;
	}
}
