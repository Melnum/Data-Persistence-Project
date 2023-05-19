using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    private float shakeAmount = 0.7f;
    private Vector3 orignalCameraPos;
    private bool shakeInProgress = false;
    private float shakeTimer;

    // Update is called once per frame
    void Update()
    {
        if (shakeInProgress)
        {
            StartCameraShakeEffect();
        }
    }

    public void ShakeCamera(float _shakeDuration, float _shakeAmount)
    {
        orignalCameraPos = cameraTransform.localPosition;
        shakeAmount = _shakeAmount;
        shakeTimer = _shakeDuration;
        shakeInProgress = true;
    }

    public void StartCameraShakeEffect()
    {
        if (shakeTimer > 0)
        {
            cameraTransform.localPosition = orignalCameraPos + Random.insideUnitSphere * shakeAmount;
            shakeTimer -= Time.deltaTime;
        }
        else
        {
            shakeTimer = 0f;
            cameraTransform.localPosition = orignalCameraPos;
            shakeInProgress = false;
        }
    }
}