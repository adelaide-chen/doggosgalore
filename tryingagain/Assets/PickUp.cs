using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class PickUp : MonoBehaviour {

    public GameObject ball;
    public GameObject parentFigure;
    public Transform guide;
    public bool carrying;

	// Use this for initialization
	void Start () {
        ball.GetComponent<Rigidbody>().useGravity = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (carrying == false)
        {
            if (Vector3.Distance(ball.transform.position, parentFigure.transform.position) < 0.25)
            {
                PickingUp();
                carrying = true;
                print(carrying);
            }

        }
        else
        {
            Dropping();
            carrying = false;
        }
	}

    void PickingUp()
    {
        ball.GetComponent<Rigidbody>().useGravity = false;
        ball.GetComponent<Rigidbody>().isKinematic = true;
        ball.transform.position = guide.transform.position;
        ball.transform.rotation = guide.transform.rotation;
        ball.transform.parent = parentFigure.transform;
    }

    void Dropping()
    {
        ball.GetComponent<Rigidbody>().useGravity = true;
        ball.GetComponent<Rigidbody>().isKinematic = false;
        ball.transform.parent = null;
        ball.transform.position = guide.transform.position;
    }
}

