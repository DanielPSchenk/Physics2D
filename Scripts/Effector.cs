using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effector : MonoBehaviour
{
    public abstract void ApplyForce(Vector3[] stateVector, Vector3[] firstDerivativeVector, Vector3[] secondDerivativeVector);
}
