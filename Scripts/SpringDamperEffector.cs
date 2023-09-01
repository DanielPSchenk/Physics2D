using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpringDamperEffector : Effector
{
    GameObject connector;
    public GameObject connectorPrefab;

    public PhysicsPart connectedPart;

    public float relaxedDistance = 1;
    public float springConstant = 1;
    public float dampingConstant = 10;
    public float tangentDamping = .1f;


    private void Awake()
    {
        connector = Instantiate(connectorPrefab);
        SpriteRenderer sp = connector.GetComponent<SpriteRenderer>();
        sp.size = new Vector2(relaxedDistance, .5f);
    }

    private void Update()
    {
        connector.transform.position = (connectedPart.transform.position + parentPart.transform.position) * .5f;
        connector.transform.right = connectedPart.transform.position - parentPart.transform.position;
        Vector3 s = connector.transform.localScale;
        s.x = (connectedPart.transform.position - parentPart.transform.position).magnitude / relaxedDistance;
        connector.transform.localScale = s;
    }

    public override void ApplyForce(Vector3[][] stateVector,
        Vector3[] secondDerivativeVector)
    {
        Vector2 parentPosition = new Vector2(stateVector[0][parentPart.id].x, stateVector[0][parentPart.id].y);
        Vector2 connectedPosition = new Vector2(stateVector[0][connectedPart.id].x, stateVector[0][connectedPart.id].y);

        Vector2 toConnected = connectedPosition - parentPosition;
        
        Vector2 direction = toConnected.normalized;
        Vector2 tangent = new Vector2(-direction.y, direction.x);
        Vector2 relaxedVector = relaxedDistance * direction;
        Vector2 forceVector = (toConnected - relaxedVector) * springConstant;
        AddForce(parentPart, forceVector, secondDerivativeVector);
        AddForce(connectedPart, -forceVector, secondDerivativeVector);
        
        float relativeSpeed = Vector2.Dot(direction, (Vector2)stateVector[1][parentPart.id]) + Vector2.Dot(-direction, (Vector2)stateVector[1][connectedPart.id]);
        float tangentSpeed = Vector2.Dot(tangent, stateVector[1][parentPart.id]) +
                             Vector2.Dot(-tangent, stateVector[1][connectedPart.id]);
        
        AddForce(parentPart, -direction * (relativeSpeed * dampingConstant), secondDerivativeVector);
        AddForce(connectedPart, direction * (relativeSpeed * dampingConstant), secondDerivativeVector);
        
        AddForce(parentPart, -tangent * (tangentSpeed * tangentDamping), secondDerivativeVector);
        AddForce(connectedPart, tangent * (tangentSpeed * tangentDamping), secondDerivativeVector);
    }
}
