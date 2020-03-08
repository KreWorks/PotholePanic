using UnityEngine;

[System.Serializable]
public struct PotholeType 
{
	public GameObject prefab;
	public float repairTime;

	public PotholeSize GetSize()
	{
		if (prefab.name.Contains("small"))
		{
			return PotholeSize.Small; 
		}
		else if (prefab.name.Contains("big"))
		{
			return PotholeSize.Big;
		}
		else
		{
			return PotholeSize.Medium;
		}
	}
}
