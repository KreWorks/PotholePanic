using UnityEngine;
using System;

public abstract class CarState
{
	protected CharacterNavigationController characterNavigatonController;
	protected float sinceStopped;

	public CarState(CharacterNavigationController characterNavigatonController)
	{
		this.characterNavigatonController = characterNavigatonController;
	}

	public abstract bool IsMoving();
	public abstract bool StoppedByCar();
	public abstract bool StoppedByPothole();

	public virtual void SetPothole(Pothole pothole, Action potholeWatcher) { }
	public virtual Pothole GetPothole() { return null; }
	public virtual void SetOtherCarController(CharacterNavigationController otherCarController) { }
	public virtual CharacterNavigationController GetOtherCarController() { return null; }

	public virtual bool NeedToKillCar() { return false; }

	public virtual void TransitionToState(CarState newState, CharacterNavigationController otherCarController) { }
	public virtual void TransitionToState(CarState newState, Pothole pothole, Action potholeWatcher) { }

	public void TransitionToState(CarState newState)
	{
		this.characterNavigatonController.carState.ExitState();
		this.characterNavigatonController.carState = newState;
		this.characterNavigatonController.carState.EnterState();
	}

	public void EnterState()
	{
		this.sinceStopped = 0.0f;
	}

	public virtual void ExitState() { }

	public void TimeGoesBy(float time) { this.sinceStopped += time; }
	public float GetTimeSinceStopped() { return this.sinceStopped; }
}
