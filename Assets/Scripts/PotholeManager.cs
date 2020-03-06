using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotholeManager 
{
	GridStructure grid;
	RoadRepository roadRepository;
	PlacementManager placementManager; 

	int gridSize;

	int potholeCount = 0;
	Vector3Int potholeStatusCounter = new Vector3Int(0, 0, 0);

	public float potholeSpawnTime = 20f;
	public PotholeType[] holeTypes;
	public GameObject roadObject;

	public CarSpawnManager carSpawnerManager;
	public UIManager uiManager;
	public GameManager gameManager;

	List<Road> roads;
	List<Pothole> holes;

	public PotholeManager(int cellSize, int gridSize, RoadRepository roadRepository, PlacementManager placementManager)
	{
		//Set up parameters
		this.grid = new GridStructure(cellSize, gridSize);
		this.roadRepository = roadRepository;
		this.gridSize = gridSize;
		this.placementManager = placementManager;

		this.potholeCount = 0;
		this.potholeStatusCounter = new Vector3Int(0, 0, 0);

		//Generate the city's road system
		this.placementManager.GenerateCityRoads(grid, roadRepository);
	}

	public float TimeSinceLastPothole(float time, float potholeSpawnTime)
	{
		return time - potholeCount * potholeSpawnTime;
	}

	public GameObject SpawnPothole()
	{
		Vector2Int index = GetRandomRoadIndex();
		GameObject potholePrefab = GetRandomSizedPotholePrefab();
		GameObject potholeObject = placementManager.CreateRoadObject(index.x, index.y, grid, potholePrefab);

		//Remove road from grid
		GameObject road = grid.GetRoadObjectFromGird(index.x, index.y);
		placementManager.RemoveObject(road);

		//Set new pothole to grid
		grid.PlaceRoadToGrid(index.x, index.y, potholeObject, true);

		potholeCount++;
		potholeStatusCounter.x++;

		return potholeObject;

		//TODO car spawner, UI
	}


	Vector2Int GetRandomRoadIndex()
	{
		Vector2Int index = new Vector2Int(Random.Range(0, gridSize), Random.Range(0, gridSize));

		while(!grid.CanSpawnPothole(index.x, index.y))
		{
			index = new Vector2Int(Random.Range(0, gridSize), Random.Range(0, gridSize));
		}

		return index;
	}

	GameObject GetRandomSizedPotholePrefab()
	{
		int potholeIndex = Random.Range( 0, roadRepository.roadModelCollection.straightRoadPrefab.potholes.Length);

		return roadRepository.roadModelCollection.straightRoadPrefab.potholes[potholeIndex].prefab;
	}

	public void FinishPothole(GameObject pothole)
	{	
		//Initializing road tile
		GameObject road = GameObject.Instantiate(roadObject, pothole.transform.position, pothole.transform.rotation);
		road.transform.parent = pothole.transform.parent;
		road.AddComponent<Road>();
		road.GetComponent<Road>().traffic = pothole.GetComponent<Pothole>().traffic;

		//Remove the carSpawner from CarSpawnerManager
		carSpawnerManager.RemoveSpawner(pothole.GetComponent<Pothole>().ownCarSpawner);
		int workers = pothole.GetComponent<Pothole>().assignedWorkers;
		//Remove hole 
		holes.Remove(pothole.GetComponent<Pothole>());
		GameObject.Destroy(pothole);

		//UI modifications (numbers and colors)
		uiManager.RepairPothole(workers);
	}

	int GetRandomRoadIndex2()
	{
		int index = Random.Range(0, roads.Count);

		return index; 
	}

	int GetRandomPotholeTypeIndex()
	{
		return Random.Range(0, holeTypes.Length);
	}

	public List<Pothole> GetHoles()
	{
		return this.holes;
	}

	Pothole GetTodoPothole()
	{
		foreach(Pothole hole in holes)
		{
			if(hole.status == PotholeStatus.ToDo)
			{
				return hole;
			}
		}

		return null;
	}

	public void ShowPothole()
	{
		Pothole holeToShow = GetTodoPothole();

		if(holeToShow != null)
		{
			CameraController cc = GameObject.FindObjectOfType<CameraController>();
			cc.MoveCameraToSpecificPosition(holeToShow.position);
		}

	}
}

