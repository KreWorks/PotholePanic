using UnityEngine;

public class TimeManager 
{
	public float timeSpent;

	public int startHour;
	public int inGameHourSeconds;

	public TimeManager(int startHour, int seconds)
	{
		this.timeSpent = 0;
		this.startHour = startHour;
		this.inGameHourSeconds = seconds;
	}

	public float TimePass(float time)
	{
		this.timeSpent += time;

		return this.timeSpent;
	}

	public float GetTimeSinceStart()
	{
		return this.timeSpent;
	}

	public int GetHour()
	{
		return this.startHour + Mathf.FloorToInt(timeSpent / (float)inGameHourSeconds);
	}

	public int GetMinute()
	{
		return Mathf.FloorToInt((timeSpent % (float)inGameHourSeconds) * (60.0f / (float)inGameHourSeconds));
	}

	public string GenerateIngameTimeText()
	{
		int hour = GetHour();
		int minute = GetMinute(); 

		string timeText = hour < 10 ? "0" + hour.ToString() : hour.ToString();
		timeText += ":";
		timeText += minute < 10 ? "0" + minute.ToString() : minute.ToString();

		return timeText;
	}
}
