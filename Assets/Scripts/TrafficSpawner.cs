using System.Collections;
using UnityEngine;

public class TrafficSpawner : MonoBehaviour
{
	public GameObject carParent;
	public GameObject[] carPrefabs;
	public int carsToSpawn;

	public float movementSpeed = 1.2f;
	public int rotationSpeed = 180;
	public float stopDistance = 0.5f;

	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(Spawn());
	}

	void Update()
	{
		if (carsToSpawn < carParent.transform.childCount)
		{
			int randomIndex = Random.Range(0, transform.childCount);
			SpawnTraffic(transform.GetChild(randomIndex), carParent.transform.childCount);
		}
	}

	IEnumerator Spawn()
	{
		int count = 0;
		while (count < carsToSpawn && count < transform.childCount)
		{
			SpawnTraffic(transform.GetChild(count), count);		

			yield return new WaitForEndOfFrame();

			count++;
		}
	}

	void SpawnTraffic(Transform positionTransform, int id)
	{
		int randomCarIndex = Random.Range(0, carPrefabs.Length);

		GameObject obj = Instantiate(carPrefabs[randomCarIndex]);
		obj.transform.name = carPrefabs[randomCarIndex].name + " " + id;
		obj.transform.parent = carParent.transform;

		Transform child = positionTransform;
		int direction = Mathf.RoundToInt(Random.Range(0f, 1f));

		obj.AddComponent<CharacterNavigationController>();
		obj.GetComponent<CharacterNavigationController>().Reset(movementSpeed, rotationSpeed, stopDistance);
		obj.AddComponent<WaypointNavigator>();
		obj.GetComponent<WaypointNavigator>().SetParams(child.GetComponent<Waypoint>(), direction);
	}
}