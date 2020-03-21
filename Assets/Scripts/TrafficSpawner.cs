using System.Collections;
using UnityEngine;

public class TrafficSpawner : MonoBehaviour
{
	public GameObject carPrefab;
	public int carsToSpawn;

	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(Spawn());
	}

	IEnumerator Spawn()
	{
		int count = 0;
		while (count < carsToSpawn)
		{
			GameObject obj = Instantiate(carPrefab);
			obj.transform.name = carPrefab.name + " " + count;

			Transform child = transform.GetChild(Random.Range(0, transform.childCount - 1));
			int direction = Mathf.RoundToInt(Random.Range(0f, 1f));

			obj.GetComponent<WaypointNavigator>().SetParams(child.GetComponent<Waypoint>(), direction);			

			yield return new WaitForEndOfFrame();

			count++;
		}
	}
}