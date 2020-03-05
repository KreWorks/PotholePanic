using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public PartOfTheDay[] dayParts;
	public DifficultyLevel[] difficulties;

	public UIManager uiManager;
	public PotholeManager potholeManager;

	public RoadRepository roadRepository;

	public int oneHourTime;
	public int startHour = 12;

	int difficulty;
	int vehicleNumber;
	int workerNumber;

	CityGrid grid; 

	float time; 

	void Start()
	{
		Time.timeScale = 1;
		difficulty = PlayerPrefs.GetInt("difficulty", 2);

		foreach(DifficultyLevel diff in difficulties)
		{
			if (difficulty == diff.level)
			{
				vehicleNumber = diff.vehicleNumber;
				workerNumber = diff.workerNumber;

				break;
			}
		}

		grid = new CityGrid(2, 9, roadRepository, this.potholeManager.roadEnvironment);

		uiManager.SetUIStart(workerNumber, vehicleNumber);
	}

	void Update()
	{
		this.time += Time.deltaTime;
	}

	public float GetTime()
	{
		return time;
	}

	public int GetHour()
	{
		return startHour + Mathf.FloorToInt(time / (float)oneHourTime);
	}

	public int GetMinute()
	{
		return Mathf.FloorToInt((time % (float)oneHourTime) * (60.0f / (float)oneHourTime)); 
	}

	public float GetPartOfTheDayMultiplier()
	{
		int hour = GetHour();
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

		int seconds = Mathf.FloorToInt(time );

		uiManager.EndGame(seconds);
	}

	public Vector3 GetDifficultyNumbers()
	{
		return new Vector3(difficulty, workerNumber, vehicleNumber);
	}

}
