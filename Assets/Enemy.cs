using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float amplitude;
    [SerializeField] private float frequency;

    private float _phaseStart;
    private float _initialScale;
    float sphereRadious;

    private void Start()
    {
        _initialScale = sphereRadious;
        _phaseStart = Random.Range(0.0f, 0.1f);
    }

    private void Update()
    {
        if (!PauseManager.Instance.isPaused)
        {
            float scaleFactor = _initialScale + amplitude * Mathf.Sin(_phaseStart + Time.time * frequency);
            SetRadious(scaleFactor);
        }
    }

    public void SetRadious(float r)
    { 
        sphereRadious = r;
        transform.localScale = new Vector3(sphereRadious, sphereRadious, sphereRadious);
    }

    public float GetRadious() { return sphereRadious; }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            EnemyManager.Instance.RemoveEnemy(this);
            Destroy(gameObject);
        }
    }
}
