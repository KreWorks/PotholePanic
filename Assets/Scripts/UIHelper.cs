using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHelper 
{
	Color activeColor;
	Color inactiveColor;

	public UIHelper(Color aColor, Color iaColor)
	{
		this.activeColor = aColor;
		this.inactiveColor = iaColor;
	}

	public GameObject InitWorkerIcons(int workerCount, GameObject workerIconPrefab, GameObject parent)
	{
		for (int i = 0; i < workerCount; i++)
		{
			GameObject workerIcon = GameObject.Instantiate(workerIconPrefab, parent.transform);
			workerIcon.GetComponent<Image>().color = activeColor;
		}

		return parent;
	}

	public void ChangeWorkerColor(int availableWorker, Image[] icons)
	{
		int index = 0;
		foreach (Image icon in icons)
		{
			if (index < availableWorker)
			{
				icon.color = activeColor;
			}
			else
			{
				icon.color = inactiveColor;
			}
		}
	}

	public void ChangePotholeCount(Vector3 potholeCounts,TMP_Text potholeTodoCount, TMP_Text potholeInProgressCount, TMP_Text potholeDoneCount)
	{
		potholeTodoCount.text = potholeCounts.x.ToString();
		potholeInProgressCount.text = potholeCounts.y.ToString();
		potholeDoneCount.text = potholeCounts.z.ToString();
	}
}
