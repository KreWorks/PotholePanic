using UnityEngine;
using UnityEditor;

[InitializeOnLoad()]
public class WaypointEditor 
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
	public static void OnDrawSceneGizmo(Waypoint waypoint, GizmoType gizmoType)
	{
		if ((gizmoType & GizmoType.Selected) != 0)
		{
			Gizmos.color = Color.yellow;
		}
		else
		{
			Gizmos.color = Color.yellow * 0.5f;
		}

		Gizmos.DrawSphere(waypoint.transform.position, .1f);

		Gizmos.color = Color.white;
		Gizmos.DrawLine(waypoint.transform.position + (waypoint.transform.right * waypoint.width / 2f), 
			waypoint.transform.position - (waypoint.transform.right * waypoint.width / 2f));

		if(waypoint.previousWaypoint != null)
		{
			foreach (Waypoint previousWaypoint in waypoint.previousWaypoint)
			{
				DrawLine(Color.red, waypoint.transform, previousWaypoint.transform, waypoint.width, previousWaypoint.width, -1);
			}
		}

		if(waypoint.nextWaypoint != null)
		{
			foreach(Waypoint nextWaypoint in waypoint.nextWaypoint)
			{
				DrawLine(Color.green, waypoint.transform, nextWaypoint.transform, waypoint.width, nextWaypoint.width, 1);
			}
		}
	}

	static void DrawLine(Color color, Transform waypointFrom, Transform waypointTo, float fromWidth, float toWidth, int direction)
	{
		Gizmos.color = color;

		Vector3 offset = waypointFrom.right * direction * fromWidth / 2f;
		Vector3 offsetTo = waypointTo.right * direction * toWidth / 2f;

		Gizmos.DrawLine(waypointFrom.position + offset, waypointTo.position + offsetTo);
	}
}
