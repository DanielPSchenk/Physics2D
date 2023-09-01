using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    public int resolution;
    public float drawArea = .95f;
    private LinePlot[] lines;

    private int counter = 0;
    public int updateFrequency = 30;

    private void Start()
    {
        lines = FindObjectsOfType<LinePlot>();
        RectTransform rt = GetComponent<RectTransform>();
        float borderWidth = (1 - drawArea) / 2;
        Vector2 borderOffset = new Vector2(rt.rect.width * borderWidth, rt.rect.height * borderWidth);
        Vector2 drawSize = new Vector2(rt.rect.width, rt.rect.height) * drawArea;
        
        Rect drawRect = new Rect(rt.rect.min + borderOffset, drawSize);
        foreach (var l in lines)
        {
            l.setArea(drawRect);
            l.SetResolution(resolution);
        }
    }

    private void FixedUpdate()
    {
        counter = (counter + 1) % updateFrequency;
        if (counter == 0)
        {
            float minimum = float.MaxValue;
            float maximum = float.MinValue;
            foreach (var l in lines)
            {
                l.ValueUpdate();
                if (l.GetMaximumValue() > maximum) maximum = l.GetMaximumValue();
                if (l.GetMinimumValue() < minimum) minimum = l.GetMinimumValue();
            }

            if (minimum == maximum)
            {
                minimum = 0;
                maximum = 1;
            }

            foreach (var l in lines)
            {
                l.SetPlotBounds(minimum, maximum);
                l.PlotUpdate();    
            }
            
        }
    }
}
