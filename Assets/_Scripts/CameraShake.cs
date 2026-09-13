using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 _originalPosition;
    private float _shakeTimer = 0f;
    private float _shakeMagnitude = 0f;

    void Start()
    {
        // THIS is what was missing! We have to save the camera's Z:-10 position when the game loads.
        _originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (_shakeTimer > 0)
        {
            // Pick a rapid, random offset to simulate intense shaking
            float x = Random.Range(-1f, 1f) * _shakeMagnitude;
            float y = Random.Range(-1f, 1f) * _shakeMagnitude;
            
            transform.localPosition = new Vector3(_originalPosition.x + x, _originalPosition.y + y, _originalPosition.z);
            
            // Count down the timer
            _shakeTimer -= Time.deltaTime;
        }
        else if (transform.localPosition != _originalPosition)
        {
            // Snap back perfectly into place when the shake is done
            transform.localPosition = _originalPosition;
        }
    }

    public void TriggerShake(float duration, float magnitude)
    {
        _originalPosition = transform.localPosition;
        _shakeTimer = duration;
        _shakeMagnitude = magnitude;
    }
}