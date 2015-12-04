using UnityEngine;
using System.Collections;

/// <summary>
/// 主界面
/// </summary>
public class MainUI : SingletonWindow<MainUI>
{
    /// <summary>
    /// 背景1
    /// </summary>
    private Transform m_tfBg01;
    
    /// <summary>
    /// 背景2
    /// </summary>
    private Transform m_tfBg02;

    void Start()
    {
        m_tfBg01 = Util.FindCo<Transform>(gameObject, "bg01");
        m_tfBg02 = Util.FindCo<Transform>(gameObject, "bg02");
    }

    public override void OnShow()
    {
        base.OnShow();
        StartScroll();
    }

    public override void OnHide()
    {
        base.OnHide();
        StopCoroutine("ScrollBkg");
    }

    /// <summary>
    /// 开始滚动
    /// </summary>
    private void StartScroll()
    {
        Vector3 tmpPos = m_tfBg01.localPosition;
        tmpPos.x = 0.00f;
        m_tfBg01.localPosition = tmpPos;

        tmpPos = m_tfBg02.localPosition;
        tmpPos.x = 958.00f;
        m_tfBg02.localPosition = tmpPos;

        StopCoroutine("ScrollBkg");
        StartCoroutine("ScrollBkg");
    }

    /// <summary>
    /// 滚动
    /// </summary>
    /// <returns></returns>
    private IEnumerator ScrollBkg()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();

            Vector3 tmpPos = m_tfBg01.localPosition;
            tmpPos.x -= 5.00f;
            if (tmpPos.x < -958.00f)
            {
                tmpPos.x = 958.00f;
            }
            m_tfBg01.localPosition = tmpPos;

            tmpPos = m_tfBg02.localPosition;
            tmpPos.x -= 5.00f;
            if (tmpPos.x < -958.00f)
            {
                tmpPos.x = 958.00f;
            }
            m_tfBg02.localPosition = tmpPos;

            if (this.IsVisible == false)
            {
                break;
            }
        }
        yield return null;
    }
}
