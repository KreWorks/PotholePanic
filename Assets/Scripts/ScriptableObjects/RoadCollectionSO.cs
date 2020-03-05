using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Road Collection", menuName = "RoadCollection")]
public class RoadCollectionSO : ScriptableObject
{
	public RoadStraightSO straightRoadPrefab;
	public RoadCornerSO cornerRoadPrefab;
	public RoadThreewaySO threeWayRoadPrefab;
	public RoadFourwaySO fourWayRoadPrefab;
}
