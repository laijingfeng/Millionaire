using UnityEngine;
using System.Collections;

public class BirdController : MonoBehaviour
{
    void Update()
    {
        float x = Input.GetAxis("Horizontal") * Time.deltaTime;
        float y = Input.GetAxis("Vertical") * Time.deltaTime;

        Vector3 vt = transform.position;
        vt += new Vector3(x, y, 0);

        transform.position = vt;
    }
}
