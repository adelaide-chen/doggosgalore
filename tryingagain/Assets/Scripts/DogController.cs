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
    public Transform ballTransform;
    public Transform headTransform;

    Animator animator;
    NavMeshAgent agent;
    
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
        //Debug.Log(distance);
        if (distance < stopDistance || distance == float.PositiveInfinity) { //but how does he escape
            agent.enabled = false;
            if (state == DogState.Chase) {
                //Debug.Log("Run to drink (chase)");
                animator.SetInteger("state", (int)AnimatorTransition.runToDrink);
                state = DogState.Retrieve;
                agent.enabled = true;
            }
            else if (state == DogState.Retrieve) {
                //Debug.Log("Run to idle (retrieve)");
                animator.SetInteger("state", (int)AnimatorTransition.runToIdle);
            }
        }
        else {
            agent.enabled = true;
            if (state == DogState.Chase) {
                //Debug.Log("Idle to run (chase)");
                animator.SetInteger("state", (int)AnimatorTransition.idleToRun);
            }
            else if (state == DogState.Retrieve) {
                //Debug.Log("Idle to run (retrieve)");
                animator.SetInteger("state", (int)AnimatorTransition.idleToRun);
            }
        }
	}

    public void Fetch() {
        state = DogState.Chase;
        agent.destination = ballTransform.position;
    }

    public void Retrieve() {
        state = DogState.Retrieve;
        agent.destination = headTransform.position;
    }
}
