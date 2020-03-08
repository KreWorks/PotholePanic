using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PotholeState 
{
	protected Pothole pothole;

	public PotholeState(Pothole hole)
	{
		this.pothole = hole;
	}

	public virtual void OnEnterState() { }
	public virtual void OnExitState() { }

	public abstract void OnState();
}
