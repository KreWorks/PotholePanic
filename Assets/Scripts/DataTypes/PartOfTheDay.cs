using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PartOfTheDay 
{
	public float multiplier;
	public int startHour;
	public int endHour;

	public PartOfTheDay(int startH, int endH, float multiplier)
	{
		this.startHour = startH;
		this.endHour = endH;
		this.multiplier = multiplier;
	}

	public bool IsValidMultiplier(int hour)
	{
		return (hour >= startHour && hour < endHour);
	}
}
