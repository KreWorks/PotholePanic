using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoadBaseSo : ScriptableObject
{
	public string roadName;
	public GameObject prefab;

	public int traffic;
	public bool canHasPothole;
}
