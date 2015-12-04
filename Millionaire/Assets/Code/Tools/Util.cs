using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 工具
/// </summary>
public class Util
{
    #region 查找

    /// <summary>
    /// 查找
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent"></param>
    /// <param name="name"></param>
    /// <param name="includeInactive"></param>
    /// <returns></returns>
    public static GameObject FindGo<T>(GameObject parent, string name, bool includeInactive = true) where T : Component
    {
        if (parent == null)
        {
            return null;
        }

        T t = FindCo<T>(parent.transform, name, includeInactive);

        return (t == null) ? null : t.gameObject;
    }

    /// <summary>
    /// 查找
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="name"></param>
    /// <param name="includeInactive"></param>
    /// <returns></returns>
    public static GameObject FindGo(GameObject parent, string name, bool includeInactive = true)
    {
        if (parent == null)
        {
            return null;
        }

        Transform t = FindCo<Transform>(parent.transform, name, includeInactive);

        return (t == null) ? null : t.gameObject;
    }

    /// <summary>
    /// 查找
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent"></param>
    /// <param name="name"></param>
    /// <param name="includeInactive"></param>
    /// <returns></returns>
    public static T FindCo<T>(GameObject parent, string name, bool includeInactive = true) where T : Component
    {
        if (parent == null)
        {
            return null;
        }

        return FindCo<T>(parent.transform, name, includeInactive);
    }

    /// <summary>
    /// 查找
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent"></param>
    /// <param name="name"></param>
    /// <param name="includeInactive"></param>
    /// <returns></returns>
    public static T FindCo<T>(Transform parent, string name, bool includeInactive = true) where T : Component
    {
        if (parent == null)
        {
            return null;
        }

        foreach (T t in parent.GetComponentsInChildren<T>(includeInactive))
        {
            if (t.name.Equals(name))
            {
                return t;
            }
        }

        return null;
    }

    #endregion

    #region 删除

    /// <summary>
    /// 立即删除所有儿子结点
    /// </summary>
    /// <param name="go"></param>
    public static void DestroyAllChildrenImmediate(GameObject go)
    {
        if (go == null)
        {
            return;
        }

        List<GameObject> list = new List<GameObject>();

        for (int i = 0, imax = go.transform.childCount; i < imax; i++)
        {
            list.Add(go.transform.GetChild(i).gameObject);
        }

        foreach (GameObject g in list)
        {
            UnityEngine.Object.DestroyImmediate(g);
        }
        list.Clear();
    }

    /// <summary>
    /// 删除所有儿子结点
    /// </summary>
    /// <param name="go"></param>
    public static void DestroyAllChildren(GameObject go)
    {
        if (go == null)
        {
            return;
        }

        List<GameObject> list = new List<GameObject>();

        for (int i = 0, imax = go.transform.childCount; i < imax; i++)
        {
            list.Add(go.transform.GetChild(i).gameObject);
        }

        foreach (GameObject g in list)
        {
            UnityEngine.Object.Destroy(g);
        }
        list.Clear();
    }

    /// <summary>
    /// 删除所有儿子结点
    /// </summary>
    /// <param name="comp"></param>
    public static void DestroyAllChildren(Component comp)
    {
        if (comp == null)
        {
            return;
        }
        DestroyAllChildren(comp.gameObject);
    }

    #endregion
}
