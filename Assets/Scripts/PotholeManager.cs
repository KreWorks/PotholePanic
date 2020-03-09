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
	private Action<int> OnAvailableWorkerChangeEvent;

	int gridSize;

	int potholeCount = 0;
	Vector3Int potholeStatusCounter = new Vector3Int(0, 0, 0);

	int workerCount;
	int availableWorkerCount;

	public PotholeManager(int cellSize, int gridSize, RoadRepository roadRepository, PlacementManager placementManager, int workerCount)
	{
		//Set up parameters
		this.grid = new GridStructure(cellSize, gridSize);
		this.roadRepository = roadRepository;
		this.gridSize = gridSize;
		this.placementManager = placementManager;

		this.potholeCount = 0;
		this.potholeStatusCounter = new Vector3Int(0, 0, 0);

		this.workerCount = workerCount;
		this.availableWorkerCount = workerCount;

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
		placementManager.AddIndicatorToPothole(potholeObject);
		potholeObject.transform.tag = "Pothole";
		potholeObject.name = "Pothole (" + index.x + ", " + index.y + ")";

		Pothole newPothole = potholeObject.AddComponent<Pothole>();
		newPothole.SetPothole(this, potholeType.repairTime, potholeType.GetSize());
		
		//Remove road from grid
		GameObject road = grid.GetRoadObjectFromGird(index.x, index.y);
		placementManager.RemoveObject(road);

		//Set new pothole to grid
		grid.PlaceRoadToGrid(index.x, index.y, potholeObject, true);

		//Change numbers
		potholeCount++;
		ChangePotholeCount(new Vector3Int(1, 0, 0));

		return potholeObject;
	}

	public void StartRepairPothole(Pothole pothole, int workerCount)
	{
		Vector2Int gridIndex = grid.GetGridIndexByPosition(pothole.transform.position);
		GameObject gridObject = grid.GetRoadObjectFromGird(gridIndex.x, gridIndex.y);

		if (gridObject.GetComponent<Pothole>() == pothole)
		{
			//Remove indicator
			GameObject indicator = pothole.gameObject.GetComponentInChildren<IconController>().gameObject;
			placementManager.RemoveObject(indicator);

			//Add canvas and particle system
			GameObject canvas = placementManager.AddSliderIconToPothole(pothole.gameObject);
			canvas.GetComponent<SliderController>().SetSolveTime(pothole.repairTime);

			GameObject repairParticle = placementManager.AddParticleSystemToPothole(pothole.gameObject);

			//Change numbers
			ChangePotholeCount(new Vector3Int(-1, 1, 0));
			ChangeAvailableWorker(-workerCount);
		}
	}

	public void FinishPothole(Pothole pothole, int workerCount)
	{
		Vector2Int gridIndex = grid.GetGridIndexByPosition(pothole.transform.position);
		GameObject potholeObject = grid.GetRoadObjectFromGird(gridIndex.x, gridIndex.y);

		if (potholeObject.GetComponent<Pothole>() == pothole)
		{
			//Create road
			GameObject roadObject = placementManager.CreateRoadObject(gridIndex.x, gridIndex.y, grid, roadRepository.roadModelCollection.straightRoadPrefab.prefab);

			//Remove Pothole from grid
			placementManager.RemoveObject(potholeObject);

			//Set the road to the grid
			grid.PlaceRoadToGrid(gridIndex.x, gridIndex.y, roadObject, false);

			//Change numbers
			ChangePotholeCount(new Vector3Int(0, -1, 1));
			ChangeAvailableWorker(workerCount);
		}
	}

	void ChangePotholeCount(Vector3Int change)
	{
		potholeStatusCounter = potholeStatusCounter + change;

		OnPotholeCountChangeEvent?.Invoke(potholeStatusCounter);
	}

	void ChangeAvailableWorker(int change)
	{
		availableWorkerCount += change;
		OnAvailableWorkerChangeEvent?.Invoke(availableWorkerCount);
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

	public void AddListenerOnAvailableWorkerChangeEvent(Action<int> listener)
	{
		OnAvailableWorkerChangeEvent += listener;
	}
	public void RemoveListenerOnAvailableWorkerChangeEvent(Action<int> listener)
	{
		OnAvailableWorkerChangeEvent -= listener;
	}

	/*Pothole GetTodoPothole()
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

	}*/
}

