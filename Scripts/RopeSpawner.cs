using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSpawner : MonoBehaviour
{
    public SpringDamperEffector start;

    public SpringDamperEffector prefab;

    public int length = 100;
    
    
    // Start is called before the first frame update
    void Start()
    {
        PhysicsManager manager = Transform.FindObjectOfType<PhysicsManager>();
        for (int i = 0; i < 100; i++)
        {
            SpringDamperEffector nPart = Instantiate(prefab);
            nPart.transform.position = new Vector3(start.transform.position.x, start.transform.position.y - 1,
                start.transform.position.z);
            nPart.connectedPart = start.GetComponent<PhysicsPart>();
            start = nPart;
        }
    }

    
}
