using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheFetchOfDoggo : MonoBehaviour {

    private Animator doggo;
    private MoveTo pupper;

	// Use this for initialization
	void Start () {
        doggo = GetComponent<Animator>();
        pupper = doggo.GetComponent<MoveTo>();
    }
	
	// Update is called once per frame
	void Update () {
        if (pupper.isMoving)
        {
            doggo.SetBool("isChasing", true);
            print("chasingggg");
        } else
        {
            doggo.SetBool("isChasing", false);
        }

        if (Vector3.Distance(pupper.transform.position, pupper.goal.transform.position) < 1.0f)
        {
            doggo.SetBool("isChasing", false);
            pupper.isMoving = false;
            doggo.SetBool("isReached", true);
            doggo.SetBool("isRetrieved", true);
            print("reached and retrieved!!");

        } else
        {
            doggo.SetBool("isReached", false);
            doggo.SetBool("isRetrieved", false);
        }
	}
}
