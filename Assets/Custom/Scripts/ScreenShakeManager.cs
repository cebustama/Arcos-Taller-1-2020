using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeManager : MonoBehaviour
{
    public static ScreenShakeManager instance;
    public const float maxShake = 1f;

    Vector3 originalCamPos;

    float totalShakeMagnitude = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        originalCamPos = transform.localPosition;
    }

    private void Update()
    {
        // Shaking
        if (totalShakeMagnitude > 0)
        {
            totalShakeMagnitude = (totalShakeMagnitude > maxShake) ? maxShake : totalShakeMagnitude;
            transform.localPosition = originalCamPos + (Vector3)Random.insideUnitCircle * totalShakeMagnitude;
        }
        else
        {
            transform.localPosition = originalCamPos;
        }
    }

    public void AddShakeManual(float amount)
    {
        totalShakeMagnitude += amount;
    }

    public void RemoveShakeManual(float amount)
    {
        totalShakeMagnitude -= amount;
    }

    public void AddShake(float amount, float time)
    {
        totalShakeMagnitude += amount;

        StartCoroutine(EndShake(amount, time));
    }

    public void AddShake(float amount)
    {
        totalShakeMagnitude += amount;

        StartCoroutine(EndShake(amount, 0.2f));
    }

    IEnumerator EndShake(float amount, float delay)
    {
        yield return new WaitForSeconds(delay);

        totalShakeMagnitude -= amount;
    }
}
