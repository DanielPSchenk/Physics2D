using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ExternalForceEffector : Effector
{
    public Vector2 externalForce;
    public override void ApplyForce(Vector3[][] stateVector, Vector3[] secondDerivativeVector)
    {
        AddForce(parentPart, externalForce, secondDerivativeVector);
    }
}
