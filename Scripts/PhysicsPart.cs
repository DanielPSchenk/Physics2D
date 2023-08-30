using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsPart : MonoBehaviour
{
    public float mass = 1;

    public Vector3 state;

    public Vector3 stateDerivative;

    public int id;

    private void Awake()
    {
        SetPosition();
    }

    public void SetPosition()
    {
        state.x = transform.position.x;
        state.y = transform.position.y;
    }

    public void ApplyUpdate()
    {
        transform.SetPositionAndRotation(new Vector3(state.x, state.y, transform.position.z), Quaternion.AngleAxis(state.z, Vector3.back));
        
    }
    
    
}
