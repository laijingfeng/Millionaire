using UnityEngine;
using System.Collections;

public class Scene : MonoBehaviour
{
    public float speed = 0.1f;

    void Update()
    {
        Vector3 vt = transform.position;
        vt.x -= speed;
        transform.position = vt;
    }
}
