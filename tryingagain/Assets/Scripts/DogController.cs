using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[System.Serializable]
public class DogController : MonoBehaviour
{

    public enum DogState { Chase, Retrieve };
    public enum AnimatorTransition { idleToRun, runToIdle, runToDrink, drinkToRun, idleToDrink, drinkToIdle };
    public float ballStopDistance;
    public float playerStopDistance;
    public Transform mouth;
    public Transform player;
    public Transform ball;

    Animator animator;
    NavMeshAgent agent;
    Rigidbody ballRB;
    Transform target;
    bool hasBall;

    float animPercent
    {
        get
        {
            AnimatorStateInfo animatorInfo = animator.GetCurrentAnimatorStateInfo(0);
            return animatorInfo.normalizedTime;
        }
    }

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        ballRB = ball.GetComponent<Rigidbody>();
        target = ball;
        hasBall = false;
    }

    void FixedUpdate()
    {
        agent.destination = target.position;

        //float distance = CalculatePathDistance(agent);
        float distance = Vector3.Distance(transform.position, target.position);
        // Change this later!
        float xzDistance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(target.position.x, target.position.z));

        if (target == ball)
        {
            if (distance < ballStopDistance)
            {
                agent.isStopped = true;
                animator.SetInteger("state", (int)AnimatorTransition.runToDrink);
                // What if idle to drink?
                if (animPercent >= .3f && animator.GetCurrentAnimatorStateInfo(0).IsName("PugDrink"))
                {
                    Debug.Log("transition");
                    target = player;
                    hasBall = true;
                    ballRB.isKinematic = true;
                    agent.isStopped = false;
                }
            }
            else
            {
                agent.isStopped = false;
                animator.SetInteger("state", (int)AnimatorTransition.idleToRun);
            }
        }
        else if (target == player)
        {
            if (xzDistance < playerStopDistance)
            {
                agent.isStopped = true;
                // What if drinking when less than playerStopDistance?
                animator.SetInteger("state", (int)AnimatorTransition.runToIdle);
            }
            else
            {
                agent.isStopped = false;
                animator.SetInteger("state", (int)AnimatorTransition.drinkToRun);
            }
            if (hasBall)
            {
                ball.position = mouth.position;
                ball.rotation = mouth.rotation;
            }
        }
        else
        {
            // Do nothing
            animator.SetInteger("state", (int)AnimatorTransition.runToIdle);
        }

    }

    public void Stop()
    {
        target = transform;
        ballRB.isKinematic = false;
        hasBall = false;
    }

    public void Go()
    {
        target = ball;
    }

    /*private float CalculatePathDistance(NavMeshAgent agent)
    {
        NavMeshPath path = agent.path;j lkgfd;sajfd;lskafj;dlska

        float distance = 0f;
        Vector3[] corners = path.corners;
        for (int c = 0; c < corners.Length - 1; ++c)
        {
            distance += Mathf.Abs((corners[c] - corners[c + 1]).magnitude);
        }
        return distance;
    }*/
}
