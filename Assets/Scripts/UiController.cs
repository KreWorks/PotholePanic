using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

	[Header("Panels")]
	public GameObject overviewPanel;
	public GameObject statusPanel;
	public GameObject assignPanel;
	public GameObject endGamePanel;

	[Header("Other Controllers")]
	public AssignWorkerController assignWorkerController;
	public PotholeStatusController potholeStatusController;
	public OverviewController overviewController;

	[Header("Rest")]
	public TMP_Text endGameText;

	UIHelper uiHelper; 

	void Awake()
	{
		uiHelper = new UIHelper(active, inactive);
		ChangePotholeCount(new Vector3(0, 0, 0));
	}

	void Start()
	{
		assignPanel.SetActive(false);
		statusPanel.SetActive(false);
		overviewPanel.SetActive(false);
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

	public void EndGame(string timeText)
	{
		endGamePanel.SetActive(true);

		endGameText.text = "You have managed the Pothole Panic for " + timeText + ".";
	}

	public void OpenAssignPanel(Pothole hole)
	{
		assignPanel.SetActive(true);

		assignWorkerController.SetParams(hole);
	}

	public void OpenStatusPanel(Pothole hole)
	{
		statusPanel.SetActive(true);
		potholeStatusController.SetParams(hole);
	}

	public void ReturnToMenu()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
	/*
	public void ToggleAudio()
	{
		if (backgroundMusic.volume == 0.6f)
		{
			backgroundMusic.volume = 0;
			toggleAudioButton.image.sprite = AudioOff;
		}
		else
		{
			backgroundMusic.volume = 0.6f;
			toggleAudioButton.image.sprite = AudioOn;
		}

	}
	*/
}
