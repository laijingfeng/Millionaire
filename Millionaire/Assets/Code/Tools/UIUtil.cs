using UnityEngine;
using System.Collections;
using System;

public class UIUtil
{
    #region 特效相关

    /// <summary>
    /// 特效播放模式
    /// </summary>
    public enum EFX_PLAY_MODE
    {
        /// <summary>
        /// 循环播放
        /// </summary>
        LOOP = 0,

        /// <summary>
        /// 播放一次且排他
        /// </summary>
        ONCE_EXCLUSIVE,

        /// <summary>
        /// 播放一次不排它
        /// </summary>
        ONCE_NONEXCLUSIVE,
    }

    /// <summary>
    /// 播放特效
    /// </summary>
    /// <param name="go">挂载点</param>
    /// <param name="name">特效名</param>
    /// <param name="mode">播放模式</param>
    /// <param name="onEnd">特效播放完事件</param>
    public static void PlayFX(GameObject go, string name, GameObject VV, EFX_PLAY_MODE mode, UIFXListener.OnFXEnd onEnd = null)
    {
        if (go == null)
        {
            return;
        }

        GameObject uiFxGO = null;

        UIFXController fxc = go.GetComponent<UIFXController>();
        if (fxc == null)
        {
            fxc = go.AddComponent<UIFXController>();
            fxc.mode = mode;
            fxc.fxName = name;
        }
        else if (fxc.fxList.Count > 0)
        {
            switch (fxc.mode)
            {
                case EFX_PLAY_MODE.LOOP:
                    {
                        //如果是循环特效则返回 (仅存在一个)
                        return;
                    }
                    break;
                case EFX_PLAY_MODE.ONCE_EXCLUSIVE:
                    {
                        StopFX(go);
                    }
                    break;
                case EFX_PLAY_MODE.ONCE_NONEXCLUSIVE:
                    {
                        foreach (UIFX fx in fxc.fxList)
                        {
                            GameObject goUIFx = fx.gameObject;
                            if (goUIFx.transform.childCount == 0)
                            {
                                uiFxGO = goUIFx;
                                break;
                            }
                        }
                    }
                    break;
            }
        }

        if (uiFxGO == null)
        {
            uiFxGO = new GameObject("UIFxGO");
            uiFxGO.transform.parent = go.transform;

            uiFxGO.transform.localPosition = new Vector3(0, 0, -15);

            if (go.GetComponent<UISprite>() != null
                || go.GetComponent<UITexture>() != null)
            {
                uiFxGO.transform.parent = go.transform.parent;
            }
            uiFxGO.transform.localScale = new Vector3(107, 107, 107);

            UIFX uiFx = uiFxGO.AddComponent<UIFX>();
            fxc.fxList.Add(uiFx);
        }

        OnFXLoaded(uiFxGO, VV);

        if (onEnd != null)
        {
            UIFX fx = uiFxGO.GetComponent<UIFX>();
            if (fx == null)
            {
                Debug.LogError("fx 为空！！！");
            }
            else
            {
                fx.BindAnimator(onEnd);
            }
        }
    }

    /// <summary>
    /// 特效加载完成
    /// </summary>
    /// <param name="res"></param>
    private static void OnFXLoaded(GameObject uiFxGO, GameObject VV)
    {
        if (uiFxGO == null)
        {
            return;
        }

        GameObject fxGO = UnityEngine.Object.Instantiate(VV) as GameObject;

        fxGO.transform.parent = uiFxGO.transform;
        fxGO.transform.localPosition = Vector3.zero;
        fxGO.transform.localScale = Vector3.one;

        SetLayerRecursively(uiFxGO, LayerMask.NameToLayer("UI"));
    }

    public static void SetLayerRecursively(GameObject go, int newLayer)
    {
        if (null == go)
        {
            return;
        }

        go.layer = newLayer;

        foreach (Transform child in go.transform)
        {
            if (null == child)
            {
                continue;
            }

            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    /// <summary>
    /// 停止特效
    /// </summary>
    /// <param name="go">挂载点</param>
    public static void StopFX(GameObject go)
    {
        if (go == null)
        {
            return;
        }

        UIFXController fxc = go.GetComponent<UIFXController>();
        if (fxc == null)
        {
            return;
        }

        foreach (UIFX fx in fxc.fxList)
        {
            if (fx != null)
            {
                GameObject.Destroy(fx.gameObject);
            }
        }
        fxc.fxList.Clear();

        //GameObject.Destroy(fxc); 修复了一个bug，删除特效控制器会导致多个特效管理时出现问题
    }

    #endregion

    /// <summary>
    /// 去除委托的点击函数
    /// </summary>
    /// <param name="go"></param>
    public static void RemoveClickFunction(GameObject go)
    {
        if (go == null)
        {
            return;
        }

        UIEventListener eventListener = go.GetComponent<UIEventListener>();
        if (eventListener == null)
        {
            return;
        }

        if (eventListener.onClick == null)
        {
            return;
        }

        Delegate[] invList = eventListener.onClick.GetInvocationList();
        foreach (UIEventListener.VoidDelegate handler in invList)
        {
            go.GetComponent<UIEventListener>().onClick -= handler;
        }
    }

    /// <summary>
    /// 调整位置
    /// </summary>
    /// <param name="goTarget">目标位置物体</param>
    /// <param name="goWin">浮动窗体</param>
    public static bool AdjustPos(GameObject goTarget, GameObject goWin)
    {
        if (goTarget == null
            || goWin == null)
        {
            return false;
        }

        float rate = UIRoot.GetPixelSizeAdjustment(goTarget);

        Vector2 vtMaxBound = new Vector2(Screen.width, Screen.height) * rate * 0.5f;

        //目标的包围大小
        Bounds bdTarget = NGUIMath.CalculateRelativeWidgetBounds(goTarget.transform.GetComponentInParent<UIRoot>().transform, goTarget.transform);

        //浮动窗体的大小
        Bounds bdWin = NGUIMath.CalculateRelativeWidgetBounds(goWin.transform.GetComponentInParent<UIRoot>().transform, goWin.transform);

        //确定的目标坐标
        float ftX = 0f, ftY = 0f;

        //确定的目标坐标下最边界
        Vector3 vtBian = Vector3.zero;

        //我的中心在目标中心的什么方位（共有9个：左下、右上、右下、左上、下、上、右、左、中心）
        //x:1右 0同 -1左
        //y:1上 0同 -1下
        //数组顺序前2个和最后1个不要动
        int[,] itAdjustType = new int[9, 2] { { -1, -1 }, { 1, 1 }, { 1, -1 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, 0 }, { -1, 0 }, { 0, 0 } };

        for (int i = 0, imax = itAdjustType.Length; i < imax; i++)
        {
            ftX = bdTarget.center.x + itAdjustType[i, 0] * bdTarget.extents.x + itAdjustType[i, 0] * bdWin.extents.x;
            ftY = bdTarget.center.y + itAdjustType[i, 1] * bdTarget.extents.y + itAdjustType[i, 1] * bdWin.extents.y;

            int j;
            for (j = 0; j < 2; j++)
            {
                vtBian.x = ftX + itAdjustType[j, 0] * bdWin.extents.x;
                vtBian.y = ftY + itAdjustType[j, 1] * bdWin.extents.y;

                if (Mathf.Abs(vtBian.x) > vtMaxBound.x
                    || Mathf.Abs(vtBian.y) > vtMaxBound.y)
                {
                    break;
                }
            }

            if (j == 2)
            {
                break;
            }
        }

        goWin.transform.localPosition = new Vector3(ftX, ftY);

        return true;
    }
}
