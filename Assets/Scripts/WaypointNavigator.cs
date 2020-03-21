using UnityEngine;

public class WaypointNavigator : MonoBehaviour
{
	CharacterNavigationController characterController;

	public Waypoint currentWaypoint;

	private void Awake()
	{
		characterController = GetComponent<CharacterNavigationController>(); 
	}

    // Update is called once per frame
    void Update()
    {
		if (characterController.reachedDestination)
		{
			if(characterController.goingForward)
			{
				currentWaypoint = currentWaypoint.GetNextWaypoint();
			}
			else if (!characterController.goingForward)
			{
				currentWaypoint = currentWaypoint.GetPreviousWaypoint();
			}
			
			characterController.SetDestination(currentWaypoint.GetPosition(characterController.goingForward));
		}
    }

	public void SetParams(Waypoint waypoint, int direction)
	{
		this.currentWaypoint = waypoint;
		this.characterController.goingForward = direction == 0 ? true : false;

		this.characterController.SetDestination(this.currentWaypoint.GetPosition(characterController.goingForward));

		Vector3 basePosition = new Vector3(waypoint.transform.position.x, 0, waypoint.transform.position.z);

		if (characterController.goingForward)
		{
			transform.position = basePosition + waypoint.transform.right * waypoint.width / 4.0f;
			this.transform.forward = waypoint.transform.forward;
		}
		else
		{
			transform.position = basePosition - waypoint.transform.right * waypoint.width / 4.0f;
			this.transform.forward = -waypoint.transform.forward;
		}
	}
}
