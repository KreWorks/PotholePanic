using UnityEngine;
using System;

public class PotholeStoppedCarState : CarState
{
	public Pothole pothole;

	Action potholeWatcherAction;

	public PotholeStoppedCarState(CharacterNavigationController characterNavigationController) : base(characterNavigationController)
	{
		this.pothole = null;
		this.sinceStopped = 0;
	}

	public override bool IsMoving()
	{
		return false;
	}

	public override bool StoppedByCar()
	{
		return false;
	}

	public override bool StoppedByPothole()
	{
		return true;
	}

	public override void SetPothole(Pothole pothole, Action potholeWatcher)
	{
		this.pothole = pothole;
		this.pothole.AddCarToPothole();
		this.pothole.AddListenerOnPotholeDestructionEvent(potholeWatcher);
		this.potholeWatcherAction = potholeWatcher;
	}

	public override void ExitState()
	{
		this.pothole.RemoveListenerOnPotholeDestructionEvent(this.potholeWatcherAction);
	}

	public override Pothole GetPothole()
	{
		return this.pothole;
	}
}
