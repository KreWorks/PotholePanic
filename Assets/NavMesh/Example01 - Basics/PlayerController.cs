using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
	public Camera cam;
	public NavMeshAgent agent;

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			Debug.Log("you clicked");
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				Debug.Log("You clicked and hit" + hit.point);
				agent.SetDestination(hit.point);
			}
		}
    }
}
