using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionObserver : Observer
{
    public PhysicsPart observedObject;
    public override float GetValue()
    {
        return observedObject.state.x;
    }
}
