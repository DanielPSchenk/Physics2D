using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PhysicsManager : MonoBehaviour
{
    // Start is called before the first frame update
    private List<PhysicsPart> _parts;
    private List<Effector> _effectors;
    private Vector3[] _stateVector;
    private Vector3[] _firstDerivativeVector;
    

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
        ApplyGaussStep(Time.fixedDeltaTime);
        SwapOutVector();
    }

    private void SwapOutVector()
    {
        for (int i = 0; i < _parts.Count; i++)
        {
            if (_parts[i].constrained) continue;
            _parts[i].state = _stateVector[i];
            _parts[i].stateDerivative = _firstDerivativeVector[i];
            _parts[i].ApplyUpdate();
        }
    }

    private void SwapInVector()
    {
        _stateVector = new Vector3[_parts.Count];
        _firstDerivativeVector = new Vector3[_parts.Count];
        for (int i = 0; i < _parts.Count; i++)
        {
            _stateVector[i] = _parts[i].state;
            _firstDerivativeVector[i] = _parts[i].stateDerivative;
        }
    }

    private void ApplyGaussStep(float timeStep)
    {
        Vector3[] secondDerivativeVector = new Vector3[_parts.Count];
        foreach (Effector e in _effectors)
        {
            e.ApplyForce(_stateVector, _firstDerivativeVector, secondDerivativeVector);
        }
        
        

        for (int i = 0; i < _parts.Count; i++)
        {
            if(_parts[i].constrained)continue;
            _firstDerivativeVector[i] += timeStep * secondDerivativeVector[i];
            _stateVector[i] += timeStep * _firstDerivativeVector[i];
        }

    }

}
