using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UIWidgetAlpha : MonoBehaviour
{
    public float alpha = 1f;
    public UIWidget[] widgetList;
    void Start()
    {
        widgetList = GetComponentsInChildren<UIWidget>();
        foreach (UIWidget widget in widgetList)
        {
            widget.alpha = alpha;
        }
    }

    void Update() 
    {
        if (widgetList == null)
            return;

        foreach (UIWidget widget in widgetList)
        {
            widget.alpha = alpha; 
        }
    }
}