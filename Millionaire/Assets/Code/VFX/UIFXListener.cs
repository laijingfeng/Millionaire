using UnityEngine;
using System.Collections;

/// <summary>
/// UI特效监听器
/// </summary>
public class UIFXListener : MonoBehaviour
{
    /// <summary>
    /// 特效结束委托
    /// </summary>
    public delegate void OnFXEnd();

    /// <summary>
    /// 特效结束函数
    /// </summary>
    public OnFXEnd onFXEnd;

    void OnDestroy()
    {
        if (onFXEnd != null)
        {
            onFXEnd();
        }
    }
}

