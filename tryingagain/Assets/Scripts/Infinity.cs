using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Infinity : MonoBehaviour {

    public float pathlength;
    public bool everyframe;
    private NavMeshAgent agent;
    public GameObject obj;
    
    public void Reset()
    {
        pathlength = 0f;
        everyframe = false;
        obj = null;
    }

    public void OnEnter()
    {
        agent = GetComponent<NavMeshAgent>();
        if (!everyframe)
        {
            CornersCalculation();
        }
    }

    public void OnUpdate()
    {
        if (everyframe)
        {
            CornersCalculation();
        }
    }

    public void CornersCalculation()
    {
        obj = GetComponent<GameObject>();
        if (obj == null)
        {
            return;
        }

        NavMeshPath path = agent.path;

        Vector3 previousCorner = path.corners[0];
        float lengthSoFar = 0f;
        int i = 1;
        while (i < path.corners.Length)
        {
            Vector3 currentCorner = path.corners[1];
            lengthSoFar += Vector3.Distance(previousCorner, currentCorner);
            previousCorner = currentCorner;
            i++;
        }

        pathlength = lengthSoFar;
    }
}