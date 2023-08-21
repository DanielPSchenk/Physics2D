using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effector : MonoBehaviour
{
    public abstract void ApplyForce(Vector3[][] stateVector, Vector3[] secondDerivativeVector);

    protected void AddForce(PhysicsPart part, Vector2 force, Vector3[] secondDerivativeVector)
    {
        secondDerivativeVector[part.id] += (Vector3)force / part.mass;
    }
}
