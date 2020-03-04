using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Timer : MonoBehaviour
{
	public GameObject sun;
	public GameObject moon;

	public TMPro.TMP_Text timer;

	GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
		gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
		int hour = gm.GetHour();
		int min = gm.GetMinute();

		while (hour >= 24)
		{
			hour = hour - 24;
		}

		string timeText = GenerateTimerText(hour, min);

		timer.text = timeText;
		ChangePicto(hour);
    }

	string GenerateTimerText(int hour, int min)
	{
		string timeText;

		timeText = hour < 10 ? "0" + hour.ToString() : hour.ToString();
		timeText += ":";
		timeText += min < 10 ? "0" + min.ToString() : min.ToString();

		return timeText;
	}

	void ChangePicto(int hour)
	{
		if(hour > 22 || hour < 4 )
		{
			sun.SetActive(false);
			moon.SetActive(true); 
		}
		else if (moon.activeSelf)
		{
			moon.SetActive(false);
			sun.SetActive(true);
		}
	}
}
