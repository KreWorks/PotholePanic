using UnityEngine;

[System.Serializable]
public class CarSpawner
{
	public Transform parent;
	public Vector3 position;
	public Quaternion direction;
	public int traffic;
	public float time;
	public int carCount;

	public CarSpawner(Transform par, int traf)
	{
		parent = par;
		position = par.transform.position;
		direction = par.transform.rotation;
		traffic = traf;
		time = 0.0f;
		carCount = 0;
	}

	public void TimeGoBy(float deltaTime, float multiplier)
	{
		//TODO include traffic and partofthe day
		this.time = this.time + deltaTime * multiplier * Mathf.Sqrt(traffic);
	}

	public void AddCar(float spawnTime)
	{
		carCount++;
		time -= spawnTime;
	}
}
