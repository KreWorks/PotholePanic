using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
	/**
	 * Based on the tutorial of Game Dev Guide
	 * https://www.youtube.com/watch?v=MXCZ-n5VyJc
	 */
	
	public List<Waypoint> previousWaypoint;
	public List<bool> previousWaypointDirection;
	public List<Waypoint> nextWaypoint;
	public List<bool> nextWaypointDirection;
	
	
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
			return transform.position + transform.right * width / 3.0f;
		}
		else
		{
			return transform.position - transform.right * width / 3.0f;
		}
	}

	public WaypointWithDirection GetNextWaypoint(Waypoint lastWaypoint)
	{
		return GetNextInList(nextWaypoint, nextWaypointDirection, lastWaypoint);
	}

	public WaypointWithDirection GetPreviousWaypoint(Waypoint lastWaypoint)
	{
		return GetNextInList(previousWaypoint, previousWaypointDirection, lastWaypoint);
	}

	WaypointWithDirection GetNextInList(List<Waypoint> list, List<bool> listWithDirection, Waypoint lastWaypoint)
	{
		WaypointWithDirection returnValue;

		if (list.Count == 1)
		{
			returnValue.waypoint = list[0];
			returnValue.goingForward = listWithDirection[0];
		}
		else
		{
			int index = -1;

			while (index == -1 || list[index] == lastWaypoint)
			{
				index = Mathf.FloorToInt(Random.Range(0, list.Count));
			}

			returnValue.waypoint = list[index];
			returnValue.goingForward = listWithDirection[index];
		}

		return returnValue;
	}
}

public struct WaypointWithDirection
{
	public Waypoint waypoint;
	public bool goingForward;
}
