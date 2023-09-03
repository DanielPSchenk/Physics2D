using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plot : MonoBehaviour
{
    public int resolution;
    public float drawArea = .95f;
    private LinePlot[] lines;

    private int counter = 0;
    public int updateFrequency = 30;
    private RectTransform rt;

    private RectTransform canvas;

    private void Start()
    {
        Canvas c = GetComponentInParent<Canvas>();
        canvas = c.GetComponent<RectTransform>();
        lines = FindObjectsOfType<LinePlot>();
        rt = GetComponent<RectTransform>();
        
        foreach (var l in lines)
        {
            
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
            
            //view space. Starts at top left corner
            float borderWidth = (1 - drawArea) / 2;
            Vector2 borderOffset = rt.rect.size * borderWidth;
            Vector2 start = new Vector2(rt.rect.xMin, rt.rect.yMin);
            Debug.Log(start);
            start += borderOffset;
            
            start.x = start.x / canvas.rect.width;
            start.y = start.y / canvas.rect.height;
            
            Vector2 drawSize = new Vector2(rt.rect.width / canvas.rect.width, rt.rect.height / canvas.rect.height) * drawArea;
        
            Rect drawRect = new Rect(start, drawSize);
            
            //world space. Starts at image center
            
            float viewHeight = Camera.main.orthographicSize * 2;
            float viewWidth = Camera.main.aspect * viewHeight;
            float graphWidth = viewWidth * drawRect.width;
            float graphHeight = viewHeight * drawRect.height;

            float viewStartX = -graphWidth * .5f;
            //Debug.Log(.5f - drawRect.yMin - drawRect.height);
            float viewStartY = graphHeight * (.5f - drawRect.yMin - drawRect.height);
            Rect worldSpaceRect = new Rect(new Vector2(viewStartX, viewStartY), new Vector2(graphWidth, graphHeight));
            

            foreach (var l in lines)
            {
                l.setArea(worldSpaceRect);
                l.SetPlotBounds(minimum, maximum);
                l.PlotUpdate();    
            }
            
        }
    }
}
