using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringDamperEffector : Effector
{
    private PhysicsPart _parentPart;

    public PhysicsPart connectedPart;

    public float relaxedDistance = 1;
    public float springConstant = 1;
    public float dampingConstant = 10;
    public float tangentDamping = .1f;
    

    private void Start()
    {
        _parentPart = GetComponent<PhysicsPart>();
    }

    public override void ApplyForce(Vector3[][] stateVector,
        Vector3[] secondDerivativeVector)
    {
        Vector2 parentPosition = new Vector2(stateVector[0][_parentPart.id].x, stateVector[0][_parentPart.id].y);
        Vector2 connectedPosition = new Vector2(stateVector[0][connectedPart.id].x, stateVector[0][connectedPart.id].y);

        Vector2 toConnected = connectedPosition - parentPosition;
        
        Vector2 direction = toConnected.normalized;
        Vector2 tangent = new Vector2(-direction.y, direction.x);
        Vector2 relaxedVector = relaxedDistance * direction;
        Vector2 forceVector = (toConnected - relaxedVector) * springConstant;
        AddForce(_parentPart, forceVector, secondDerivativeVector);
        AddForce(connectedPart, -forceVector, secondDerivativeVector);
        
        float relativeSpeed = Vector2.Dot(direction, (Vector2)stateVector[1][_parentPart.id]) + Vector2.Dot(-direction, (Vector2)stateVector[1][connectedPart.id]);
        float tangentSpeed = Vector2.Dot(tangent, stateVector[1][_parentPart.id]) +
                             Vector2.Dot(-tangent, stateVector[1][connectedPart.id]);
        
        AddForce(_parentPart, -direction * (relativeSpeed * dampingConstant), secondDerivativeVector);
        AddForce(connectedPart, direction * (relativeSpeed * dampingConstant), secondDerivativeVector);
        
        AddForce(_parentPart, -tangent * (tangentSpeed * tangentDamping), secondDerivativeVector);
        AddForce(connectedPart, tangent * (tangentSpeed * tangentDamping), secondDerivativeVector);
    }
}
