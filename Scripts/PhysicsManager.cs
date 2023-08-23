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
    private bool _needsVectorSwapIn;

    Vector3[][] t1, t2, s2, t3, s3, t4, s4;
    

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
        _needsVectorSwapIn = true;      
    }

    private void FixedUpdate()
    {
        if (_needsVectorSwapIn)
        {
            CreatePartsVector();
            SwapInVector();
            AllocateAllVectors();
            _needsVectorSwapIn = false;
        }
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
        _stateVector = VectorAllocate();
        
        for (int i = 0; i < _parts.Count; i++)
        {
            _stateVector[0][i] = _parts[i].state;
            _stateVector[1][i] = _parts[i].stateDerivative;
        }
    }

    private Vector3[][] VectorAllocate(){
        Vector3[][] vec = new Vector3[2][];
        vec[0] = new Vector3[_parts.Count];
        vec[1] = new Vector3[_parts.Count];
        return vec;
    }

    private void AllocateAllVectors()
    {
        t1 = VectorAllocate();
        t2 = VectorAllocate();
        t3 = VectorAllocate();
        t4 = VectorAllocate();
        
        s2 = VectorAllocate();
        s3 = VectorAllocate();
        s4 = VectorAllocate();
        
    }

    private void ApplyGaussStep(float timeStep)
    {
        NewDerivativeVector(_stateVector, t1);
        StateAfterStep(_stateVector, t1, timeStep, _stateVector);

    }

    private void NewDerivativeVector(Vector3[][] stateVector, Vector3[][] ret)
    {
        for (int i = 0; i < _parts.Count(); i++)
        {
            ret[0][i] = stateVector[1][i];
            ret[1][i] = Vector3.zero;
        }

        foreach (var e in _effectors)
        {
            e.ApplyForce(stateVector, ret[1]);
        }

        
    }

    private void StateAfterStep(Vector3[][] state, Vector3[][] step, float timeStep, Vector3[][] ret)
    {

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

        
    }

    private void ApplyRungeKuttaStep(float timeStep)
    {
        
        NewDerivativeVector(_stateVector, t1);
        StateAfterStep(_stateVector, t1, timeStep / 2, s2);

        NewDerivativeVector(s2, t2);
        StateAfterStep(_stateVector, t2, timeStep / 2, s3);

        NewDerivativeVector(s3, t3);
        StateAfterStep(_stateVector, t3, timeStep, s4);
        NewDerivativeVector(s4, t4);

        //Debug.Log(_stateVector[1][0] + " " + s2[1][0] + " " + s3[1][0] + s4[1][0]);

        StateAfterStep(_stateVector, t1, timeStep / 6, _stateVector);
        StateAfterStep(_stateVector, t2, timeStep / 3, _stateVector);
        StateAfterStep(_stateVector, t3, timeStep / 3, _stateVector);
        StateAfterStep(_stateVector, t4, timeStep / 6, _stateVector);
    }

}
