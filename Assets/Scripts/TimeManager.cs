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

		return TimeFormatter(hour, minute);
	}

	public string GetTimeText()
	{
		string timeText = "";
		int seconds = Mathf.FloorToInt(timeSpent);

		if (seconds > 60)
		{
			timeText += Mathf.FloorToInt(seconds / 60.0f).ToString();
			timeText += ":";
			int secs = Mathf.FloorToInt(seconds % 60);
			timeText += secs < 10 ? "0" + secs.ToString() : secs.ToString();
		}
		else
		{
			timeText += seconds.ToString() + " seconds";
		}

		return timeText;
	}

	public string TimeFormatter(int hour, int minute)
	{
		string timeText = hour < 10 ? "0" + hour.ToString() : hour.ToString();
		timeText += ":";
		timeText += minute < 10 ? "0" + minute.ToString() : minute.ToString();

		return timeText;
	}
}
