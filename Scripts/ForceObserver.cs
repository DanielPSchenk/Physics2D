using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceObserver : Observer
{
    public ExternalForceEffector force;

    public override float GetValue()
    {
        return force.externalForce.x;
    }
}
