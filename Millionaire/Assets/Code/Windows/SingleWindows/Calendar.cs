using UnityEngine;
using System.Collections;
using System;

public class Calendar : SingletonWindow<Calendar>
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
    /// 月减
    /// </summary>
    private GameObject m_goMonthLeft;

    /// <summary>
    /// 月加
    /// </summary>
    private GameObject m_goMonthRight;

    /// <summary>
    /// 年减
    /// </summary>
    private GameObject m_goYearLeft;

    /// <summary>
    /// 年加
    /// </summary>
    private GameObject m_goYearRight;

    /// <summary>
    /// 日期预设
    /// </summary>
    private GameObject m_goDatePrefab;

    /// <summary>
    /// 日期表
    /// </summary>
    private UITable m_taTable;

    /// <summary>
    /// 年月
    /// </summary>
    private UILabel m_lbYearMonth;

    #endregion

    #region 逻辑变量

    /// <summary>
    /// 选中日期回调
    /// </summary>
    public delegate void OnCalendarSelected(CalendarReturnData data);

    /// <summary>
    /// 选中日期回调
    /// </summary>
    private OnCalendarSelected m_onCalendarSelected;

    /// <summary>
    /// 日历输入数据
    /// </summary>
    public class CalendarInputData
    {
        public Calendar.OnCalendarSelected onCalendarSelected;
    };

    /// <summary>
    /// 日历返回数据
    /// </summary>
    public class CalendarReturnData
    {
        public string data;
    };

    /// <summary>
    /// 目标
    /// </summary>
    private GameObject m_goTarget;

    /// <summary>
    /// 年
    /// </summary>
    private int m_iYear;

    /// <summary>
    /// 月
    /// </summary>
    private int m_iMonth;

    /// <summary>
    /// 上次选中的
    /// </summary>
    private GameObject m_goLastSelected;

    #endregion

    void Start()
    {
        m_goWin = Util.FindGo(gameObject, "Win");
        m_goCloseButton = Util.FindGo(gameObject, "CloseButton");
        UIEventListener.Get(m_goCloseButton).onClick += OnClickCloseButton;
        m_taTable = Util.FindCo<UITable>(gameObject, "Table");
        m_goDatePrefab = Util.FindGo(gameObject, "DatePrefab");
        m_goDatePrefab.SetActive(false);

        m_goMonthLeft = Util.FindGo(gameObject, "MonthLeft");
        m_goMonthRight = Util.FindGo(gameObject, "MonthRight");
        m_goYearLeft = Util.FindGo(gameObject, "YearLeft");
        m_goYearRight = Util.FindGo(gameObject, "YearRight");

        CustomData.Set(m_goMonthLeft, -1);
        UIEventListener.Get(m_goMonthLeft).onClick += OnClickDateControl;
        CustomData.Set(m_goMonthRight, 1);
        UIEventListener.Get(m_goMonthRight).onClick += OnClickDateControl;
        CustomData.Set(m_goYearLeft, -2);
        UIEventListener.Get(m_goYearLeft).onClick += OnClickDateControl;
        CustomData.Set(m_goYearRight, 2);
        UIEventListener.Get(m_goYearRight).onClick += OnClickDateControl;

        m_lbYearMonth = Util.FindCo<UILabel>(gameObject, "YearMonth");
    }

    /// <summary>
    /// 日期点击
    /// </summary>
    /// <param name="go"></param>
    private void OnClickDateControl(GameObject go)
    {
        int data = (int)CustomData.Get(go);
        switch (data)
        {
            case -1:
                {
                    if (m_iMonth > 1)
                    {
                        m_iMonth--;
                    }
                    else
                    {
                        m_iMonth = 12;
                        m_iYear--;
                    }
                }
                break;
            case 1:
                {
                    if (m_iMonth < 12)
                    {
                        m_iMonth++;
                    }
                    else
                    {
                        m_iMonth = 1;
                        m_iYear++;
                    }
                }
                break;
            case -2:
                {
                    m_iYear--;
                }
                break;
            case 2:
                {
                    m_iYear++;
                }
                break;
        }
        Refresh();
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
    /// 设置日历
    /// </summary>
    /// <param name="goTarget"></param>
    /// <param name="onCalendarSelected"></param>
    public static void SetCalendar(GameObject goTarget, OnCalendarSelected onCalendarSelected)
    {
        if (goTarget == null)
        {
            return;
        }

        UIUtil.RemoveClickFunction(goTarget);
        CustomData.Set(goTarget, new CalendarInputData() { onCalendarSelected = onCalendarSelected });
        UIEventListener.Get(goTarget).onClick += OnClickCalendar;
    }

    /// <summary>
    /// 日历点击
    /// </summary>
    /// <param name="go"></param>
    private static void OnClickCalendar(GameObject go)
    {
        if (go == null)
        {
            return;
        }

        CalendarInputData calendarData = CustomData.Get(go) as CalendarInputData;
        Calendar.Instance.ShowCalendar(go, calendarData.onCalendarSelected);
    }

    /// <summary>
    /// 点击日期
    /// </summary>
    private void OnClickDate(GameObject go)
    {
        if (go == null)
        {
            return;
        }

        if (m_goLastSelected != null)
        {
            UISprite st1 = Util.FindCo<UISprite>(m_goLastSelected, "Selected");
            st1.gameObject.SetActive(false);
        }

        UISprite st2 = Util.FindCo<UISprite>(go, "Selected");
        st2.gameObject.SetActive(true);

        m_goLastSelected = go;

        int day = (int)CustomData.Get(go);
        if (m_onCalendarSelected != null)
        {
            m_onCalendarSelected(new CalendarReturnData() { data = new DateTime(m_iYear, m_iMonth, day, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss") });
        }
    }

    /// <summary>
    /// 显示日历
    /// </summary>
    /// <param name="goTarget">目标</param>
    /// <param name="onCalendarSelected"></param>
    public void ShowCalendar(GameObject goTarget, OnCalendarSelected onCalendarSelected)
    {
        Show();
        m_goTarget = goTarget;
        m_onCalendarSelected = onCalendarSelected;

        UIUtil.AdjustPos(m_goTarget, m_goWin);

        m_iYear = DateTime.Now.Year;
        m_iMonth = DateTime.Now.Month;

        Refresh();
    }

    /// <summary>
    /// 获得星期数字
    /// </summary>
    /// <param name="week"></param>
    /// <returns></returns>
    private int GetWeekNum(DayOfWeek week)
    {
        int ret = 0;
        switch (week)
        {
            case DayOfWeek.Monday:
                {
                    ret = 1;
                }
                break;
            case DayOfWeek.Tuesday:
                {
                    ret = 2;
                }
                break;
            case DayOfWeek.Wednesday:
                {
                    ret = 3;
                }
                break;
            case DayOfWeek.Thursday:
                {
                    ret = 4;
                }
                break;
            case DayOfWeek.Friday:
                {
                    ret = 5;
                }
                break;
            case DayOfWeek.Saturday:
                {
                    ret = 6;
                }
                break;
            case DayOfWeek.Sunday:
                {
                    ret = 7;
                }
                break;
        }
        return ret;
    }

    /// <summary>
    /// 获得当月天数
    /// </summary>
    /// <returns></returns>
    private int GetDaysOfCurrentMonth()
    {
        int[] arr = new int[12] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        if (m_iYear % 400 == 0
            || (m_iYear % 4 == 0 && m_iYear % 100 != 0))
        {
            arr[1] = 29;
        }
        return arr[m_iMonth - 1];
    }

    /// <summary>
    /// 刷新
    /// </summary>
    private void Refresh()
    {
        m_lbYearMonth.text = m_iYear + "." + m_iMonth;

        DateTime now = DateTime.Now;
        DateTime data = new DateTime(m_iYear, m_iMonth, 1);
        int week = GetWeekNum(data.DayOfWeek);
        int days = GetDaysOfCurrentMonth();

        m_goLastSelected = null;

        Util.DestroyAllChildrenImmediate(m_taTable.gameObject);

        for (int i = 0; i < week - 1; i++)
        {
            GameObject date = NGUITools.AddChild(m_taTable.gameObject, m_goDatePrefab);
            UILabel Label = Util.FindCo<UILabel>(date, "Date");
            UISprite NonSelected = Util.FindCo<UISprite>(date, "NonSelected");
            Label.text = "";
            NonSelected.enabled = false;
        }

        for (int i = 0; i < days; i++)
        {
            GameObject date = NGUITools.AddChild(m_taTable.gameObject, m_goDatePrefab);
            UILabel Label = Util.FindCo<UILabel>(date, "Date");
            if (m_iYear == now.Year
                && m_iMonth == now.Month
                && (i + 1) == now.Day)
            {
                Label.text = "[FF0000]" + (i + 1).ToString() + "[-]";
            }
            else
            {
                Label.text = (i + 1).ToString();
            }
            CustomData.Set(date, i + 1);
            UIEventListener.Get(date).onClick += OnClickDate;
        }

        m_taTable.Reposition();
    }
}
