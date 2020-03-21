using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotholeStoppedCarState : CarState
{
	public Pothole pothole;

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

	public override void SetPothole(Pothole pothole)
	{
		this.pothole = pothole;
	}

	public override Pothole GetPothole()
	{
		return this.pothole;
	}
}
