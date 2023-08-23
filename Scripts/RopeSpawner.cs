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
        for (int i = 0; i < length; i++)
        {
            SpringDamperEffector nPart = Instantiate(prefab);
            nPart.transform.position = new Vector3(start.transform.position.x + .2f, start.transform.position.y - 2,
                start.transform.position.z);
            nPart.connectedPart = start.GetComponent<PhysicsPart>();
            nPart.GetComponent<PhysicsPart>().SetPosition();
            start = nPart;
        }
    }

    
}
