using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [SerializeField] private Transform platform;

    void Start()
    {
        platform.localScale *= _camera.orthographicSize;
        platform.localScale = new Vector3(4f * _camera.orthographicSize, 1, 2f *_camera.orthographicSize);
    }
}