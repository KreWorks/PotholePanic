using UnityEngine;
using UnityEngine.UI;

public class PotholeStatusManager : MonoBehaviour
{
	public Slider slider;

	public Image[] holes;
	public Image[] workers;

	public Color activeColor;
	public Color inactiveColor;

	Pothole hole;

	public void SetParams(Pothole ph)
	{
		this.hole = ph;
		PotholeSize holeType = hole.size;

		//Reset
		for(int i =0; i < workers.Length; i++)
		{
			workers[i].color = activeColor;
		}

		for (int i = 0; i < holes.Length; i++)
		{
			holes[i].color = inactiveColor;
		}


		switch (hole.assignedWorkers)
		{
			case 0:
				workers[0].color = new Color(workers[0].color.r, workers[0].color.g, workers[0].color.b, 0); ;
				workers[1].color = new Color(workers[1].color.r, workers[1].color.g, workers[1].color.b, 0); ;
				workers[2].color = new Color(workers[2].color.r, workers[2].color.g, workers[2].color.b, 0); ;
				break;
			case 1:
				workers[1].color = new Color(workers[1].color.r, workers[1].color.g, workers[1].color.b, 0); ;
				workers[2].color = new Color(workers[2].color.r, workers[2].color.g, workers[2].color.b, 0); ;
				break;
			case 2:
				workers[2].color = new Color(workers[2].color.r, workers[2].color.g, workers[2].color.b, 0); ;
				break;
			default:
				break;
		}

		//Set t active the hole size
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
	}

	void Update()
	{
		float status = hole.progress * slider.maxValue / hole.repairTime;

		slider.value = status;
	}
}
