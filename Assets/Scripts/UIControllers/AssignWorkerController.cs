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
	}

	public void SelectWorkers(int count)
	{
		Image[] workers = workersPanel.GetComponentsInChildren<Image>();

		uiHelper.ChangeWorkerColor(count, workers);
	}

	public void DisableWorkers(int count)
	{
		if (count <= 3)
		{
			Image[] workers = workersPanel.GetComponentsInChildren<Image>(true);
			int index = 0; 
			foreach(Image workerIcon in workers)
			{
				if (index < count)
				{
					workerIcon.gameObject.SetActive(true);
					workerIcon.color = inactiveColor;
				}
				else
				{
					workerIcon.gameObject.SetActive(false);
				}
				index++;
			}
		}
	}

	public void AssignWorker()
	{
		int workerCount = CountSelectedWorkers();

		if(workerCount > 0)
		{
			pothole.StartSolvePothole(workerCount);
		}
		
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
