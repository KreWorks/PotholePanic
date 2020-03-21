using UnityEngine;

public class WaypointNavigator : MonoBehaviour
{
	CharacterNavigationController characterController;

	public Waypoint currentWaypoint;
	public Waypoint lastWaypoint;

	private void Awake()
	{
		characterController = GetComponent<CharacterNavigationController>(); 
	}

    // Update is called once per frame
    void Update()
    {
		if (characterController.reachedDestination)
		{
			WaypointWithDirection newWaypoint = new WaypointWithDirection();
			lastWaypoint = currentWaypoint;

			if (characterController.goingForward)
			{
				newWaypoint = currentWaypoint.GetNextWaypoint(lastWaypoint);
				currentWaypoint = newWaypoint.waypoint;
			}
			else if (!characterController.goingForward)
			{
				newWaypoint = currentWaypoint.GetPreviousWaypoint(lastWaypoint);
				currentWaypoint = newWaypoint.waypoint;
			}

			characterController.SetDestination(currentWaypoint.GetPosition(newWaypoint.goingForward), newWaypoint.goingForward);
		}
    }

	public void SetParams(Waypoint waypoint, int direction)
	{
		this.currentWaypoint = waypoint;
		this.characterController.goingForward = direction == 0 ? true : false;

		this.characterController.SetDestination(this.currentWaypoint.GetPosition(characterController.goingForward));

		Vector3 basePosition = waypoint.transform.position + new Vector3(0, 0.01f, 0);

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
