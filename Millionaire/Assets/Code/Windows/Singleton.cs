using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 单例类
/// </summary>
/// <typeparam name="T"></typeparam>
[System.Reflection.Obfuscation(ApplyToMembers = true, Exclude = true, Feature = "renaming")]
public class Singleton<T>
{
    /// <summary>
    /// 单例
    /// </summary>
    private static T m_instance = default(T);

    /// <summary>
    /// 单例
    /// </summary>
    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = (T)Activator.CreateInstance(typeof(T), true);
            }
            return m_instance;
        }
    }
}

/// <summary>
/// <para>单例Mono</para>
/// <para>要么悬挂点要和脚本同名，要么挂在Client上或其子孙上</para>
/// <para>都没有则新挂在Client上</para>
/// </summary>
[System.Reflection.Obfuscation(ApplyToMembers = true, Exclude = true, Feature = "renaming")]
public class SingletonMono<T> : MonoBehaviour where T : UnityEngine.Component
{
    /// <summary>
    /// 单例
    /// </summary>
    private static T m_instance = default(T);

    public virtual void Awake()
    {
        //一来就在Hierarchy的active对象直接初始化
        m_instance = (T)(System.Object)(this);
    }
   
    /// <summary>
    /// 单例
    /// </summary>
    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                //找同名的GameObject
                GameObject root = GameObject.Find((typeof(T).Name));

                if (root != null)
                {
                    m_instance = root.GetComponent<T>();
                }
                else
                {
                    //找到Client
                    root = GameObject.Find("/Client");
                    
                    //没有Client则新建单例
                    if (root == null)
                    {
                        root = new GameObject(typeof(T).Name);
                    }

                    //找脚本
                    T[] list = root.GetComponentsInChildren<T>(true);

                    if (list != null 
                        && list.Length != 0)
                    {
                        m_instance = list[0];
                    }
                    else
                    {
                        m_instance = root.AddComponent<T>();
                    }
                }
            }

            return m_instance;
        }
    }
}
