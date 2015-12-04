using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestroyTimer : MonoBehaviour
{
    public float time = 1.0f;

    float pastTime;

    void Start()
    {
        pastTime = 0;

        pause = false;
    }

    void FixedUpdate()
    {
        if (pause)
        {
            return;
        }

        pastTime += Time.fixedDeltaTime;

        if (pastTime >= time)
        {
            Object.Destroy(gameObject);
        }
    }

    bool pause = false;
    public void Pause()
    {
        pause = true;
    }

    public void Restore()
    {
        pause = false;
    }
}