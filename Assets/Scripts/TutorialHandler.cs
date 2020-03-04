using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialHandler : MonoBehaviour
{
	public GameObject[] elementToShow;
	public GameObject assignTab;

	bool needsUpdate = false;
	int stepindex;
    void Start()
    {
		stepindex = 0;
		ShowElement();
    }


    void Update()
    {
        if (needsUpdate)
		{
			ShowElement();
			if (stepindex == 2)
			{
				assignTab.SetActive(true);
			}
			else
			{
				assignTab.SetActive(false);
			}
			needsUpdate = false;
		}
    }

	public void ShowElement()
	{
		for (int i = 0; i < elementToShow.Length; i++)
		{
			if (stepindex == i)
			{
				elementToShow[i].SetActive(true);
			}
			else
			{
				elementToShow[i].SetActive(false);
			}
		}
	}

	public void NextStep()
	{
		stepindex++;
		needsUpdate = true;
	}

	public void StartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
