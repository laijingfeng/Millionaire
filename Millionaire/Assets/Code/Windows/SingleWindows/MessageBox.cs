using UnityEngine;
using System.Collections;

public class MessageBox : SingletonWindow<MessageBox>
{
    private UILabel m_lbLabel;

    private UIButtonMessage m_btnButton;

    void Start()
    {
        m_btnButton = Util.FindCo<UIButtonMessage>(gameObject, "Button");

        m_lbLabel = Util.FindCo<UILabel>(gameObject, "Label");

        UIEventListener.Get(m_btnButton.gameObject).onClick += OnClickButton;
    }

    public void Show(string text, bool bUseBoxCollider = true)
    {
        Show(); 
        
        m_btnButton.GetComponent<BoxCollider>().enabled = bUseBoxCollider;

        if (m_btnButton.animation)
        {
            m_btnButton.animation.Stop();
            m_btnButton.animation.Play();
            StopCoroutine("WaitAnimation");
            StartCoroutine("WaitAnimation");
        }

        m_lbLabel.text = text;
    }

    private IEnumerator WaitAnimation()
    {
        yield return new WaitForEndOfFrame();
        while(m_btnButton.animation.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
        Hide();
    }

    private void OnClickButton(GameObject go)
    {
        Hide();
    }
}
