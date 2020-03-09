using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
	public Transform cityRoads;

	public GameObject potholeIndicatorPrefab;
	public GameObject repairParticlePrefab;
	public GameObject sliderIcon;
	public GameObject carPrefab;
	

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

	public GameObject CreateRepairParticleSystem(GameObject potholeObject)
	{
		GameObject repairParticle = Instantiate(repairParticlePrefab, potholeObject.transform);

		return repairParticle;
	}

	public GameObject AddIndicatorToPothole(GameObject pothole)
	{
		GameObject indicator = Instantiate(potholeIndicatorPrefab, pothole.transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity, pothole.transform);

		MeshRenderer[] parts = indicator.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer mr in parts)
		{
			mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		}

		return indicator;
	}

	public GameObject AddSliderIconToPothole(GameObject pothole)
	{
		GameObject sliderIconObject = Instantiate(sliderIcon, pothole.transform.position + new Vector3(0, 2f, 0), Quaternion.identity, pothole.transform);

		sliderIconObject.GetComponent<SliderController>().cam = Camera.main.transform;

		return sliderIconObject;
	}

	public GameObject AddParticleSystemToPothole(GameObject pothole)
	{
		GameObject repairParticleObject = Instantiate(repairParticlePrefab, pothole.transform);

		return repairParticleObject;
	}

	public GameObject SpawnCar(Vector3 position, Quaternion rotation, Transform pothole)
	{
		return Instantiate(carPrefab,position, rotation, pothole);
	}

	public void RemoveObject(GameObject roadToRemove)
	{
		Destroy(roadToRemove);
	}

}
