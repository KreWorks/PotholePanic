using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
	private Action<Vector3> OnPointerDownHandler;
	private Action OnPointerUpHandler;


	public string potholeTagName;
	public string mainHouseTagName;
	public CameraController camCont;

	UIManager uiManager;
	//private Action<Vector3> OnPointerDownHandler;

	// Start is called before the first frame update
	void Start()
    {
		uiManager = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
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
				Vector3 position = hit.point - transform.position;

				Pothole ph = hit.transform.gameObject.GetComponent<Pothole>();
				if (ph.status == PotholeStatus.ToDo)
				{
					uiManager.OpenAssignTab(hit.transform.gameObject);
				}
				else if(ph.status == PotholeStatus.InProgress)
				{
					uiManager.PotholeStatus(ph);
				}
			}
			else if (Physics.Raycast(ray.origin, ray.direction, out hit) && hit.transform.tag == mainHouseTagName)
			{
				uiManager.OverViewTab.SetActive(true);
			}
		}
	}

	/*public void AddListenerOnPointerDownevent(Action<Vector3> listener)
	{
		OnPointerDownHandler += listener;
	}

	public void RemoveListenerOnPointerDownEvent(Action<Vector3> listener)
	{
		OnPointerDownHandler -= listener;
	}*/
}
