using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiController : MonoBehaviour
{
	[Header("Timer variables")]
	public TMP_Text timeText;
	public Image dayTimeIcon;

	public Sprite sun;
	public Sprite moon;

	public Color sunColor;
	public Color moonColor;

	[Header("Pothole count variables")]
	public TMP_Text potholeTodoCount;
	public TMP_Text potholeInProgressCount;
	public TMP_Text potholeDoneCount;

	[Header("Worker variables")]
	public GameObject worker;
	public GameObject workerIconPrefab;

	public Color active;
	public Color inactive;

	UIHelper uiHelper; 

	void Awake()
	{
		uiHelper = new UIHelper(active, inactive);
		ChangePotholeCount(new Vector3(0, 0, 0));
	}

	public void InitWorkerIcons(int workerCount)
	{
		uiHelper.InitWorkerIcons(workerCount, workerIconPrefab, worker);
	}

	public void ChangeWorkerColor(int availableWorker)
	{
		Image[] icons = worker.GetComponentsInChildren<Image>();
		uiHelper.ChangeWorkerColor(availableWorker, icons);
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


	public void ChangePotholeCount(Vector3 potholeCounts)
	{
		uiHelper.ChangePotholeCount(potholeCounts, potholeTodoCount, potholeInProgressCount, potholeDoneCount);
	}

}
