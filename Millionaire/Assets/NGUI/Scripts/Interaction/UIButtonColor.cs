//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Simple example script of how a button can be colored when the mouse hovers over it or it gets pressed.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Button Color")]
public class UIButtonColor : MonoBehaviour
{
	/// <summary>
	/// Target with a widget, renderer, or light that will have its color tweened.
	/// </summary>

	public GameObject tweenTarget;

	/// <summary>
	/// Color to apply on hover event (mouse only).
	/// </summary>

	public Color hover = new Color(1f, 1f, 1f, 1f);

	/// <summary>
	/// Color to apply on the pressed event.
	/// </summary>

	public Color pressed = Color.grey;

	/// <summary>
	/// Duration of the tween process.
	/// </summary>

	public float duration = 0.0f; // add bu Timorkong 0.2f to 0.0f

    /// add by thomas-meng
    /// fix button disappear issues
    /// add end
	protected Color mColor = new Color(1, 1, 1, 1);
	protected bool mStarted = false;
	protected bool mHighlighted = false;

	/// <summary>
	/// UIButtonColor's default (starting) color. It's useful to be able to change it, just in case.
	/// </summary>

	public Color defaultColor
	{
		get
		{
			if (!mStarted) Init();
			return mColor;
		}
		set 
        { 
            mColor = value;
        }
	}

	void Start ()
	{
		if (!mStarted)
		{
			Init();
			mStarted = true;
		}
	}

	protected virtual void OnEnable () { if (mStarted && mHighlighted) OnHover(UICamera.IsHighlighted(gameObject)); }

	void OnDisable ()
	{
		if (mStarted && tweenTarget != null)
		{
			TweenColor tc = tweenTarget.GetComponent<TweenColor>();

			if (tc != null)
			{
				tc.color = mColor;
				tc.enabled = false;
			}
		}
	}

	protected void Init ()
	{
		if (tweenTarget == null) tweenTarget = gameObject;
		UIWidget widget = tweenTarget.GetComponent<UIWidget>();

		if (widget != null)
		{
			mColor = widget.color;
		}
		else
		{
			Renderer ren = tweenTarget.renderer;

			if (ren != null)
			{
				mColor = ren.material.color;
			}
			else
			{
				Light lt = tweenTarget.light;

				if (lt != null)
				{
					mColor = lt.color;
				}
				else
				{
                    //这句话太烦人，而且又没有什么实际的意义，只是多了一堆运行时警告
					//Debug.LogWarning(NGUITools.GetHierarchy(gameObject) + " has nothing for UIButtonColor to color", this);
					enabled = false;
				}
			}
		}
		OnEnable();
	}

	public virtual void OnPress (bool isPressed)
	{
		if (enabled)
		{
			if (!mStarted) Start();
			TweenColor.Begin(tweenTarget, duration, isPressed ? pressed : (UICamera.IsHighlighted(gameObject) ? hover : mColor));
		}
	}

	public virtual void OnHover (bool isOver)
	{
		if (enabled)
		{
			if (!mStarted) Start();
			TweenColor.Begin(tweenTarget, duration, isOver ? hover : mColor);
			mHighlighted = isOver;
		}
	}
}
