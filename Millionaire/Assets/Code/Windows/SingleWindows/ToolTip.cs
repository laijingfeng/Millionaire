using UnityEngine;
using System.Collections;
using System;

public class ToolTip : SingletonWindow<ToolTip>
{
    #region UI变量

    /// <summary>
    /// 窗体
    /// </summary>
    private GameObject m_goWin;

    /// <summary>
    /// <para>关闭按钮</para>
    /// <para>背景</para>
    /// </summary>
    private GameObject m_goCloseButton;

    /// <summary>
    /// 内容
    /// </summary>
    private UILabel m_lbContent;

    #endregion

    #region 逻辑变量

    /// <summary>
    /// 输入数据
    /// </summary>
    public class ToolTipInputData
    {
        public string m_data = string.Empty;
    };

    /// <summary>
    /// 目标
    /// </summary>
    private GameObject m_goTarget;

    #endregion

    void Start()
    {
        m_goWin = Util.FindGo(gameObject, "Win");

        m_goCloseButton = Util.FindGo(gameObject, "CloseButton");
        UIEventListener.Get(m_goCloseButton).onClick += OnClickCloseButton;

        m_lbContent = Util.FindCo<UILabel>(m_goWin, "Content");
    }

    /// <summary>
    /// 关闭
    /// </summary>
    /// <param name="go"></param>
    private void OnClickCloseButton(GameObject go)
    {
        Hide();
    }

    /// <summary>
    /// 设置
    /// </summary>
    /// <param name="goTarget"></param>
    /// <param name="data"></param>
    public static void SetToolTip(GameObject goTarget, string data)
    {
        if (goTarget == null)
        {
            return;
        }

        UIUtil.RemoveClickFunction(goTarget);
        CustomData.Set(goTarget, new ToolTipInputData() { m_data = data });
        UIEventListener.Get(goTarget).onClick += OnClickToolTip;
    }

    /// <summary>
    /// 点击
    /// </summary>
    /// <param name="go"></param>
    private static void OnClickToolTip(GameObject go)
    {
        if (go == null)
        {
            return;
        }

        ToolTip.Instance.ShowToolTip(go);
    }

    /// <summary>
    /// 显示
    /// </summary>
    /// <param name="goTarget">目标</param>
    /// <param name="onCalendarSelected"></param>
    public void ShowToolTip(GameObject goTarget)
    {
        Show();
        m_goTarget = goTarget;

        UIUtil.AdjustPos(m_goTarget, m_goWin);

        Refresh();
    }

    /// <summary>
    /// 刷新
    /// </summary>
    private void Refresh()
    {
        ToolTipInputData data = CustomData.Get(m_goTarget) as ToolTipInputData;
        m_lbContent.text = data.m_data;
    }
}
