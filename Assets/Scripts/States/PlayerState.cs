using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState 
{
	protected GameManager gameManager; 

	public PlayerState(GameManager gameManager)
	{
		this.gameManager = gameManager;
	}
}
