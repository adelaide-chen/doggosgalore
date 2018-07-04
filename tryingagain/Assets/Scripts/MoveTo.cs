// MoveTo.cs
using UnityEngine;
using System.Collections;

public class MoveTo : MonoBehaviour
{
    public Transform goal;
    public bool isMoving;

    void FixedUpdate()
    {
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (goal.transform.position.y <= agent.nextPosition.y)
        {
            isMoving = true;
            agent.destination = goal.position;
        }
    }
}