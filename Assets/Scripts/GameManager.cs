﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public PartOfTheDay[] dayParts;
	public DifficultyLevel[] difficulties;

	public RoadRepository roadRepository;

	public PlacementManager placementManager;
	public PotholeManager potholeManager;

	public UiController uiController; 
	public UIManager uiManager;
	public CarSpawnManager carSpawnManager;

	public int oneHourTime;
	public int startHour = 12;

	public float potholeSpawnTime = 10f;

	int difficulty;
	int vehicleNumber;
	int workerNumber;

	public TimeManager timeManager;

	void Awake()
	{
		SetDifficultyParameters();
	}

	void Start()
	{
		Time.timeScale = 1;

		//Initialize
		timeManager = new TimeManager(startHour, oneHourTime);
		potholeManager = new PotholeManager(2, 9, roadRepository, placementManager);

		uiManager.SetUIStart(workerNumber, vehicleNumber);
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

	public void EndGame()
	{
		Time.timeScale = 0;

		int seconds = Mathf.FloorToInt(timeManager.GetTimeSinceStart());

		uiManager.EndGame(seconds);
	}

	public Vector3 GetDifficultyNumbers()
	{
		return new Vector3(difficulty, workerNumber, vehicleNumber);
	}

	private void SetDifficultyParameters()
	{
		difficulty = PlayerPrefs.GetInt("difficulty", 2);

		foreach (DifficultyLevel diff in difficulties)
		{
			if (difficulty == diff.level)
			{
				vehicleNumber = diff.vehicleNumber;
				workerNumber = diff.workerNumber;

				break;
			}
		}
	}

}
