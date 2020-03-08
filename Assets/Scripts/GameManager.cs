using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
	public PartOfTheDay[] dayParts;
	public DifficultyLevel[] difficulties;

	private Action<int> OnWorkerCountChangeEvent;

	public RoadRepository roadRepository;

	public PlacementManager placementManager;
	public PotholeManager potholeManager;
	public TimeManager timeManager;

	public UiController uiController; 


	public int oneHourTime;
	public int startHour = 12;

	public float potholeSpawnTime = 10f;
	public int endGameCarCount = 5;

	int difficulty;
	int workerNumber;
	int availableWorkers;

	public int allWorkerCount { get => workerNumber; }

	void Awake()
	{
		SetDifficultyParameters();

		//Initialize
		timeManager = new TimeManager(startHour, oneHourTime);
		potholeManager = new PotholeManager(2, 9, roadRepository, placementManager);

		uiController.InitWorkerIcons(workerNumber);
	}

	void Start()
	{
		Time.timeScale = 1;

		potholeManager.AddListenerOnPotholeCountChangeEvent((potholeCount) => uiController.ChangePotholeCount(potholeCount));
		AddListenerOnWorkerCountChangeEvent((workerCount) => uiController.ChangeWorkerColor(workerCount));
	}

	//Change worker count
	public bool AssignWorkers(int workerCount)
	{
		if (availableWorkers >= workerCount && allWorkerCount >= (availableWorkers - workerCount))
		{
			availableWorkers -= workerCount;
			//Notify others with the change
			OnWorkerCountChangeEvent?.Invoke(availableWorkers);
			return true;
		}
		else
		{
			return false;
		}
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
		if (carCount > endGameCarCount)
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

	public Vector3 GetDifficultyNumbers()
	{
		return new Vector3(difficulty, workerNumber, 0);
	}

	private void SetDifficultyParameters()
	{
		difficulty = PlayerPrefs.GetInt("difficulty", 2);

		foreach (DifficultyLevel diff in difficulties)
		{
			if (difficulty == diff.level)
			{
				workerNumber = diff.workerNumber;
				availableWorkers = workerNumber;

				break;
			}
		}
	}

	public void AddListenerOnWorkerCountChangeEvent(Action<int> listener)
	{
		OnWorkerCountChangeEvent += listener;
	}
	public void RemoveListenerOnWorkerCountChangeEvent(Action<int> listener)
	{
		OnWorkerCountChangeEvent -= listener;
	}

}
