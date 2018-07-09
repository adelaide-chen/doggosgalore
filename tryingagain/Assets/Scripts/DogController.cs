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
    public enum AnimatorTransition { idleToRun, runToIdle, runToDrink, drinkToRun };
    public DogState state;
    public float stopDistance;
    public GameObject ball;
    public Transform mouth;
    public Transform cameraTransform;
    public RWVR_InteractionController controller1;
    public RWVR_InteractionController controller2;

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
        if (state == DogState.Chase)
        {
            Fetch();
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
        //Debug.Log(distance);
        switch (state)
        {
            case DogState.Chase:
                if (distance < stopDistance || distance == Mathf.Infinity)
                {
                    agent.enabled = false;
                    animator.SetInteger("state", (int)AnimatorTransition.runToDrink);
                    Debug.Log("runtodrink");
                    if (animPercent >= 0.3f && animator.GetCurrentAnimatorStateInfo(0).IsName("PugDrink"))
                    {
                        Retrieve();
                    }
                }
                else
                {
                    //Debug.Log(".");
                    animator.SetInteger("state", (int)AnimatorTransition.idleToRun);
                    //agent.SetDestination(ball.transform.position);
                }
                break;
            case DogState.Retrieve:
                ball.transform.position = mouth.position;
                ball.transform.rotation = mouth.rotation;
                if (distance < stopDistance || distance == Mathf.Infinity)
                {
                    agent.enabled = false;
                    animator.SetInteger("state", (int)AnimatorTransition.runToIdle);
                    if (controller1.pressed || controller2.pressed)
                    {
                        ball.GetComponent<Rigidbody>().isKinematic = false;
                        Fetch();
                    }
                }
                else
                {
                    animator.SetInteger("state", (int)AnimatorTransition.drinkToRun);
                }
                break;
            default:
                break;
        }

    }

    void Fetch()
    {
        state = DogState.Chase;
        agent.enabled = true;
        agent.destination = ball.transform.position;
    }

    void Retrieve()
    {
        state = DogState.Retrieve;
        agent.enabled = true;
        ball.GetComponent<Rigidbody>().isKinematic = true;
        agent.destination = cameraTransform.position;
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
