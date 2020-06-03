using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    public static HitStop instance;

    bool waiting;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Stop(float stopTime)
    {
        if (waiting) return;

        Time.timeScale = 0f;

        StartCoroutine(EndStop(stopTime));
    }

    IEnumerator EndStop(float delay)
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(delay);

        Time.timeScale = 1f;
        waiting = false;
    }
}