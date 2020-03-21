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
		HandleCarSpawn();

		gameManager.CheckEndGame(carCount);
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

	void HandleCarSpawn()
	{
		if(carSpawnTime <= (timeSinceSpawn - carCount * carSpawnTime))
		{
			SpawnCar();
		}
	}

	void SpawnCar()
	{
		carCount++;
		Vector3 position = this.transform.position + GetCarTranslateValue();
		Quaternion direction = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y, 0);

		potholeManager.PlaceCarNextPothole(position, direction, this.transform);
	}

	Vector3 GetCarTranslateValue()
	{
		Vector3 translateVector = new Vector3(0, 0, 0);

		if (size == PotholeSize.Small)
		{
			translateVector -= 0.35f * transform.right;
			translateVector -= 0.1f * transform.up;
		}
		else
		{
			translateVector += 0.35f * transform.right;
			translateVector += 0.2f * transform.up;
		}

		translateVector += transform.up * (carCount * 0.7f);

		return translateVector;
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
