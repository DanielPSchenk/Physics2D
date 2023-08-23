using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Pid : MonoBehaviour
{
    public float kP;
    public float kI;
    public float kD;
    private float t;
    private float _lastE, _lastU;

    private void Start()
    {
        t = Time.fixedDeltaTime;
    }

    public float NextU(float e)
    {
        float u = kP * e + kI * (t / 2 * (e + _lastE) + _lastU) + kD * (2 / t * (e - _lastE) - _lastU);
        _lastE = e;
        _lastU = u;
        return u;
    }
}
