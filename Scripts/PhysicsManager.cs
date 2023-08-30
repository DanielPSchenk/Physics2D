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
    private List<Constraint> _constraints;
    private Vector3[][] _stateVector;
    private bool _needsVectorSwapIn;

    Vector3[][] _t1, _t2, _s2, _t3, _s3, _t4, _s4;
    

    void CreatePartsVector()
    {
        _parts = new List<PhysicsPart>(Transform.FindObjectsOfType<PhysicsPart>());
        for (int i = 0; i < _parts.Count; i++)
        {
            _parts[i].id = i;
        }

        _effectors = new List<Effector>(Transform.FindObjectsOfType<Effector>());
        _constraints = new List<Constraint>(Transform.FindObjectsOfType<Constraint>());
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
        _t1 = VectorAllocate();
        _t2 = VectorAllocate();
        _t3 = VectorAllocate();
        _t4 = VectorAllocate();
        
        _s2 = VectorAllocate();
        _s3 = VectorAllocate();
        _s4 = VectorAllocate();
        
    }

    private void ApplyGaussStep(float timeStep)
    {
        NewDerivativeVector(_stateVector, _t1);
        StateAfterStep(_stateVector, _t1, timeStep, _stateVector);

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

        foreach (var c in _constraints)
        {
            c.ApplyForce(stateVector, ret[1]);
        }
    }

    private void StateAfterStep(Vector3[][] state, Vector3[][] step, float timeStep, Vector3[][] ret)
    {

        for (int i = 0; i < _parts.Count; i++)
        {

                ret[0][i] = state[0][i] + state[1][i] * timeStep;
                ret[1][i] = state[1][i] + step[1][i] * timeStep;

        }

        
    }

    private void ApplyRungeKuttaStep(float timeStep)
    {
        
        NewDerivativeVector(_stateVector, _t1);
        StateAfterStep(_stateVector, _t1, timeStep / 2, _s2);

        NewDerivativeVector(_s2, _t2);
        StateAfterStep(_stateVector, _t2, timeStep / 2, _s3);

        NewDerivativeVector(_s3, _t3);
        StateAfterStep(_stateVector, _t3, timeStep, _s4);
        NewDerivativeVector(_s4, _t4);

        //Debug.Log(_stateVector[1][0] + " " + s2[1][0] + " " + s3[1][0] + s4[1][0]);

        StateAfterStep(_stateVector, _t1, timeStep / 6, _stateVector);
        StateAfterStep(_stateVector, _t2, timeStep / 3, _stateVector);
        StateAfterStep(_stateVector, _t3, timeStep / 3, _stateVector);
        StateAfterStep(_stateVector, _t4, timeStep / 6, _stateVector);
    }

}
