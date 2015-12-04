using UnityEngine;
using System.Collections;

/// <summary>
/// UI特效
/// </summary>
public class UIFX : MonoBehaviour 
{
    /// <summary>
    /// 绑定动作
    /// </summary>
    /// <param name="onEnd"></param>
    public void BindAnimator(UIFXListener.OnFXEnd onEnd)
    {
        var timer = gameObject.GetComponentInChildren<DestroyTimer>();
        if (timer != null)
        {
            timer.gameObject.AddComponent<UIFXListener>().onFXEnd = onEnd;
        }
    }
}
