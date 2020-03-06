using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PotholeStatus { ToDo, InProgress, Done}
public enum PotholeSize { Small = 0, Medium  = 1, Big = 2}


public class Pothole : Road
{
	public PotholeStatus status;
	public PotholeSize size;

	public float repairTime;
	public float progress = 0.0f;
	public int assignedWorkers;

	public Vector3 position;

	public CarSpawner ownCarSpawner;

	// Update is called once per frame
	void Update()
	{
		SolvePothole();
	}

	public void SetPothole(float repairTime, int traffic, int size, Vector3 pos)
	{
		this.status = PotholeStatus.ToDo;
		this.repairTime = repairTime;
		this.traffic = traffic;
		this.size = size == 0 ? PotholeSize.Small : (size == 1 ? PotholeSize.Medium : PotholeSize.Big);
		this.position = pos;
	}

	public void SetCarSpawner(CarSpawner carSpawner)
	{ 
		this.ownCarSpawner = carSpawner;
	}

	public void StartSolvePothole(int workerCount)
	{
		this.status = PotholeStatus.InProgress;
		this.assignedWorkers = workerCount;
	}

	void SolvePothole()
	{
		if (this.status == PotholeStatus.InProgress)
		{
			progress += ((assignedWorkers + 1.0f) / 2.0f) * Time.deltaTime;
			if (progress >= repairTime)
			{
				FinishPothole();
			}
		}
	}

	void FinishPothole()
	{
		this.status = PotholeStatus.Done;

		/*PotholeManager phManager = FindObjectOfType<PotholeManager>();

		phManager.FinishPothole(this.gameObject);*/
	}
}
