using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[System.Serializable]
public class DogController : MonoBehaviour {

    public enum DogState { Chase, Retrieve };
    public enum AnimatorTransition { idleToRun, runToIdle, runToDrink, drinkToRun };
    public DogState state;
    public float stopDistance = 1.0f;
    public GameObject ball;
    public Transform mouth;
    public Transform cameraTransform;

    Animator animator;
    NavMeshAgent agent;

    float animPercent {
        get {
            AnimatorStateInfo animatorInfo = animator.GetCurrentAnimatorStateInfo(0);
            return animatorInfo.normalizedTime;
        }
    }
    
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        
        // Code to trigger action on startup
        if (state == DogState.Chase) {
            Fetch();
        }
        else if (state == DogState.Retrieve) {
            Retrieve();
        }
    }
	
	void FixedUpdate () {
        float distance = Vector3.Distance(transform.position, agent.destination);
        Debug.Log(animPercent);

        switch (state)
        {
            case DogState.Chase:
                if (distance < stopDistance || distance == float.PositiveInfinity)
                {
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
                if (distance < stopDistance || distance == float.PositiveInfinity) {
                    Arrive();
                }
                else {
                    animator.SetInteger("state", (int)AnimatorTransition.drinkToRun);
                }
                break;
            default:
                break;
        }
        
	}

    void Fetch() {
        state = DogState.Chase;
        agent.enabled = true;
        agent.destination = ball.transform.position;
    }

    void Retrieve() {
        state = DogState.Retrieve;
        agent.enabled = true;
        agent.destination = cameraTransform.position;
        Destroy(ball.GetComponent<Rigidbody>());
    }

    void Arrive() {
        agent.enabled = false;
        animator.SetInteger("state", (int)AnimatorTransition.runToIdle);
        //ball.AddComponent<Rigidbody>();
    }
}
