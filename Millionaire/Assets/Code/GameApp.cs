using UnityEngine;
using System.Collections;

public class GameApp : MonoBehaviour
{
    void Awake()
    {
        LoadSingleWindow();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void Start()
    {
        StopCoroutine("GameStart");
        StartCoroutine("GameStart");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)
            || Input.GetKeyDown(KeyCode.Home))
        {
            MessageBox.Instance.Show("Quit");
            Application.Quit();
        }

        //表格测试
        if (Input.GetKeyDown(KeyCode.B))
        {
            Table.SERVER_NOTICE_MSG serverNoticeMsgTable = null;
            if (ServerNoticeMsgTableManager.Instance.TryGetValue(1000, out serverNoticeMsgTable) == true)
            {
                Debug.LogError("C=" + serverNoticeMsgTable.content);
            }
            else
            {
                Debug.LogError("NULL");
            }
        }
    }

    /// <summary>
    /// 游戏开始
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameStart()
    {
        //等候一帧，让windows的Start执行完，不然可能窗体的变量未绑定
        yield return new WaitForEndOfFrame();
        GameInit();
        yield break;
    }

    /// <summary>
    /// 游戏初始化
    /// </summary>
    private void GameInit()
    {
        //TableLoader.Instance.LoadTables();
        MainUI.Instance.Show();
    }

    /// <summary>
    /// <para>加载窗体</para>
    /// <para>没有手动放到Hierarchy的窗体都加载到2DUICamera下</para>
    /// </summary>
    private void LoadSingleWindow()
    {
        GameObject goUICamera = Util.FindGo(gameObject, "2DUICamera");
        Object[] wins = Resources.LoadAll("SingleWindows");
        foreach (Object obj in wins)
        {
            GameObject go = GameObject.Find(obj.name);
            if(go == null)
            {
                go = Object.Instantiate(obj) as GameObject;
                go.name = go.name.Replace("(Clone)", "");
                go.transform.parent = goUICamera.transform;
            }
        }
    }
}
