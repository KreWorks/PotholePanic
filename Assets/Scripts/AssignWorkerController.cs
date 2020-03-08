using UnityEngine;
using UnityEngine.UI;

public class AssignWorkerController : MonoBehaviour
{
	public Color activeColor;
	public Color inactiveColor;

	public GameObject workersPanel;
	public GameObject potholeTypesPanel;

	UIHelper uiHelper;
	Pothole pothole = null; 

	void Awake()
	{
		uiHelper = new UIHelper(activeColor, inactiveColor);
	}

	public void SetParams(Pothole pothole)
    {
		this.pothole = pothole;
		PotholeSize holeType = pothole.size;

		Image[] holes = potholeTypesPanel.GetComponentsInChildren<Image>();
		uiHelper.SetPotholeTypeIcon(holes, holeType);

		Image[] workers = workersPanel.GetComponentsInChildren<Image>();
		uiHelper.ChangeWorkerColor(0, workers);

		//TODO need to subscribe for some event to update availableWorkers
	}

	public void SelectWorkers(int count)
	{
		Image[] workers = workersPanel.GetComponentsInChildren<Image>();

		uiHelper.ChangeWorkerColor(count, workers);
	}

	public void AssignWorker()
	{
		GameManager gameManager = FindObjectOfType<GameManager>();


	}

	int CountSelectedWorkers()
	{
		Image[] workerIcons = workersPanel.GetComponentsInChildren<Image>();
		int counter = 0; 

		foreach(Image icon in workerIcons)
		{
			if (icon.color == activeColor)
			{
				counter++; 
			}
		}

		return counter;
	}
}
