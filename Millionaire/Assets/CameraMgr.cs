using UnityEngine;
using System.Collections;

public class CameraMgr : MonoBehaviour
{
    public bool followTarget = true;

    public float smoothTime = 0.07f;

    public GameObject target;

    public float leftPos;

    public float rightPos;

    private float leftPosX;

    private float rightPosX;

    /// <summary>
    /// <para>相机的半宽</para>
    /// <para>size/640=halfWidth/960</para>
    /// </summary>
    private float HalfCameraWidth
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
        float halfCameraWidth = HalfCameraWidth;
        leftPosX = leftPos + halfCameraWidth;
        rightPosX = rightPos - halfCameraWidth;
    }

    void LateUpdate()
    {
        if(Camera.main == null
            || followTarget == false)
        {
            return;
        }

        if(leftPosX > rightPosX)
        {
            return;
        }

        Vector3 p = Camera.main.transform.position;
        if (target != null)
        {
            float velo = 0.0f;
            p.x = Mathf.SmoothDamp(p.x, target.transform.position.x, ref velo, smoothTime);
        }

        if (p.x < leftPosX)
        {
            p.x = leftPosX;
        }
        else if(p.x > rightPosX)
        {
            p.x = rightPosX;
        }

        Camera.main.transform.position = p;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(new Vector3(leftPos, 0.5f, -5), new Vector3(rightPos, 0.5f, -5));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(leftPosX, 0, -5), new Vector3(rightPosX, 0, -5));
    }
}
