using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WaypointManagerWindow : EditorWindow
{
	/**
	 * Based on the tutorial of Game Dev Guide
	 * https://www.youtube.com/watch?v=MXCZ-n5VyJc
	 */
	[MenuItem("Tools/Waypoint Editor")]
    public static void Open()
	{
		GetWindow<WaypointManagerWindow>();
	}
	//Parent for our waypoints
	public Transform waypointRoot;

	private void OnGUI()
	{
		SerializedObject obj = new SerializedObject(this);

		EditorGUILayout.PropertyField(obj.FindProperty("waypointRoot"));

		if (waypointRoot == null)
		{
			EditorGUILayout.HelpBox("Root transform must be selected. Please assign a root transform.", MessageType.Warning);
		}
		else
		{
			EditorGUILayout.BeginVertical("box");
			DrawButtons();
			EditorGUILayout.EndVertical();
		}

		obj.ApplyModifiedProperties();
	}

	void DrawButtons()
	{
		if (GUILayout.Button("Creeate Waypoint"))
		{
			CreateWaypoint();
		}

		if(Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Waypoint>())
		{
			if(GUILayout.Button("Create Waypoint Before"))
			{
				CreateWaypointBefore();
			}
			if (GUILayout.Button("Create Waypoint After"))
			{
				CreateWaypointAfter();
			}
			if(GUILayout.Button("Remove Waypoint"))
			{
				RemoveWaypoint();
			}
		}
	}

	void CreateWaypoint()
	{
		GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
		waypointObject.transform.SetParent(waypointRoot, false);

		Waypoint waypoint = waypointObject.GetComponent<Waypoint>();
		if (waypointRoot.childCount > 1)
		{
			waypoint.previousWaypoint.Add(waypointRoot.GetChild(waypointRoot.childCount - 2).GetComponent<Waypoint>());
			int previousWaypointIndex = waypoint.previousWaypoint.Count - 1;
			waypoint.previousWaypoint[previousWaypointIndex].nextWaypoint.Add(waypoint);
			//Place the waypoint at the last position
			waypoint.transform.position = waypoint.previousWaypoint[previousWaypointIndex].transform.position;
			waypoint.transform.forward = waypoint.previousWaypoint[previousWaypointIndex].transform.forward;
		}

		Selection.activeGameObject = waypoint.gameObject;
	}

	void CreateWaypointBefore()
	{
		GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
		waypointObject.transform.SetParent(waypointRoot, false);

		Waypoint newWaypoint = waypointObject.GetComponent<Waypoint>();

		Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

		waypointObject.transform.position = selectedWaypoint.transform.position;
		waypointObject.transform.forward = selectedWaypoint.transform.forward;

		if (selectedWaypoint.previousWaypoint != null)
		{
			for(int i = 0; i < selectedWaypoint.previousWaypoint.Count; i++)
			{
				newWaypoint.previousWaypoint.Add(selectedWaypoint.previousWaypoint[i]);
				selectedWaypoint.previousWaypoint[i].nextWaypoint = new List<Waypoint>();
				selectedWaypoint.previousWaypoint[i].nextWaypoint.Add(newWaypoint);
			}
		}

		newWaypoint.nextWaypoint.Add(selectedWaypoint);

		selectedWaypoint.previousWaypoint.Add(newWaypoint);

		newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());

		Selection.activeGameObject = newWaypoint.gameObject;
	}

	void CreateWaypointAfter()
	{
		GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
		waypointObject.transform.SetParent(waypointRoot, false);

		Waypoint newWaypoint = waypointObject.GetComponent<Waypoint>();

		Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

		waypointObject.transform.position = selectedWaypoint.transform.position;
		waypointObject.transform.forward = selectedWaypoint.transform.forward;

		newWaypoint.previousWaypoint.Add(selectedWaypoint);

		if (selectedWaypoint.nextWaypoint != null)
		{
			for (int i = 0; i < selectedWaypoint.nextWaypoint.Count; i++)
			{
				selectedWaypoint.previousWaypoint[i].nextWaypoint.Add(newWaypoint);
				newWaypoint.nextWaypoint.Add(selectedWaypoint.nextWaypoint[i]);
			}
			
		}

		selectedWaypoint.nextWaypoint.Add(newWaypoint);

		newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex() + 1);

		Selection.activeGameObject = newWaypoint.gameObject;
	}

	void RemoveWaypoint()
	{
		Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

		if (selectedWaypoint.nextWaypoint != null)
		{
			for(int i =0; i < selectedWaypoint.nextWaypoint.Count; i++)
			{
				selectedWaypoint.nextWaypoint[i].previousWaypoint = selectedWaypoint.previousWaypoint;
			}
		}
		if (selectedWaypoint.previousWaypoint != null)
		{
			for(int i = 0; i < selectedWaypoint.previousWaypoint.Count; i++)
			{
				selectedWaypoint.previousWaypoint[i].nextWaypoint = selectedWaypoint.nextWaypoint;
			}
			Selection.activeGameObject = selectedWaypoint.previousWaypoint[0].gameObject;
		}

		DestroyImmediate(selectedWaypoint.gameObject);
	}
}
