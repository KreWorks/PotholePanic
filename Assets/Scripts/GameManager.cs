using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
	public PartOfTheDay[] dayParts;
	public DifficultyLevel[] difficulties;

	private Action<int> OnWorkerCountInitEvent;

	public RoadRepository roadRepository;
	public GameObject roads; 

	public PlacementManager placementManager;
	public PotholeManager potholeManager;
	public TimeManager timeManager;

	public UiController uiController; 

	public int oneHourTime;
	public int startHour = 12;

	public float potholeSpawnTime = 10f;
	public float carSpawnTime = 8f;
	public int endGameCarCount = 5;

	int difficulty;
	int workerNumber;

	void Awake()
	{
		//Set params
		SetWorkerCountByDifficulty();
		//Initialize
		timeManager = new TimeManager(startHour, oneHourTime);
		potholeManager = new PotholeManager(2, 9, roadRepository, placementManager, workerNumber);
	}

	void Start()
	{
		Time.timeScale = 1;
		GridStructure grid = new GridStructure(2, 9);

		//Subscribers
		//Worker count init
		AddListenerOnWorkerCountInitEvent((workerCount) => uiController.InitWorkerIcons(workerCount));
		AddListenerOnWorkerCountInitEvent((workerCount) => uiController.overviewController.InitWorkerIcons(workerCount));
		//Available worker count change
		potholeManager.AddListenerOnAvailableWorkerChangeEvent((workerCount) => uiController.ChangeWorkerColor(workerCount));
		potholeManager.AddListenerOnAvailableWorkerChangeEvent((workerCount) => uiController.overviewController.ChangeWorkerColor(workerCount));
		potholeManager.AddListenerOnAvailableWorkerChangeEvent((workerCount) => uiController.assignWorkerController.DisableWorkers(workerCount));
		//Pothole counter change
		potholeManager.AddListenerOnPotholeCountChangeEvent((potholeCount) => uiController.ChangePotholeCount(potholeCount));
		potholeManager.AddListenerOnPotholeCountChangeEvent((potholeCount) => uiController.overviewController.ChangePotholeCount(potholeCount));

		OnWorkerCountInitEvent?.Invoke(workerNumber);
	}

	void Update()
	{
		//Managing the time and the time UI
		timeManager.TimePass(Time.deltaTime);
		uiController.ChangeTimeText(timeManager.GenerateIngameTimeText());
		uiController.ChangePicto(timeManager.GetHour());

		//Managing pothole spawning
		if (potholeManager.TimeSinceLastPothole(timeManager.GetTimeSinceStart(), potholeSpawnTime) >= potholeSpawnTime)
		{
			potholeManager.SpawnPothole();
		}
	}

	public float GetPartOfTheDayMultiplier()
	{
		int hour = timeManager.GetHour();
		foreach(PartOfTheDay dayTime in dayParts)
		{
			if (dayTime.IsValidMultiplier(hour))
			{
				return dayTime.multiplier;
			}
		}

		return 1.0f;
	}

	public void CheckEndGame(int carCount)
	{
		if (carCount >= endGameCarCount)
		{
			EndGame();
		}
	}

	public void EndGame()
	{
		Time.timeScale = 0;

		int seconds = Mathf.FloorToInt(timeManager.GetTimeSinceStart());

		uiController.EndGame(timeManager.GetTimeText());
	}

	private void SetWorkerCountByDifficulty()
	{
		this.difficulty = PlayerPrefs.GetInt("game_difficulty", 2);

		foreach (DifficultyLevel diff in difficulties)
		{
			if (difficulty == diff.level)
			{
				workerNumber = diff.workerNumber;
			}
		}
	}

	public void AddListenerOnWorkerCountInitEvent(Action<int> listener)
	{
		OnWorkerCountInitEvent += listener;
	}
	public void RemoveListenerOnWorkerCountInitEvent(Action<int> listener)
	{
		OnWorkerCountInitEvent -= listener;
	}

}
