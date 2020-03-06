using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiController : MonoBehaviour
{
	public GameObject timer;

	public Sprite sun;
	public Sprite moon;

	public Color sunColor;
	public Color moonColor;

	TMP_Text timeText;
	Image dayTimeIcon;

	void Start()
	{
		SetTimeUiElements();
	}

	public void ChangePicto(int hour)
	{
		if (hour >= 22 || hour < 4 && dayTimeIcon.color != moonColor)
		{
			dayTimeIcon.sprite = moon;
			dayTimeIcon.color = moonColor;
		}
		else if(dayTimeIcon.color != sunColor)
		{
			dayTimeIcon.sprite = sun;
			dayTimeIcon.color = sunColor;
		}
	}

	public void ChangeTimeText(string time)
	{
		timeText.text = time;
	}

	private void SetTimeUiElements()
	{
		timeText = timer.GetComponentInChildren<TextMeshProUGUI>();
		dayTimeIcon = timer.GetComponentsInChildren<Image>()[1];
	}
}
