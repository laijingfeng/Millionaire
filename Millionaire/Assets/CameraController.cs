using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    #region public

    /// <summary>
    /// 是否跟随目标
    /// </summary>
    public bool followTarget = true;

    /// <summary>
    /// 平滑时间
    /// </summary>
    public float smoothTime = 0.07f;

    /// <summary>
    /// 目标
    /// </summary>
    public GameObject target;

    #endregion

    /// <summary>
    /// 相机移动区域，最左
    /// </summary>
    private float cameraAreaLeft;

    /// <summary>
    /// 相机移动区域，最右
    /// </summary>
    private float cameraAreaRight;

    /// <summary>
    /// 单例
    /// </summary>
    private static CameraController m_instance = null;

    /// <summary>
    /// 单例
    /// </summary>
    public static CameraController Instance
    {
        get
        {
            if (m_instance != null)
            {
                return m_instance;
            }

            if (Camera.main != null)
            {
                return null;
            }

            m_instance = Camera.main.GetComponent<CameraController>();

            if (m_instance != null)
            {
                return m_instance;
            }

            m_instance = Camera.main.gameObject.AddComponent<CameraController>();

            return m_instance;
        }
    }

    /// <summary>
    /// <para>相机的半宽</para>
    /// <para>orthographicSize/640=halfWidth/960</para>
    /// </summary>
    public float HalfCameraWidth
    {
        get
        {
            float size = 3f;
            if (Camera.main != null)
            {
                size = Camera.main.orthographicSize;
            }
            return size * 960f / 640f;
        }
    }

    void Update()
    {
        SetCaremaArea(Scene.Instance.xMin, Scene.Instance.xMax);
    }

    /// <summary>
    /// 设置相机区域
    /// </summary>
    /// <param name="xMin"></param>
    /// <param name="xMax"></param>
    public void SetCaremaArea(float xMin, float xMax)
    {
        float halfCameraWidth = HalfCameraWidth;
        cameraAreaLeft = xMin + halfCameraWidth;
        cameraAreaRight = xMax - halfCameraWidth;
    }

    void LateUpdate()
    {
        if (Camera.main == null
            || followTarget == false)
        {
            return;
        }

        if (cameraAreaLeft > cameraAreaRight)
        {
            return;
        }

        Vector3 p = Camera.main.transform.position;
        if (target != null)
        {
            float velo = 0.0f;
            p.x = Mathf.SmoothDamp(p.x, target.transform.position.x, ref velo, smoothTime);
        }

        if (p.x < cameraAreaLeft)
        {
            p.x = cameraAreaLeft;
        }
        else if (p.x > cameraAreaRight)
        {
            p.x = cameraAreaRight;
        }

        Camera.main.transform.position = p;
    }
}
