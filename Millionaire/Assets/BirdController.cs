using UnityEngine;
using System.Collections;

public class BirdController : MonoBehaviour
{
    public float speed = 1.0f;

    void Update()
    {
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float y = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        Vector3 vt = transform.position;
        vt += new Vector3(x, y, 0);

        vt.x = Mathf.Clamp(vt.x, Scene.Instance.xMin, Scene.Instance.xMax);
        vt.y = Mathf.Clamp(vt.y, Scene.Instance.yMin, Scene.Instance.yMax);

        transform.position = vt;
    }
}
