using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedObserver : Observer
{
    public PhysicsPart observedObject;
    public override float GetValue()
    {
        return observedObject.stateDerivative.x;
    }
}
