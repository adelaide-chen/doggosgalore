using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[System.Serializable]
public class DogController : MonoBehaviour
{

    public enum DogState { Idle, Chase, Retrieve };
    public enum AnimatorTransition { idleToRun, runToIdle, runToDrink, drinkToRun };
    public DogState state;
    public float ballStopDistance;
    public float playerStopDistance;
    public GameObject ball;
    public Rigidbody ballRB;
    public Transform mouth;
    public Transform cameraTransform;

    Animator animator;
    NavMeshAgent agent;

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

        // Code to trigger action on startup
        if (state == DogState.Idle)
        {
            Idle();
        }
        else if (state == DogState.Chase)
        {
            Chase();
        }
        else if (state == DogState.Retrieve)
        {
            Retrieve();
        }
    }

    void FixedUpdate()
    {
        //float distance = CalculatePathDistance(agent);
        float distance = Vector3.Distance(transform.position, agent.destination);
        Debug.Log(distance);
        Debug.Log((AnimatorTransition)animator.GetInteger("state"));
        switch (state)
        {
            case DogState.Idle:
                animator.SetInteger("state", (int)AnimatorTransition.runToIdle);
                if (distance > playerStopDistance && distance != Mathf.Infinity)
                {
                    Chase();
                }
                break;
            case DogState.Chase:
                if (distance < ballStopDistance || distance == Mathf.Infinity) {
                    agent.enabled = false;
                    animator.SetInteger("state", (int)AnimatorTransition.runToDrink);
                    if (animPercent >= 0.3f && animator.GetCurrentAnimatorStateInfo(0).IsName("PugDrink"))
                    {
                        Retrieve();
                    }
                }
                else
                {
                    animator.SetInteger("state", (int)AnimatorTransition.idleToRun);
                }
                break;
            case DogState.Retrieve:
                ball.transform.position = mouth.position;
                ball.transform.rotation = mouth.rotation;
                if (distance < playerStopDistance || distance == Mathf.Infinity)
                {
                    animator.SetInteger("state", (int)AnimatorTransition.runToIdle);
                    Debug.Log("here");
                    agent.enabled = false;
                }
                else
                {
                    animator.SetInteger("state", (int)AnimatorTransition.drinkToRun);
                    Debug.Log("there");
                }
                break;
            default:
                break;
        }

    }

    public void Idle()
    {
        ballRB.isKinematic = false;
        state = DogState.Idle;
    }

    public void Chase()
    {
        ballRB.isKinematic = false;
        agent.enabled = true;
        agent.destination = ball.transform.position;
        state = DogState.Chase;
    }

    public void Retrieve()
    {
        ballRB.isKinematic = true;
        agent.enabled = true;
        agent.destination = cameraTransform.position;
        state = DogState.Retrieve;
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
