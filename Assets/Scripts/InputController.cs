using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour
{
	public CameraController cameraController;
	public UiController uiController;

	public string potholeTagName;
	public string mainHouseTagName;


	void Update()
	{
		GetInput();
	}

	public void GetInput()
	{
		//you pressed the button AND you clicked on a non UI object
		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray.origin, ray.direction, out hit) && hit.transform.tag == potholeTagName)
			{
				HandlePotholeClick(hit);
			}
			else if (Physics.Raycast(ray.origin, ray.direction, out hit) && hit.transform.tag == mainHouseTagName)
			{
				HandleBaseClick();
			}
		}
	}

	void HandlePotholeClick(RaycastHit hit)
	{
		Pothole pothole = hit.transform.gameObject.GetComponent<Pothole>();

		if (pothole.status == PotholeStatus.ToDo)
		{
			uiController.OpenAssignPanel(pothole);
		}
		else if (pothole.status == PotholeStatus.InProgress)
		{
			uiController.OpenStatusPanel(pothole);
		}
	}

	void HandleBaseClick()
	{
		uiController.overviewPanel.SetActive(true);
	}
}
