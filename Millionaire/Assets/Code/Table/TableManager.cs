﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.ComponentModel;

public class TableLoader : Singleton<TableLoader>
{
    /// <summary>
    /// 表格描述列表
    /// </summary>
    public List<TableDesc> tableDescList = new List<TableDesc>
    {
         new TableDesc("ServerNoticeMsg", 0, "SERVER_NOTICE_MSG", "server_notice_msg", "ServerNoticeMsg"),
         new TableDesc("ActivityAward",0,"ACTIVITY_AWARD","activity_award","Activity"),//活跃度奖励
    };

    /// <summary>
    /// 表格描述
    /// </summary>
    public class TableDesc
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="tableName">表名</param>
        public TableDesc(string tableName)
        {
            this.tableName = tableName;
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="sheetIndex">Excel表页签ID，0开始</param>
        /// <param name="proto_message_name">proto消息名</param>
        /// <param name="outFileName">输出二进制文件名</param>
        /// <param name="excelName">Excel表名</param>
        public TableDesc(string tableName, int sheetIndex, string proto_message_name, string outFileName, string excelName)
        {
            this.tableName = tableName;
            this.excelName = excelName;
            this.sheetIndex = sheetIndex;
            this.proto_message_name = proto_message_name;
            this.outFileName = outFileName;
        }

        /// <summary>
        /// 表名
        /// </summary>
        public string tableName;

        /// <summary>
        /// Excel表名
        /// </summary>
        public string excelName;

        /// <summary>
        /// Excel表页签ID，0开始
        /// </summary>
        public int sheetIndex;

        /// <summary>
        /// proto消息名
        /// </summary>
        public string proto_message_name;

        /// <summary>
        /// 输出二进制文件名
        /// </summary>
        public string outFileName;
    }

    public TableLoader() { }

    /// <summary>
    /// 加载所有表格
    /// </summary>
    public void LoadTables()
    {
        foreach (TableDesc desc in tableDescList)
        {
            string name = desc.tableName.ToLower();
            if (!string.IsNullOrEmpty(desc.outFileName))
            {
                name = desc.outFileName;
            }

            Resource res = ResourceManager.Instance.LoadResource("Table/" + name + ".bytes", true, false, false, true);

            string tableMgrName = desc.tableName + "TableManager";

            Type type = Type.GetType(tableMgrName);
            if (type == null)
            {
                Debug.LogError(string.Format("{0} is Not Defined!", tableMgrName));
                continue;
            }

            PropertyInfo pinfo = null;
            while (type != null)
            {
                pinfo = type.GetProperty("Instance");

                if (pinfo != null)
                {
                    break;
                }

                type = type.BaseType;
            }

            if (pinfo == null)
            {
                continue;
            }

            MethodInfo instMethod = pinfo.GetGetMethod();
            if (instMethod == null)
            {
                continue;
            }

            System.Object tblMgrInst = instMethod.Invoke(null, null);
            if (tblMgrInst == null)
            {
                continue;
            }

            Delegate dele = Delegate.CreateDelegate(typeof(Resource.OnLoaded), tblMgrInst, "OnResourceLoaded");
            res.onLoaded += (Resource.OnLoaded)dele;
        }
    }
}

/// <summary>
/// 表格管理
/// </summary>
/// <typeparam name="TableArrayT">表组</typeparam>
/// <typeparam name="T">表</typeparam>
/// <typeparam name="K">键值</typeparam>
/// <typeparam name="T_1">具体表管理器类名</typeparam>
public abstract class TableManager<TableArrayT, T, K, T_1> : Singleton<T_1>, IEnumerable
{
    /// <summary>
    /// 表组
    /// </summary>
    public TableArrayT array;

    /// <summary>
    /// 键
    /// </summary>
    public K key;

    /// <summary>
    /// 数据
    /// </summary>
    public readonly Dictionary<K, T> dic = new Dictionary<K, T>();

    /// <summary>
    /// 获得枚举器
    /// </summary>
    /// <returns></returns>
    public IEnumerator GetEnumerator()
    {
        return dic.GetEnumerator();
    }

    /// <summary>
    /// 增加表
    /// </summary>
    /// <param name="table"></param>
    public void AddTable(T table)
    {
        K key = GetKey(table);

        //服务器允许ID为0的KEY， 暂时注掉了
        //if (typeof(K) == typeof(uint))
        //{
        //    if ((uint)(object)key == 0)
        //    {
        //        return;
        //    }
        //}

        if (dic.ContainsKey(key))
        {
            Debug.LogError(string.Format("{0}'s key {1} exist!", array, key));
        }
        else
        {
            dic.Add(key, table);
        }

        PostProcess(table);
    }

    /// <summary>
    /// 获取键值
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    public abstract K GetKey(T table);

    /// <summary>
    /// 查表
    /// </summary>
    /// <param name="key">键值</param>
    /// <param name="tbl">返回的表</param>
    /// <param name="bShowErrLog">是否显示查表失败的错误日志</param>
    /// <returns></returns>
    public virtual bool TryGetValue(K key, out T tbl, bool bShowErrLog = true)
    {
        if (!dic.TryGetValue(key, out tbl))
        {
            if (typeof(K) == typeof(long))
            {
                uint subKey = (uint)(Convert.ToInt64(key) & uint.MaxValue);
                uint id = (uint)(Convert.ToInt64(key) >> 32);

                if (bShowErrLog)
                {
                    Debug.LogError(string.Format("{0} 表格中没有 id={1} sub_key={1} 的数据！", typeof(T), subKey));
                }
            }
            else
            {
                if (bShowErrLog)
                {
                    Debug.LogError(string.Format("{0} 表格中没有 id={1} 的数据！", typeof(T)));
                }
            }

            return false;
        }

        return true;
    }

    /// <summary>
    /// 处理完一行数据
    /// </summary>
    /// <param name="table"></param>
    protected virtual void PostProcess(T table) { }

    /// <summary>
    /// 处理完所有行数据
    /// </summary>
    protected virtual void OnAllTablesLoaded() { }

    /// <summary>
    /// 表格加载成功回调
    /// </summary>
    /// <param name="res"></param>
    [System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
    public void OnResourceLoaded(Resource res)
    {
        byte[] raw_data = res.WWW.bytes;
        if (raw_data[1] != 'B' || raw_data[2] != 'L')
        {
            //解密数据
            for (int i = 1; i < raw_data.Length; ++i)
            {
                byte c0 = raw_data[i - 1];
                byte c1 = raw_data[i];
                raw_data[i] = (byte)((((c1 + 0x3E) ^ 0x4A ^ c0) + 0x33) ^ 0x1A);
            }
        }

        byte[] data = new byte[raw_data.Length - 3];
        for (int i = 0, imax = raw_data.Length - 3; i < imax; data[i] = raw_data[i + 3], ++i) ;

        using (MemoryStream stream = new MemoryStream(data))
        {
            array = ProtoBuf.Serializer.Deserialize<TableArrayT>(stream);

            System.Type type = array.GetType();
            PropertyInfo pinfo = type.GetProperty("rows");
            if (pinfo != null)
            {
                MethodInfo mInfo = pinfo.GetGetMethod();
                if (mInfo != null)
                {
                    List<T> list = mInfo.Invoke(array, null) as List<T>;
                    if (list != null)
                    {
                        foreach (T table in list)
                        {
                            AddTable(table);
                        }
                    }
                }

                OnAllTablesLoaded();
            }
            else
            {
                Debug.LogError(string.Format("{0} does not has rows{1} exist!", array, key));
            }
        }
    }
}

/// <summary>
/// 服务器通知消息
/// </summary>
[System.Reflection.Obfuscation(ApplyToMembers = false, Exclude = true, Feature = "renaming")]
public class ServerNoticeMsgTableManager : TableManager<Table.SERVER_NOTICE_MSG_ARRAY, Table.SERVER_NOTICE_MSG, uint, ServerNoticeMsgTableManager>
{
    public override uint GetKey(Table.SERVER_NOTICE_MSG table)
    {
        return table.id;
    }
}

/// <summary>
/// 活跃度奖励表
/// </summary>
[System.Reflection.Obfuscation(ApplyToMembers = false, Exclude = true, Feature = "renaming")]
public class ActivityAwardTableManager : TableManager<Table.ACTIVITY_AWARD_ARRAY, Table.ACTIVITY_AWARD, int, ActivityAwardTableManager>
{
    public override int GetKey(Table.ACTIVITY_AWARD table)
    {
        return table.score;
    }
}