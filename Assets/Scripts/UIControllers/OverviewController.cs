using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverviewController : MonoBehaviour
{
	[Header("Pothole count variables")]
	public TMP_Text potholeTodoCount;
	public TMP_Text potholeInProgressCount;
	public TMP_Text potholeDoneCount;

	[Header("Worker variables")]
	public GameObject workers;
	public GameObject workerIconPrefab;

	public Color activeColor;
	public Color inactiveColor;

	UIHelper uiHelper;

	void Awake()
	{
		uiHelper = new UIHelper(activeColor, inactiveColor);
		ChangePotholeCount(new Vector3(0, 0, 0));
	}

	void Start()
	{
		GameManager gameManager = FindObjectOfType<GameManager>();
		if (gameManager != null)
		{
			InitWorkerIcons(gameManager.allWorkerCount);
			gameManager.potholeManager.AddListenerOnPotholeCountChangeEvent((potholeCount) => ChangePotholeCount(potholeCount));
			gameManager.AddListenerOnWorkerCountChangeEvent((workerCount) => ChangeWorkerColor(workerCount));
		}
	}

	public void InitWorkerIcons(int workerCount)
	{
		uiHelper.InitWorkerIcons(workerCount, workerIconPrefab, workers);
	}

	public void ChangeWorkerColor(int availableWorker)
	{
		Image[] icons = workers.GetComponentsInChildren<Image>();
		uiHelper.ChangeWorkerColor(availableWorker, icons);
	}

	public void ChangePotholeCount(Vector3 potholeCounts)
	{
		uiHelper.ChangePotholeCount(potholeCounts, potholeTodoCount, potholeInProgressCount, potholeDoneCount);
	}
}
