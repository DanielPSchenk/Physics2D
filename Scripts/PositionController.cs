using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionController : MonoBehaviour
{
    public float w;
    private PhysicsPart parent;
    private Pid r;
    private ExternalForceEffector actor;
    
    private void Start()
    {
        parent = GetComponent<PhysicsPart>();
        r = GetComponent<Pid>();
        actor = GetComponent<ExternalForceEffector>();
    }

    private void FixedUpdate()
    {
        float e = w - parent.state.x;
        actor.externalForce.x = r.NextU(e);
    }
}
