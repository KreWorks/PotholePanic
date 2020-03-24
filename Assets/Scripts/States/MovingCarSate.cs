using UnityEngine;
using System;

public class MovingCarSate : CarState
{
	public MovingCarSate(CharacterNavigationController characterNavigationController) : base(characterNavigationController)
	{

	}

	public override bool IsMoving()
	{
		return true;
	}

	public override bool StoppedByCar()
	{
		return !IsMoving();
	}

	public override bool StoppedByPothole()
	{
		return !IsMoving();
	}

	public override void TransitionToState(CarState newState, CharacterNavigationController otherCarController)
	{
		newState.SetOtherCarController(otherCarController);
		base.TransitionToState(newState);
	}

	public override void TransitionToState(CarState newState, Pothole pothole, Action potholeWatcher)
	{
		newState.SetPothole(pothole, potholeWatcher);
		base.TransitionToState(newState);
	}

}
