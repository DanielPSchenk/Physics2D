using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LinePlot : MonoBehaviour
{
    private LineRenderer line;
    private int resolution;
    private int now;
    private float[] values;
    private Rect area;

    private Vector3[] positions;

    public Observer observer;

    private float minimum, maximum;
    
    private void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    public void SetResolution(int resolution)
    {
        this.resolution = resolution;
        values = new float[resolution];
        positions = new Vector3[resolution];
    }

    public void ValueUpdate()
    {
        values[now] = observer.GetValue();
        now = (now + 1) % resolution;
    }

    public void SetPlotBounds(float min, float max)
    {
        minimum = min;
        maximum = max;
    }

    public void PlotUpdate()
    {
        float range = maximum - minimum;
        
        for (int i = 0; i < resolution; i++)
        {
            int index = (i + now) % resolution;
            float mappedValue = (values[index] - minimum) / range * area.height;
            float progress = i / (float)resolution;
            Vector2 worldSpacePosition = new Vector2(progress * area.width + area.xMin, area.yMin + mappedValue);
            
            positions[i] = worldSpacePosition;

        }
        //Debug.Log(area.width + area.xMin);
        line.SetPositions(positions);
        line.positionCount = resolution;
    }

    public float GetMinimumValue()
    {
        return values.Min();
    }

    public float GetMaximumValue()
    {
        return values.Max();
    }
    

    public void setArea(Rect area)
    {
        //Debug.Log("setting area");
        this.area = area;
    }
}
