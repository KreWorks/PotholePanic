using System;
using UnityEngine;

public enum PotholeStatus { ToDo, InProgress, Done}
public enum PotholeSize { Small = 0, Medium  = 1, Big = 2}

public class Pothole : MonoBehaviour
{
	public PotholeStatus status;
	public PotholeSize size;

	public GameManager gameManager;
	public PotholeManager potholeManager;
	public SliderController sliderController;

	public float repairTime;
	public float progress = 0.0f;
	public int assignedWorkers;

	float timeSinceSpawn;
	float carSpawnTime;
	int carCount;

	private Action OnPotholeDestruction;

	public void SetPothole(PotholeManager potholeManager, float repairTime, PotholeSize potholeSize)
	{
		this.gameManager = FindObjectOfType<GameManager>();
		this.carSpawnTime = gameManager.carSpawnTime;
		this.potholeManager = potholeManager;
		this.repairTime = repairTime;
		this.size = potholeSize;
		this.timeSinceSpawn = 0.0f;
		this.carCount = 0;
	}

	void Update()
	{
		TimeGoBy();
		SolvePothole();
	}

	public Vector3 GetPotholePositionOnRoad()
	{
		Vector3 position = this.transform.position;

		if (size == PotholeSize.Small)
		{
			position -= this.transform.right * 0.5f;
		}
		else
		{
			position += this.transform.right * 0.5f;
		}
		
		return position;
	}

	public void AddCarToPothole()
	{
		carCount++;
		gameManager.CheckEndGame(carCount);
	}

	void TimeGoBy()
	{
		//multiplier based on the time of day
		float multiplier = gameManager.GetPartOfTheDayMultiplier();

		timeSinceSpawn = timeSinceSpawn + Time.deltaTime * multiplier;
	}

	void SolvePothole()
	{
		if (this.status == PotholeStatus.InProgress)
		{
			progress += ((assignedWorkers + 1.0f) / 2.0f) * Time.deltaTime;
			sliderController.SetProgressTime(progress);

			if (progress >= repairTime)
			{
				FinishPothole();
			}
		}
	}

	public void StartSolvePothole(int workerCount)
	{
		this.status = PotholeStatus.InProgress;
		this.assignedWorkers = workerCount;

		potholeManager.StartRepairPothole(this, workerCount);
		sliderController = GetComponentInChildren<SliderController>();
	}
	

	void FinishPothole()
	{
		this.status = PotholeStatus.Done;
		OnPotholeDestruction?.Invoke();

		potholeManager.FinishPothole(this, this.assignedWorkers);
	}

	public void AddListenerOnPotholeDestructionEvent(Action listener)
	{
		OnPotholeDestruction += listener;
	}
	public void RemoveListenerOnPotholeDestructionEvent(Action listener)
	{
		OnPotholeDestruction -= listener;
	}
}
