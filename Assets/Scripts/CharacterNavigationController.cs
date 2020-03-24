using UnityEngine;

public class CharacterNavigationController : MonoBehaviour
{
	public float movementSpeed;
	public int rotationSpeed;
	public float stopDistance;
	public Vector3 destination;
	public bool reachedDestination = false;
	public bool goingForward;

	float carInFrontAngle = 30f;
	float potholeInFrontAngle = 70f;
	float inFrontDistance = 0.7f;
	float toleranceTime = 5f;

	public CarState carState;

	public MovingCarSate movingState;
	public CarStoppedCarState stoppedByCarState;
	public PotholeStoppedCarState stoppedByPotholeState;

	private void Awake()
	{
		movingState = new MovingCarSate(this);
		stoppedByCarState = new CarStoppedCarState(this, toleranceTime);
		stoppedByPotholeState = new PotholeStoppedCarState(this);
	}

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
		carState = movingState;
	}

	// Update is called once per frame
	void Update()
	{
		Pothole pothole = CheckForPotholes();

		if(pothole != null)
		{
			carState.TransitionToState(stoppedByPotholeState, pothole, PotholeDone);
		}

		CharacterNavigationController otherCar = CheckForOtherCars();
		if (otherCar != null)
		{
			if (otherCar.carState.StoppedByPothole())
			{
				carState.TransitionToState(stoppedByPotholeState, otherCar.carState.GetPothole(), PotholeDone);
			}
			else
			{
				carState.TransitionToState(stoppedByCarState, otherCar);
			}
		}

		if (pothole == null && otherCar == null && !carState.IsMoving())
		{
			carState.TransitionToState(movingState);
		}

		if (carState.IsMoving())
		{
			MoveTowardsDestination();
		}
		else
		{
			carState.TimeGoesBy(Time.deltaTime);
			if (carState.NeedToKillCar())
			{
				Destroy(this.gameObject);
			}
		}
	}

	private void MoveTowardsDestination()
	{
		if (transform.position != destination)
		{
			Vector3 destinationDirection = destination - transform.position;
			destinationDirection.y = 0;

			float destinationDistance = destinationDirection.magnitude;

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

	public void SetDestination(Vector3 destination, bool goingForward = true)
	{
		this.destination = destination;
		this.goingForward = goingForward;
		this.reachedDestination = false;
	}

	public void PotholeDone()
	{
		carState.TransitionToState(movingState);
	}


	CharacterNavigationController CheckForOtherCars()
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

				if (distanceSqr < inFrontDistance && (angle > -carInFrontAngle && angle < carInFrontAngle))
				{
					return car;
				}
			}
		}

		return null;
	}

	Pothole CheckForPotholes()
	{
		Pothole[] potholes = FindObjectsOfType<Pothole>();

		foreach (Pothole pothole in potholes)
		{
			Vector3 carForward = this.transform.forward;
			Vector3 potholeDirection = pothole.GetPotholePositionOnRoad() - this.transform.position;

			float angle = Vector3.Angle(carForward, potholeDirection);
			float distanceSqr = (potholeDirection).sqrMagnitude;

			//TODO somehow detect with side has a pothole
			pothole.GetPotholePositionOnRoad();

			if (distanceSqr < 2 * inFrontDistance && (angle > -potholeInFrontAngle && angle < potholeInFrontAngle))
			{
				return pothole;
			}
		}

		return null;
	}
}
