using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// <para>用户数据</para>
/// <para>可设置多组数据</para>
/// </summary>
public class CustomData : MonoBehaviour
{
    /// <summary>
    /// 默认tag
    /// </summary>
    private const string DEFAULT_TAG = "NoTag";

    /// <summary>
    /// 存储的数据
    /// </summary>
    private Dictionary<string, System.Object> m_data = new Dictionary<string, object>();

    /// <summary>
    /// 获取数据
    /// </summary>
    /// <param name="go"></param>
    /// <param name="tag">标签</param>
    /// <returns></returns>
    public static System.Object Get(GameObject go, string tag = DEFAULT_TAG)
    {
        if (go == null)
        {
            return null;
        }

        CustomData customdata = go.GetComponent<CustomData>();

        if (customdata == null)
        {
            return null;
        }

        if (customdata.m_data.ContainsKey(tag) == false)
        {
            return null;
        }

        System.Object data = null;
        customdata.m_data.TryGetValue(tag, out data);

        return data;
    }

    /// <summary>
    /// 获取数据
    /// </summary>
    /// <param name="comp"></param>
    /// <param name="tag">标签</param>
    /// <returns></returns>
    public static System.Object Get(Component comp, string tag = DEFAULT_TAG)
    {
        if (comp == null)
        {
            return null;
        }
        return Get(comp.gameObject);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="go"></param>
    /// <param name="data"></param>
    /// <param name="tag">标签</param>
    public static void Set(GameObject go, System.Object data, string tag = DEFAULT_TAG)
    {
        if (go == null)
        {
            return;
        }

        CustomData customdata = go.GetComponent<CustomData>();

        if (customdata == null)
        {
            customdata = go.AddComponent<CustomData>();
        }

        if (customdata.m_data.ContainsKey(tag) == false)
        {
            customdata.m_data.Add(tag, data);
        }
        else
        {
            customdata.m_data[tag] = data;
        }
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="comp"></param>
    /// <param name="data"></param>
    /// <param name="tag">标签</param>
    static public void Set(Component comp, System.Object data, string tag = DEFAULT_TAG)
    {
        if (comp == null)
        {
            return;
        }

        Set(comp.gameObject, data, tag);
    }
}
