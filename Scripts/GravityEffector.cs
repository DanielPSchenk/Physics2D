using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GravityEffector : Effector
{
    private PhysicsPart _myPart;

    private void Start()
    {
        _myPart = GetComponent<PhysicsPart>();
    }

    public override void ApplyForce(Vector3[][] stateVector,
        Vector3[] secondDerivativeVector)
    {
        secondDerivativeVector[_myPart.id] -= new Vector3(0, 9.81f, 0);
    }

}
