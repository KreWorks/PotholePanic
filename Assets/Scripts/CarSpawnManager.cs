using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawnManager : MonoBehaviour
{
	public float carSpawnTime = 5.0f;
	public int endOfGameCarCount = 5;
	public GameObject carObject;
	public GameManager gameManager;

	List<CarSpawner> carSpawners;

	void Start()
	{
		carSpawners = new List<CarSpawner>();
		gameManager = FindObjectOfType<GameManager>();
	}

	public void AddSpawner(CarSpawner cs)
	{
		carSpawners.Add(cs);
	}

	public bool RemoveSpawner(CarSpawner cs)
	{
		return carSpawners.Remove(cs);
	}

	void Update()
	{
		if(carSpawners.Count > 0)
		{
			foreach (CarSpawner carSpawner in carSpawners)
			{
				carSpawner.TimeGoBy(Time.deltaTime, gameManager.GetPartOfTheDayMultiplier());

				if (carSpawner.time > carSpawnTime)
				{
					Quaternion direction = Quaternion.Euler(0, carSpawner.direction.eulerAngles.y, 0);
					Vector3 position = carSpawner.position + GetCarTranslateValue(carSpawner.carCount, carSpawner.parent);

					GameObject car = Instantiate(carObject, position, direction);
					car.transform.parent = carSpawner.parent.transform;

					carSpawner.AddCar(carSpawnTime);
				}

				if (carSpawner.carCount >= endOfGameCarCount)
				{
					GameManager gm = FindObjectOfType<GameManager>();

					gm.EndGame();
				}
			}
		}
	}

	Vector3 GetCarTranslateValue(int numberOfCars, Transform pothole)
	{
		Vector3 translateVector = new Vector3(0, 0, 0);

		if (pothole.GetComponent<Pothole>().size == PotholeSize.Small)
		{
			translateVector -= 0.4f * pothole.transform.right;
		}
		else
		{
			translateVector += 0.4f * pothole.transform.right;
		}

		translateVector += pothole.transform.up * (numberOfCars * 0.8f + 0.6f);

		return translateVector;
	}
}
