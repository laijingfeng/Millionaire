//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Simple example script of how a button can be offset visibly when the mouse hovers over it or it gets pressed.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Button Offset")]
public class UIButtonOffset : MonoBehaviour
{
	public Transform tweenTarget;
	public Vector3 hover = Vector3.zero;
    public Vector3 pressed = new Vector3(0f, -2f);// add by Timorkong (2f,-2f) to (0f, -2f)
	public float duration = 0.01f; //add by Timorkong 0.2f to 0.01f

	Vector3 mPos;
	bool mStarted = false;
	bool mHighlighted = false;

    /// <summary>
    /// hover和pressed较大的偏移量
    /// added by : Jerrylai 2014-11-14 11:09:30
    /// </summary>
    private float m_ftMaxMagnitude; 

	void Start ()
	{
		if (!mStarted)
		{
			mStarted = true;
			if (tweenTarget == null) tweenTarget = transform;
			mPos = tweenTarget.localPosition;
            
            m_ftMaxMagnitude = pressed.magnitude > hover.magnitude ? pressed.magnitude : hover.magnitude;
		}
	}

	void OnEnable () { if (mStarted && mHighlighted) OnHover(UICamera.IsHighlighted(gameObject)); }

	void OnDisable ()
	{
		if (mStarted && tweenTarget != null)
		{
			TweenPosition tc = tweenTarget.GetComponent<TweenPosition>();
            
			if (tc != null)
			{
				tc.position = mPos;
				tc.enabled = false;
			}
		}
	}

    /// <summary>
    /// 有些按钮放到UITable里重排，UITable会改变它的localPosition，但是UIButtonOffset的原mPos只赋值一次（此时是旧值），就会引起按钮飘移
    /// 这种情况下，需要给一个正确的mPos
    /// 但是不能随意给mPos赋新值，因为UIButtonOffset的TweenPosition也会改tweenTarget.localPosition，不能拿这里变化的值
    /// added by : Jerrylai 2014-11-14 11:09:30
    /// </summary>
    void CheckPos ()
    {
        if ( Vector3.Distance( mPos , tweenTarget.localPosition ) > m_ftMaxMagnitude * 2f )
        {
            mPos = tweenTarget.localPosition;
        }
    }

	void OnPress (bool isPressed)
	{
		if (enabled)
		{
			if (!mStarted) Start();
            CheckPos();
			TweenPosition.Begin(tweenTarget.gameObject, duration, isPressed ? mPos + pressed :
				(UICamera.IsHighlighted(gameObject) ? mPos + hover : mPos)).method = UITweener.Method.EaseInOut;
		}
	}

	void OnHover (bool isOver)
	{
		if (enabled)
		{
            if (!mStarted) Start();
            CheckPos();
			TweenPosition.Begin(tweenTarget.gameObject, duration, isOver ? mPos + hover : mPos).method = UITweener.Method.EaseInOut;
			mHighlighted = isOver;
		}
	}
}
