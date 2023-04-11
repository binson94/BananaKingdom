using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public Camera mainCam;

    private Vector3 originalCamPos;
    private float shakeAmount = 0;

    private void Awake()
    {
        if (mainCam == null)
        {
            Debug.Log("새 카메라 탐색");
            mainCam = Camera.main;
        }
    }

    public void Shake(float amt, float length)
    {
        shakeAmount = amt;
        InvokeRepeating("DoShake", 0, 0.01f);
        Invoke("StopShake", length);
    }

    void DoShake()
    {
        originalCamPos = mainCam.transform.position;
        Vector3 currentCamPos = mainCam.transform.position;

        if (shakeAmount > 0)
        {
            float offSetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offSetY = Random.value * shakeAmount * 2 - shakeAmount;
            currentCamPos.x += offSetX;
            currentCamPos.y += offSetY;

            mainCam.transform.position = currentCamPos;

        }
    }

    void StopShake()
    {
        CancelInvoke("DoShake");
        mainCam.transform.localPosition = originalCamPos;
    }
}
