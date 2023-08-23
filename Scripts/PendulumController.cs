using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class PendulumController : Effector
{
    public PhysicsPart b;
    public PhysicsPart pendulum;

    private Pid p;
    
    // Start is called before the first frame update
    public override void ApplyForce(UnityEngine.Vector3[][] stateVector, UnityEngine.Vector3[] secondDerivativeVector)
    {
        //PID control
        //AddForce(b, new Vector2(p.NextU(phi()),0), secondDerivativeVector);
        
        //IO-linearization control
        var relMov = stateVector[1][pendulum.id] - stateVector[1][b.id];
        Vector2 con = (stateVector[0][pendulum.id] - stateVector[0][b.id]);
        Vector2 direction = con.normalized;
        Vector2 tangent = new Vector2(direction.y, -direction.x);
        float phiDerivative = Vector2.Dot(tangent, relMov) / con.magnitude;
        float p = phi();
        float u = (9.81f * 25 * Mathf.Sin(p) + 50f * phiDerivative + 625f * p) / (2.5f * Mathf.Cos(p));
        
        AddForce(b, new Vector2(u, 0), secondDerivativeVector);
    }

    void Start()
    {
        p = GetComponent<Pid>();
    }

    private float phi()
    {
        var diff = pendulum.state - b.state;
        return MathF.Atan(diff.x / diff.y);
    }

    
}
