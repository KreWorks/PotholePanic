using System.Collections;
using UnityEngine;

public class TrafficSpawner : MonoBehaviour
{
	public GameObject carParent;
	public GameObject[] carPrefabs;
	public int carsToSpawn;

	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(Spawn());
	}

	IEnumerator Spawn()
	{
		int count = 0;
		while (count < carsToSpawn && count < transform.childCount)
		{
			int randomCarIndex = Random.Range(0, carPrefabs.Length);
			GameObject obj = Instantiate(carPrefabs[randomCarIndex]);
			obj.transform.name = carPrefabs[randomCarIndex].name + " " + count;
			obj.transform.parent = carParent.transform;

			Transform child = transform.GetChild(count);
			int direction = Mathf.RoundToInt(Random.Range(0f, 1f));

			obj.AddComponent<CharacterNavigationController>();
			obj.GetComponent<CharacterNavigationController>().Reset();
			obj.AddComponent<WaypointNavigator>();
			obj.GetComponent<WaypointNavigator>().SetParams(child.GetComponent<Waypoint>(), direction);			

			yield return new WaitForEndOfFrame();

			count++;
		}
	}
}