using UnityEngine;

public class CharacterNavigationController : MonoBehaviour
{
	public float movementSpeed;
	public int rotationSpeed;
	public float stopDistance;
	public Vector3 destination;
	public bool reachedDestination = false;
	public bool goingForward;

	float inFrontAngle = 15f;
	float inFrontDistance = 0.7f;

	private void Start()
	{
		Reset();
	}

	public void Reset()
	{
		movementSpeed = 1.2f;
		rotationSpeed = 180;
		stopDistance = 0.5f;
		reachedDestination = false;
	}

	// Update is called once per frame
	void Update()
    {
        if (transform.position != destination)
		{
			Vector3 destinationDirection = destination - transform.position;
			destinationDirection.y = 0;

			float destinationDistance = destinationDirection.magnitude;

			if (!IsOtherCarInFrontOf(inFrontDistance))
			{
				if (destinationDistance >= stopDistance)
				{
					reachedDestination = false;
					Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
					transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
					transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
				}
				else
				{
					reachedDestination = true;
				}
			}
		}
    }

	public void SetDestination(Vector3 destination)
	{
		this.destination = destination;
		this.reachedDestination = false;
	}

	bool IsOtherCarInFrontOf(float radius)
	{
		CharacterNavigationController[] cars = FindObjectsOfType<CharacterNavigationController>();

		foreach (CharacterNavigationController car in cars)
		{
			if (car != this)
			{
				Vector3 carForward = this.transform.forward;
				Vector3 otherCarDirection = car.transform.position - this.transform.position;

				float angle = Vector3.Angle(carForward, otherCarDirection);
				float distanceSqr = (otherCarDirection).sqrMagnitude;

				if (distanceSqr < radius)
				{
					if (angle > -inFrontAngle && angle < inFrontAngle)
					{
						return true;
					}
				}
			}
		}

		return false;
	}

	bool IsPotholeInFrontOf()
	{
		Pothole[] potholes = FindObjectsOfType<Pothole>();


		return false;
	}
}
