using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// UI特效控制器
/// </summary>
public class UIFXController : MonoBehaviour 
{
    /// <summary>
    /// UI特效列表
    /// </summary>
    public List<UIFX> fxList = new List<UIFX>();

    /// <summary>
    /// 播放模式
    /// </summary>
    public UIUtil.EFX_PLAY_MODE mode = UIUtil.EFX_PLAY_MODE.LOOP;

    /// <summary>
    /// 特效名
    /// </summary>
    public string fxName;
}
