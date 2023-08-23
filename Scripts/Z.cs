using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Z : Effector
{
    private PhysicsPart parent;

    private float x = 0;

    public float omega = 1;

    public float amplitude = 2;

    public float o = 1;
    // Update is called once per frame
    public override void ApplyForce(Vector3[][] stateVector, Vector3[] secondDerivativeVector)
    {
        AddForce(parent, new Vector2(o + amplitude * Mathf.Sin(x), 0), secondDerivativeVector);
    }

    private void Start()
    {
        parent = GetComponent<PhysicsPart>();
    }

    void FixedUpdate()
    {
        x += omega * Time.fixedDeltaTime;
        
    }
}
