using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotholeManager : MonoBehaviour
{
	public GameObject roadEnvironment;
	public float potholeSpawnTime = 20f;
	public PotholeType[] holeTypes;
	public GameObject roadObject;
	
	public Timer timer;

	public CarSpawnManager carSpawnerManager;
	public UIManager uiManager;
	public GameManager gameManager;

	List<Road> roads;
	List<Pothole> holes;
	GameObject[] roadGrid;

    // Start is called before the first frame update
    void Start()
    {
		holes = new List<Pothole>();
		roads = new List<Road>();

		roadEnvironment.GetComponentsInChildren<Road>(roads);

		roadGrid = new GameObject[roads.Count];
    }

    // Update is called once per frame
    void Update()
    {
		//Time since the last pothole spawn
		float potholeTime = gameManager.GetTime() - potholeSpawnTime * holes.Count;

		if (potholeTime > potholeSpawnTime)
		{
			SpawnHole();
		}
	}

	void SpawnHole()
	{
		//random numbers for the pothole
		int index = GetRandomRoadIndex();
		int sizeIndex = GetRandomPotholeTypeIndex();

		//Helper for destroy and data access
		Road roadObject = roads[index];

		//creating the object
		GameObject potholeObj = Instantiate(holeTypes[sizeIndex].holePrefab,roadObject.transform.position, roadObject.transform.rotation);
		
		potholeObj.transform.parent = roadObject.transform.parent;
		potholeObj.AddComponent<Pothole>();
		potholeObj.GetComponent<Pothole>().SetPothole(holeTypes[sizeIndex].repairTime, roads[index].traffic, sizeIndex, roadObject.transform.position);
		potholeObj.transform.gameObject.tag = "Pothole";

		//Destroy the road object
		Destroy(roads[index].gameObject);
		//We need to use the object createed before, because the index now has another roadtile
		roads.Remove(roadObject);
		
		//Add carSpawner to the spawnManager
		CarSpawner carSpawner = new CarSpawner(potholeObj.transform, potholeObj.GetComponent<Pothole>().traffic);
		carSpawnerManager.AddSpawner(carSpawner);
		potholeObj.GetComponent<Pothole>().SetCarSpawner(carSpawner);

		//Add hole to the list
		holes.Add(potholeObj.GetComponent<Pothole>());

		//UI modifications (numbers and colors)
		uiManager.AddPothole();
	}

	public void FinishPothole(GameObject pothole)
	{	
		//Initializing road tile
		GameObject road = Instantiate(roadObject, pothole.transform.position, pothole.transform.rotation);
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

	int GetRandomRoadIndex()
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
			CameraController cc = FindObjectOfType<CameraController>();
			cc.MoveCameraToSpecificPosition(holeToShow.position);
		}

	}
}

