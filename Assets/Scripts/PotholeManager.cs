using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PotholeManager 
{
	GridStructure grid;
	RoadRepository roadRepository;
	PlacementManager placementManager;

	private Action<Vector3> OnPotholeCountChangeEvent;

	int gridSize;

	int potholeCount = 0;
	Vector3Int potholeStatusCounter = new Vector3Int(0, 0, 0);

	public float potholeSpawnTime = 20f;
	public float carSpawnTime = 15f;
	public PotholeType[] holeTypes;
	public GameObject roadObject;

	public CarSpawnManager carSpawnerManager;
	public UIManager uiManager;
	public GameManager gameManager;

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
		PotholeType potholeType = GetRandomSizedPotholePrefab();

		GameObject potholeObject = placementManager.CreateRoadObject(index.x, index.y, grid, potholeType.prefab);
		potholeObject.transform.tag = "Pothole";

		Pothole newPothole = potholeObject.AddComponent<Pothole>();
		newPothole.SetPothole(this, potholeType.repairTime, potholeType.GetSize());
		
		//Remove road from grid
		GameObject road = grid.GetRoadObjectFromGird(index.x, index.y);
		placementManager.RemoveObject(road);

		//Set new pothole to grid
		grid.PlaceRoadToGrid(index.x, index.y, potholeObject, true);

		potholeCount++;
		potholeStatusCounter.x++;
		OnPotholeCountChangeEvent?.Invoke(potholeStatusCounter);

		return potholeObject;

		//TODO car spawner
		//TODO UI
		//TODO add pothole component with parameters (solve time, status, etc)
	}

	public void PlaceCarNextPothole(Vector3 position, Quaternion rotation, Transform pothole)
	{
		placementManager.SpawnCar(position, rotation, pothole);
	}

	Vector2Int GetRandomRoadIndex()
	{
		Vector2Int index = new Vector2Int(Random(0, gridSize), Random(0, gridSize));

		while(!grid.CanSpawnPothole(index.x, index.y))
		{
			index = new Vector2Int(Random(0, gridSize), Random(0, gridSize));
		}

		return index;
	}

	PotholeType GetRandomSizedPotholePrefab()
	{
		int potholeIndex = Random(0, roadRepository.roadModelCollection.straightRoadPrefab.potholes.Length);

		return roadRepository.roadModelCollection.straightRoadPrefab.potholes[potholeIndex];
	}

	int Random(int min, int max)
	{
		return UnityEngine.Random.Range(min,max);
	}

	public void AddListenerOnPotholeCountChangeEvent(Action<Vector3> listener)
	{
		OnPotholeCountChangeEvent += listener;
	}
	public void RemoveListenerOnPotholeCountChangeEvent(Action<Vector3> listener)
	{
		OnPotholeCountChangeEvent -= listener;
	}

	public void FinishPothole(GameObject pothole)
	{	
		//Initializing road tile
		GameObject road = GameObject.Instantiate(roadObject, pothole.transform.position, pothole.transform.rotation);
		road.transform.parent = pothole.transform.parent;
		road.AddComponent<Road>();
		//road.GetComponent<Road>() = pothole.GetComponent<Pothole>();

		//Remove the carSpawner from CarSpawnerManager
		//carSpawnerManager.RemoveSpawner(pothole.GetComponent<Pothole>().ownCarSpawner);
		int workers = pothole.GetComponent<Pothole>().assignedWorkers;
		//Remove hole 
		holes.Remove(pothole.GetComponent<Pothole>());
		GameObject.Destroy(pothole);

		//UI modifications (numbers and colors)
		uiManager.RepairPothole(workers);
	}

	int GetRandomRoadIndex2()
	{
		//int index = Random(0, roads.Count);

		return 0; // index; 
	}

	int GetRandomPotholeTypeIndex()
	{
		return Random(0, holeTypes.Length);
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
			//cc.MoveCameraToSpecificPosition(holeToShow.position);
		}

	}
}

