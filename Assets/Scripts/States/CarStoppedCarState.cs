using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarStoppedCarState : CarState
{
	public CharacterNavigationController otherCarController;
	float toleranceTime;

	public CarStoppedCarState(CharacterNavigationController characterNavigationController, float toleranceTime) : base(characterNavigationController)
	{
		this.otherCarController = null;
		this.sinceStopped = 0;
		this.toleranceTime = toleranceTime;
	}

	public override bool IsMoving()
	{
		return false;
	}

	public override bool StoppedByCar()
	{
		return true;
	}

	public override bool StoppedByPothole()
	{
		return false;
	}

	public override bool NeedToKillCar()
	{
		return this.sinceStopped > this.toleranceTime;
	}

	public override void SetOtherCarController(CharacterNavigationController otherCarController)
	{
		this.otherCarController = otherCarController;
	}

	public override CharacterNavigationController GetOtherCarController()
	{
		return this.otherCarController;
	}
}
