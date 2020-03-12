using UnityEngine;
using UnityEngine.AI;

public class carAI : MonoBehaviour
{
	public NavMeshAgent agent;

	Vector3 destination = new Vector3(0,1,0);
	bool isMoving = false;

    // Update is called once per frame
    void Update()
    {
		if (!isMoving)
		{
			agent.SetDestination(destination);
			
		}
		if (agent.isStopped)
		{
			destination = new Vector3(Random.Range(-10, 10), 1, Random.Range(-10, 10));
		}

	}
}
