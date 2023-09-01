using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthographicCameraScaler : MonoBehaviour
{
    public float fixedWidth = 10;

    // Update is called once per frame
    void Update()
    {
        float inverseAspect = 1 / Camera.main.aspect;
        float requiredHeight = fixedWidth * inverseAspect;
        //Camera.main.rect = new Rect(-fixedWidth, -requiredHeight / 2, fixedWidth * 2, requiredHeight);
        Camera.main.orthographicSize = requiredHeight;
    }
}
