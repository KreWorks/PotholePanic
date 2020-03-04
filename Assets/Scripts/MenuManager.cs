using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	public Image[] workers;

	public Color activeColor; // #f7dd66;
	public Color inactiveColor; //c7c7c7

	int difficulty = 2;

	public void Start()
	{
		for (int i = 0; i < workers.Length; i++)
		{
			if (i < 5)
			{
				workers[i].color = activeColor;
			}
			else
			{
				workers[i].color = inactiveColor;
			}
		}
	}

	

	public void QuitButton()
	{
		Application.Quit();
	}
	
	public void SetDifficulty(int selected)
	{
		difficulty = selected;
		int endIndex = selected == 1 ? 8 : selected == 2 ? 5 : 3;

		for(int i =0;i < workers.Length; i++)
		{
			if (i < endIndex)
			{
				workers[i].color = activeColor;
			}
			else
			{
				workers[i].color = inactiveColor;
			}
		}
	}


	public void StartGameButton()
	{
		PlayerPrefs.SetInt("game_difficulty", difficulty);

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);

	}

	public void TutorialButton()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
