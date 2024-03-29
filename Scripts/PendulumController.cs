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

    public float wOuter;
    public float wInner;

    public float ArcsinApproximation(float x)
    {
        return x + x * x * x / 6;
    }

    public float dInner, tInner, dOuter, tOuter, kOuter, kInner;
    
    public override void ApplyForce(UnityEngine.Vector3[][] stateVector, UnityEngine.Vector3[] secondDerivativeVector)
    {
        //PID control
        //AddForce(b, new Vector2(p.NextU(phi()),0), secondDerivativeVector);
        
        //IO-linearization control
        //outer controller

        wInner = kOuter * ((wOuter - 1 * stateVector[0][pendulum.id].x - 2 * tOuter * dOuter * stateVector[1][pendulum.id].x) * 2 / 5 /(tOuter * tOuter));

        

        //inner controller
        var relMov = stateVector[1][pendulum.id] - stateVector[1][b.id];
        Vector2 con = (stateVector[0][pendulum.id] - stateVector[0][b.id]);
        Vector2 direction = con.normalized;
        Vector2 tangent = new Vector2(direction.y, -direction.x);
        float phiDerivative = Vector2.Dot(tangent, relMov) / con.magnitude;
        float p = phi();
        float divider = .5f / dInner / tInner;
        float ypp = 9.81f / 5f * Mathf.Sin(p);
        float yp = phiDerivative;
        float y = p;
        float u = kInner * (-wInner + ypp + 2* dInner * tInner * yp + p) / (2.5f * Mathf.Cos(p) * tInner * tInner );
        
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
