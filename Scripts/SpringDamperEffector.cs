using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringDamperEffector : Effector
{
    private PhysicsPart _parentPart;

    public PhysicsPart connectedPart;

    public float relaxedDistance;
    public float springConstant;
    

    private void Start()
    {
        _parentPart = GetComponent<PhysicsPart>();
    }

    public override void ApplyForce(Vector3[] stateVector, Vector3[] firstDerivativeVector,
        Vector3[] secondDerivativeVector)
    {
        Vector2 parentPosition = new Vector2(stateVector[_parentPart.id].x, stateVector[_parentPart.id].y);
        Vector2 connectedPosition = new Vector2(stateVector[connectedPart.id].x, stateVector[connectedPart.id].y);

        Vector2 toConnected = connectedPosition - parentPosition;


        Vector2 direction = toConnected.normalized;
        Vector2 relaxedVector = relaxedDistance * direction;
        Vector2 forceVector = (toConnected - relaxedVector) * springConstant;
        Vector3 forceVector3 = forceVector;

        secondDerivativeVector[_parentPart.id] += forceVector3 / _parentPart.mass;
        Debug.Log(parentPosition + " " + relaxedVector + " " + toConnected + " " + forceVector3 / _parentPart.mass + " " + (forceVector3 / _parentPart.mass).magnitude);
        secondDerivativeVector[connectedPart.id] += forceVector3 / (-connectedPart.mass);
    }
}
