using UnityEngine;
using System.Collections;

public class SceneSpeed : MonoBehaviour
{
    public float speed = 0.1f;

    void LateUpdate()
    {
        if (Camera.main == null)
        {
            return ;
        }

        Vector3 vt = transform.position;
        vt.x = Camera.main.transform.position.x * speed;
        transform.position = vt;
    }
}
