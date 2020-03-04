using UnityEngine;
using UnityEngine.UI;

public class AssignWorker : MonoBehaviour
{
	public Image[] holes;

	public Image[] workers;

	public Color activeColor;
	public Color inactiveColor;

	public Button AssignButton;

	int setWorkers;
	protected GameObject holeObject;

	Color btnBaseColor;
	bool canAssignWorkers = true;

	public void SetParams(int workerCount, GameObject hole)
	{
		this.holeObject = hole;
		PotholeSize holeType = hole.GetComponent<Pothole>().size;
	
		setWorkers = 0;

		switch(workerCount)
		{
			case 0: 
				workers[0].color = new Color(workers[0].color.r, workers[0].color.g, workers[0].color.b, 0);
				workers[1].color = new Color(workers[1].color.r, workers[1].color.g, workers[1].color.b, 0);
				workers[2].color = new Color(workers[2].color.r, workers[2].color.g, workers[2].color.b, 0);
				break;
			case 1:
				workers[1].color = new Color(workers[1].color.r, workers[1].color.g, workers[1].color.b, 0);
				workers[2].color = new Color(workers[2].color.r, workers[2].color.g, workers[2].color.b, 0);
				break;
			case 2:
				workers[2].color = new Color(workers[2].color.r, workers[2].color.g, workers[2].color.b, 0);
				break;
			default:
				workers[0].color = inactiveColor;
				workers[1].color = inactiveColor;
				workers[2].color = inactiveColor;
				break;
		}

		//Set t active the hole size
		holes[0].color = inactiveColor;
		holes[1].color = inactiveColor;
		holes[2].color = inactiveColor;
		switch (holeType)
		{
			case PotholeSize.Small:
				holes[0].color = activeColor;
				break;
			case PotholeSize.Medium:
				holes[1].color = activeColor;
				break;
			case PotholeSize.Big:
				holes[2].color = activeColor;
				break;
		}

		TMPro.TMP_Text text = AssignButton.GetComponentInChildren<TMPro.TMP_Text>();
		if (workerCount == 0)
		{
			btnBaseColor = AssignButton.image.color;
			AssignButton.image.color = activeColor;
			canAssignWorkers = false;
		}
		else
		{
			AssignButton.image.color = activeColor;
			canAssignWorkers = true;
		}
	}

	public void ChangeAssignColor(int buttonIndex)
	{
		if (workers[buttonIndex].color == activeColor)
		{
			workers[buttonIndex].color = inactiveColor;
			if (buttonIndex < 2 && workers[buttonIndex + 1].color == activeColor)
			{
				ChangeAssignColor(buttonIndex + 1);
			}
			setWorkers--;
		}else 
		{
			workers[buttonIndex].color = activeColor;
			setWorkers++;
			if(buttonIndex > 0 && workers[buttonIndex - 1].color == inactiveColor)
			{
				ChangeAssignColor(buttonIndex - 1);
			}
		}
	}

	public void AssignWorkersSubmit()
	{
		UIManager uiManager = FindObjectOfType<UIManager>();
		if (canAssignWorkers)
		{
			Pothole hole = holeObject.GetComponent<Pothole>();

			hole.StartSolvePothole(this.setWorkers);
			
			uiManager.AssignWorkersToPothole(this.setWorkers);
		}
		else
		{
			uiManager.AssignTab.SetActive(false);
		}
	}
}
