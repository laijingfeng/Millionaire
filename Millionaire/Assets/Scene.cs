using UnityEngine;
using System.Collections;

public class Scene : MonoBehaviour
{
    public float xMin = 0;
    public float xMax = 1;
    public float yMin = 0;
    public float yMax = 1;

    private static Scene m_instance = null;

    public static Scene Instance
    {
        get
        {
            return m_instance;
        }
    }

    void Awake()
    {
        m_instance = this;
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

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireCube(new Vector3((xMax + xMin) * 0.5f, (yMax + yMin) * 0.5f, -5), new Vector3(xMax - xMin, yMax - yMin, 0));

        float cameraAreaHalfX = HalfCameraWidth;
        float cameraAreaLeft = xMin + cameraAreaHalfX;
        float cameraAreaRight = xMax - cameraAreaHalfX;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(cameraAreaLeft, (yMax + yMin) * 0.5f, -5), new Vector3(cameraAreaRight, (yMax + yMin) * 0.5f, -5));
    }
#endif
}
