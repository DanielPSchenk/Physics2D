using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constraint : MonoBehaviour
{
    public bool horizontal, vertical, rotation;

    private PhysicsPart parent;

    private void Start()
    {
        parent = GetComponent<PhysicsPart>();
    }

    public void ApplyForce(Vector3[][] stateVector, Vector3[] secondDerivativeVector)
    {
        if (horizontal)
        {
            secondDerivativeVector[parent.id].x = 0;
        }

        if (vertical)
        {
            secondDerivativeVector[parent.id].y = 0;
        }

        if (rotation)
        {
            secondDerivativeVector[parent.id].z = 0;
        }
    }
    
    

}
