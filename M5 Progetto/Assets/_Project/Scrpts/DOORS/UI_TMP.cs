using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TMP : MonoBehaviour
{
    private Transform _mainCameraTransform;

    private void Start()
    {
        if (Camera.main)
        {
            _mainCameraTransform = Camera.main.transform;
        }
    }

    private void LateUpdate()
    {
        if (_mainCameraTransform != null)
        {
            Vector3 targetPosition = new Vector3(_mainCameraTransform.position.x, transform.position.y , _mainCameraTransform.position.z);
            transform.LookAt(targetPosition);
            transform.Rotate(0, 180, 0);
        }
    }
}
