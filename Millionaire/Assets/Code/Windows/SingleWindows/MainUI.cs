using UnityEngine;
using System.Collections;

/// <summary>
/// 主界面
/// </summary>
public class MainUI : SingletonWindow<MainUI>
{
    /// <summary>
    /// 背景
    /// </summary>
    private Transform[] m_tfBg = new Transform[2];
    
    void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            m_tfBg[i] = Util.FindCo<Transform>(gameObject, "bg0" + i);
        }
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
        Vector3 tmpPos;
        for (int i = 0; i < 1; i++)
        {
            tmpPos = m_tfBg[i].localPosition;
            tmpPos.x = i * 2.87f;
        }

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

            Vector3 tmpPos;
            if (m_tfBg[0].localPosition.x > m_tfBg[1].localPosition.x)
            {
                Transform tmpTf = m_tfBg[0];
                m_tfBg[0] = m_tfBg[1];
                m_tfBg[1] = tmpTf;
            }

            tmpPos = m_tfBg[0].localPosition;

            tmpPos.x -= 0.02f;

            if (tmpPos.x < -2.87f)
            {
                tmpPos = m_tfBg[1].localPosition;
                tmpPos.x -= 0.02f;

                m_tfBg[1].localPosition = tmpPos;
                tmpPos.x += 2.87f;
                m_tfBg[0].localPosition = tmpPos;
            }
            else
            {
                m_tfBg[0].localPosition = tmpPos;
                tmpPos.x += 2.87f;
                m_tfBg[1].localPosition = tmpPos;
            }

            if (this.IsVisible == false)
            {
                break;
            }
        }
        yield return null;
    }
}
