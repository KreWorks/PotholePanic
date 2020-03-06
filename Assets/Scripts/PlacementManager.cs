using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
	public Transform cityRoads;

	//public RoadRepository roadRepository;

	public void GenerateCityRoads(GridStructure grid, RoadRepository roadRepository)
	{
		for(int i = 0; i < grid.GetGridLength(0); i++)
		{
			for(int j = 0; j < grid.GetGridLength(1); j++)
			{
				GameObject roadPrefab = GetRoadTypePrefab(grid.GetRoadType(i, j), roadRepository);

				if (roadPrefab != null)
				{
					GameObject roadObject = CreateRoadObject(i, j, grid, roadPrefab);

					//Set object to the grid
					grid.PlaceRoadToGrid(i, j, roadObject, false, true);
				}
			}
		}
	}

	private GameObject GetRoadTypePrefab(RoadType roadType, RoadRepository roadRepository)
	{
		switch (roadType)
		{
			case RoadType.Straight:
				return roadRepository.roadModelCollection.straightRoadPrefab.prefab;
			case RoadType.Corner:
				return roadRepository.roadModelCollection.cornerRoadPrefab.prefab;
			case RoadType.ThreeWay:
				return  roadRepository.roadModelCollection.threeWayRoadPrefab.prefab;
			case RoadType.FourWay:
				return roadRepository.roadModelCollection.fourWayRoadPrefab.prefab;
			default:
				return null;
		}
	}

	public GameObject CreateRoadObject(int i, int j, GridStructure grid, GameObject roadPrefab)
	{
		Vector3 position = new Vector3(i * grid.CellSize, 0, j * grid.CellSize) - ((grid.GridSize - 1) / 2.0f) * new Vector3(grid.CellSize, 0, grid.CellSize);
		Quaternion rotation = grid.GetRotation(i, j);

		GameObject roadObject = Instantiate(roadPrefab, position, rotation, cityRoads);
		roadObject.transform.name = roadPrefab.name + "(" + i + ", " + j + ")";
		
		return roadObject;
	}

	public void RemoveObject(GameObject roadToRemove)
	{
		Destroy(roadToRemove);
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
