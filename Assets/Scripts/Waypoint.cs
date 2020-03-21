using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
	/**
	 * Based on the tutorial of Game Dev Guide
	 * https://www.youtube.com/watch?v=MXCZ-n5VyJc
	 */
	public List<Waypoint> previousWaypoint;
	public List<Waypoint> nextWaypoint;

	[Range(0f, 2f)]
	public float width = 1.2f;

	public Waypoint()
	{
		previousWaypoint = new List<Waypoint>();
		nextWaypoint = new List<Waypoint>();
	}

	public Vector3 GetPosition(bool goingForward = true)
	{
		if (goingForward)
		{
			return transform.position + transform.right * width / 4.0f;
		}
		else
		{
			return transform.position - transform.right * width / 4.0f;
		}
	}

	public Waypoint GetNextWaypoint()
	{
		return GetNextInList(nextWaypoint);
	}

	public Waypoint GetPreviousWaypoint()
	{
		return GetNextInList(previousWaypoint);
	}

	Waypoint GetNextInList(List<Waypoint> list)
	{
		if (list.Count == 1)
		{
			return list[0];
		}
		else
		{
			return list[Mathf.FloorToInt(Random.Range(0, list.Count))];
		}
	}
}
