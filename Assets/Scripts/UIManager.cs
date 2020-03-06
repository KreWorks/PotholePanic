using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
	public Image[] workers;

	public TMP_Text potholeTodoText;
	public TMP_Text potholeInprogressText;
	public TMP_Text potholeDoneText;

	public TMP_Text endGameText;

	public Button jobsButton;
	public Button toggleAudioButton;
	public Button exitButton;

	public Color activeColor;
	public Color inaktivColor;

	public GameObject AssignTab;
	public GameObject PotholeStatusTab;
	public GameObject OverViewTab;
	public GameObject EndGameTab;

	public AudioSource backgroundMusic;
	public Sprite AudioOn;
	public Sprite AudioOff;

	int workerAvailableCount;
	int workerWorkingCount;

	int potholeTodoCount;
	int potholeInprogressCount;
	int potholeDoneCount;

	bool needsUpdate = false;

	void Update()
	{
		if (needsUpdate)
		{
			UpdateStatNumbers();
			needsUpdate = false;
		}
	}


	public void UpdateStatNumbers()
	{
		potholeTodoText.text = potholeTodoCount.ToString();
		potholeInprogressText.text = potholeInprogressCount.ToString();
		potholeDoneText.text = potholeDoneCount.ToString();

		/*for(int i =0; i < workers.Length; i++)
		{
			if(workers[i].color.a != 0)
			{
				if(i < workerAvailableCount)
				{
					workers[i].color = activeColor; 
				}
				else
				{
					workers[i].color = inaktivColor;
				}
			}

			if(i >= workerAvailableCount + workerWorkingCount)
			{
				workers[i].color = new Color (workers[i].color.r, workers[i].color.g, workers[i].color.b, 0);
			}
		}*/
	}

	public void AddPothole()
	{
		potholeTodoCount++;
		jobsButton.image.color = activeColor;
		needsUpdate = true;
	}

	public void OpenAssignTab(GameObject selectedHole)
	{
		AssignTab.SetActive(true);

		AssignWorker aw = AssignTab.GetComponent<AssignWorker>();
		aw.SetParams(workerAvailableCount, selectedHole);
	}

	public void OverviewTab()
	{
		if(jobsButton.image.color == activeColor)
		{
			/*PotholeManager pm = FindObjectOfType<PotholeManager>();

			pm.ShowPothole();*/
		}
		else
		{
			OverViewTab.SetActive(true);
		}
	}

	public void AssignWorkersToPothole(int numberOfWorkers)
	{
		if (workerAvailableCount >= numberOfWorkers)
		{
			potholeTodoCount--;
			potholeInprogressCount++;

			workerAvailableCount -= numberOfWorkers;
			workerWorkingCount += numberOfWorkers;

			needsUpdate = true;
		}

		if (potholeTodoCount == 0)
		{
			jobsButton.image.color = inaktivColor;
		}
	}

	public void RepairPothole(int numberOfWorkers)
	{
		potholeInprogressCount--;
		potholeDoneCount++;

		workerAvailableCount += numberOfWorkers;
		workerWorkingCount -= numberOfWorkers;

		needsUpdate = true;
	}

	public void PotholeStatus(Pothole hole)
	{
		PotholeStatusTab.SetActive(true);

		PotholeStatusManager psm = PotholeStatusTab.GetComponent<PotholeStatusManager>();

		psm.SetParams(hole);
	}

	public void ReturnToMenu()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
	}
	
	public void EndGame(int seconds)
	{
		EndGameTab.SetActive(true);

		string timeText = "";
		if(seconds > 60)
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

		endGameText.text = "You have managed the Pothole Panic for " + timeText + ".";
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public Vector3 GetWorkerCount()
	{
		return new Vector3(0, workerAvailableCount, workerWorkingCount);
	}

	public Vector3 GetPotholeCount()
	{
		return new Vector3(potholeTodoCount, potholeInprogressCount, potholeDoneCount);
	}

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
	//Handle sound event
}
