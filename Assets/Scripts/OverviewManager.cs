using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverviewManager : MonoBehaviour
{
	public Image[] workers;

	public Color activeColor;
	public Color inactiveColor;

	public TMP_Text hole_done;
	public TMP_Text hole_inprogress;
	public TMP_Text hole_todo;

	public GameManager gameManager;
	public UIManager uiManager;

	int workerNum;

    // Start is called before the first frame update
    void Start()
	{
		//difficulty, workerNumber, vehicleNumber
		Vector3 dn = gameManager.GetDifficultyNumbers();

		workerNum = (int)dn.y;
		
		hole_done.text = "0";
		hole_inprogress.text = "0";
		hole_todo.text = "0";

		for (int i  = 0; i < workers.Length; i++)
		{
			if (i < dn.y)
			{
				workers[i].color = activeColor;
			}
			else
			{
				workers[i].color = new Color(workers[i].color.r, workers[i].color.g, workers[i].color.b, 0); ;
			}

		}
    }

    // Update is called once per frame
    void Update()
    {
		//carAvailableCount, workerAvailableCount, workerWorkingCount
		Vector3 workerCount = uiManager.GetWorkerCount();
		//(potholeTodoCount, potholeInprogressCount, potholeDoneCount
		Vector3 potholeCount = uiManager.GetPotholeCount();

		hole_done.text = potholeCount.z.ToString();
		hole_inprogress.text = potholeCount.y.ToString();
		hole_todo.text = potholeCount.x.ToString();

		for (int i = 0; i < workerNum; i++)
		{
			if (i < workerCount.y)
			{
				workers[i].color = activeColor;
			}
			else
			{
				workers[i].color = inactiveColor;
			}
			if (i >= workerCount.y + workerCount.z)
			{
				workers[i].color = new Color(workers[i].color.r, workers[i].color.g, workers[i].color.b, 0);
			}
		}
	}
}
