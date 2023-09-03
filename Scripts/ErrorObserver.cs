using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorObserver : Observer
{
    public PositionController controller;

    public override float GetValue()
    {
        return controller.e;
    }
}
