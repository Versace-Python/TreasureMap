using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapZoom : MonoBehaviour
{
    public float zoomSpeed = 0.5f;
    public float minZoom = 1.0f;
    public float maxZoom = 10.0f;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            float scaleFactor = 1 + zoomSpeed * Input.mouseScrollDelta.y;
            transform.localScale = Vector3.ClampMagnitude(transform.localScale * scaleFactor, maxZoom);
            transform.localScale = new Vector3(Mathf.Max(minZoom, transform.localScale.x), Mathf.Max(minZoom, transform.localScale.y), 1);
        }
    }
}
