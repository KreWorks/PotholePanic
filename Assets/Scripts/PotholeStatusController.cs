using UnityEngine;
using UnityEngine.UI;

public class PotholeStatusController : MonoBehaviour
{
	public Slider slider;

	public GameObject workersPanel;
	public GameObject potholeTypesPanel;

	public Color activeColor;
	public Color inactiveColor;

	UIHelper uiHelper;
	Pothole hole;

	// Start is called before the first frame update
	void Start()
    {
		uiHelper = new UIHelper(activeColor, inactiveColor);
	}

    // Update is called once per frame
    void Update()
    {
		float status = hole.progress * slider.maxValue / hole.repairTime;

		slider.value = status;
	}

	public void SetParams(Pothole pothole)
	{
		this.hole = pothole;
		PotholeSize holeType = hole.size;
		Image[] icons = workersPanel.GetComponentsInChildren<Image>();
		Image[] holes = potholeTypesPanel.GetComponentsInChildren<Image>(); 

		uiHelper.ChangeWorkerColor(pothole.assignedWorkers, icons);

		uiHelper.SetPotholeTypeIcon(holes, holeType);
	}

}
