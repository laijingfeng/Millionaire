﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class SceneEditor : EditorWindow
{
    /// <summary>
    /// 场景同一列判断误差
    /// </summary>
    private const double EPS = 0.5f;

    /// <summary>
    /// 两图重叠部分单位
    /// </summary>
    private const float EPS2 = 0.01f;

    /// <summary>
    /// 显示
    /// </summary>
    [MenuItem("SceneEditor/SceneEditor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(SceneEditor));
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("拼接选中的场景"))
        {
            Process();
        }
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// 进行处理
    /// </summary>
    private void Process()
    {
        if (Begin() == false)
        {
            return;
        }
        DoWork();
    }

    /// <summary>
    /// 场景背景列
    /// </summary>
    private List<SceneItem> m_listSceneItem = new List<SceneItem>();

    /// <summary>
    /// 开始
    /// </summary>
    private bool Begin()
    {
        if (Selection.transforms == null
            || Selection.transforms.Length <= 0)
        {
            EditorUtility.DisplayDialog("提示", "没有选中物体", "确定");
            return false;
        }

        List<SpriteRenderer> sprites = new List<SpriteRenderer>();

        foreach (Transform tf in Selection.transforms)
        {
            sprites.AddRange(tf.GetComponentsInChildren<SpriteRenderer>());
        }

        if (sprites.Count <= 0)
        {
            EditorUtility.DisplayDialog("提示", "没有选中场景", "确定");
            return false;
        }

        sprites.Sort(SortCmp);

        m_listSceneItem.Clear();

        float fZ = sprites[0].transform.position.z;
        int sceneItemCnt = 0;
        int curSceneCellCnt = 0;

        SceneItem sceneItem = new SceneItem();
        m_listSceneItem.Add(sceneItem);

        SceneCell sceneCell = new SceneCell();
        sceneCell.m_tfTransform = sprites[0].transform;
        sceneCell.m_vtPosition = sprites[0].transform.position;
        sceneCell.m_vtSize = sprites[0].sprite.bounds.size;

        m_listSceneItem[sceneItemCnt].m_listSceneCells.Add(sceneCell);

        for (int i = 1, imax = sprites.Count; i < imax; i++)
        {
            sceneCell = new SceneCell();
            sceneCell.m_tfTransform = sprites[i].transform;
            sceneCell.m_vtPosition = sprites[i].transform.position;
            sceneCell.m_vtSize = sprites[i].sprite.bounds.size;
            sceneCell.m_vtPosition.z = fZ;

            if (Mathf.Abs(sceneCell.m_vtPosition.x - m_listSceneItem[sceneItemCnt].m_listSceneCells[curSceneCellCnt].m_vtPosition.x) < EPS)
            {
                m_listSceneItem[sceneItemCnt].m_listSceneCells.Add(sceneCell);
                curSceneCellCnt++;
            }
            else
            {
                sceneItem = new SceneItem();
                sceneItem.m_listSceneCells.Add(sceneCell);
                curSceneCellCnt = 0;

                m_listSceneItem.Add(sceneItem);
                sceneItemCnt++;
            }
        }

        return true;
    }

    /// <summary>
    /// 拼接
    /// </summary>
    private void DoWork()
    {
        SceneCell pre,now;

        for (int i = 0, imax = m_listSceneItem.Count; i < imax; i++)
        {
            for (int j = 0, jmax = m_listSceneItem[i].m_listSceneCells.Count; j < jmax; j++)
            {
                if (i == 0 
                    && j == 0)
                {
                    continue;
                }

                now = m_listSceneItem[i].m_listSceneCells[j];
                
                if (j == 0)
                {
                    pre = m_listSceneItem[i - 1].m_listSceneCells[0];
                    now.m_vtPosition.y = pre.m_vtPosition.y - pre.m_vtSize.y * 0.5f * pre.m_tfTransform.lossyScale.y + now.m_vtSize.y * 0.5f * now.m_tfTransform.lossyScale.y;
                    now.m_vtPosition.x = pre.m_vtPosition.x + pre.m_vtSize.x * 0.5f * pre.m_tfTransform.lossyScale.x + now.m_vtSize.x * 0.5f * now.m_tfTransform.lossyScale.x - EPS2 * now.m_tfTransform.lossyScale.x;
                }
                else
                {
                    pre = m_listSceneItem[i].m_listSceneCells[j - 1];
                    now.m_vtPosition.y = pre.m_vtPosition.y + pre.m_vtSize.y * 0.5f * pre.m_tfTransform.lossyScale.y + now.m_vtSize.y * 0.5f * now.m_tfTransform.lossyScale.y - EPS2 * now.m_tfTransform.lossyScale.y;
                    now.m_vtPosition.x = pre.m_vtPosition.x;
                }

                now.m_tfTransform.position = now.m_vtPosition;
            }
        }
    }

    /// <summary>
    /// 排序方式
    /// </summary>
    /// <param name="sprite1"></param>
    /// <param name="sprite2"></param>
    /// <returns></returns>
    private static int SortCmp(SpriteRenderer sprite1, SpriteRenderer sprite2)
    {
        int iRet = 0;

        if (Mathf.Abs(sprite1.transform.position.x - sprite2.transform.position.x) < EPS)
        {
            Debug.LogError("aaa");
            if (sprite1.transform.position.y >= sprite2.transform.position.y)
            {
                iRet = -1;
            }
            else
            {
                iRet = 1;
            }
        }
        else if (sprite1.transform.position.x < sprite2.transform.position.x)
        {
            iRet = -1;
        }
        else if (sprite1.transform.position.x > sprite2.transform.position.x)
        {
            iRet = 1;
        }

        return iRet;
    }

    /// <summary>
    /// 场景背景小块
    /// </summary>
    public class SceneCell
    {
        public Transform m_tfTransform;
        public Vector3 m_vtSize;
        public Vector3 m_vtPosition;
    }

    /// <summary>
    /// 场景背景列
    /// </summary>
    public class SceneItem
    {
        public List<SceneCell> m_listSceneCells = new List<SceneCell>();
    }
}