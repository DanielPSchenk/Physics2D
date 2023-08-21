using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.PlayerLoop;


public class PhysicsManager : MonoBehaviour
{
    // Start is called before the first frame update
    private List<PhysicsPart> _parts;
    private List<Effector> _effectors;
    private Vector3[][] _stateVector;
    

    void CreatePartsVector()
    {
        _parts = new List<PhysicsPart>(Transform.FindObjectsOfType<PhysicsPart>());
        for (int i = 0; i < _parts.Count; i++)
        {
            _parts[i].id = i;
        }

        _effectors = new List<Effector>(Transform.FindObjectsOfType<Effector>());
    }
    void Start()
    {
        CreatePartsVector();
        SwapInVector();
        
        
    }

    private void FixedUpdate()
    {
        ApplyRungeKuttaStep(Time.fixedDeltaTime);
        SwapOutVector();
    }

    private void SwapOutVector()
    {
        for (int i = 0; i < _parts.Count; i++)
        {
            if (_parts[i].constrained) continue;
            _parts[i].state = _stateVector[0][i];
            _parts[i].stateDerivative = _stateVector[1][i];
            _parts[i].ApplyUpdate();
        }
    }

    private void SwapInVector()
    {
        _stateVector = new Vector3[2][];
        _stateVector[0] = new Vector3[_parts.Count()];
        _stateVector[1] = new Vector3[_parts.Count()];
        
        for (int i = 0; i < _parts.Count; i++)
        {
            _stateVector[0][i] = _parts[i].state;
            _stateVector[1][i] = _parts[i].stateDerivative;
        }
    }

    private void ApplyGaussStep(float timeStep)
    {
        Vector3[][] t1 = NewDerivativeVector(_stateVector);
        _stateVector = StateAfterStep(_stateVector, t1, timeStep);

    }

    private Vector3[][] NewDerivativeVector(Vector3[][] stateVector)
    {
        Vector3[][] ret = new Vector3[2][];
        ret[0] = new Vector3[_parts.Count()];
        ret[1] = new Vector3[_parts.Count()];
        for (int i = 0; i < _parts.Count(); i++)
        {
            ret[0][i] = stateVector[1][i];
        }

        foreach (var e in _effectors)
        {
            e.ApplyForce(stateVector, ret[1]);
        }

        return ret;
    }

    private Vector3[][] StateAfterStep(Vector3[][] state, Vector3[][] step, float timeStep)
    {
        Vector3[][] ret = new Vector3[2][];
        ret[0] = new Vector3[_parts.Count()];
        ret[1] = new Vector3[_parts.Count()];

        for (int i = 0; i < _parts.Count; i++)
        {
            if (!_parts[i].constrained)
            {
                ret[0][i] = state[0][i] + state[1][i] * timeStep;
                ret[1][i] = state[1][i] + step[1][i] * timeStep;
            }
            else
            {
                ret[0][i] = state[0][i];
                ret[1][i] = state[1][i];
            }
        }

        return ret;
    }

    private void ApplyRungeKuttaStep(float timeStep)
    {
        Vector3[][] t1 = NewDerivativeVector(_stateVector);
        Vector3[][] s2 = StateAfterStep(_stateVector, t1, timeStep / 2);

        Vector3[][] t2 = NewDerivativeVector(s2);
        Vector3[][] s3 = StateAfterStep(_stateVector, t2, timeStep / 2);

        Vector3[][] t3 = NewDerivativeVector(s3);
        Vector3[][] s4 = StateAfterStep(_stateVector, t3, timeStep);

        Vector3[][] t4 = NewDerivativeVector(s4);
        
        //Debug.Log(_stateVector[1][0] + " " + s2[1][0] + " " + s3[1][0] + s4[1][0]);

        _stateVector = StateAfterStep(_stateVector, t1, timeStep / 6);
        _stateVector = StateAfterStep(_stateVector, t2, timeStep / 3);
        _stateVector = StateAfterStep(_stateVector, t3, timeStep / 3);
        _stateVector = StateAfterStep(_stateVector, t4, timeStep / 6);
    }

}
