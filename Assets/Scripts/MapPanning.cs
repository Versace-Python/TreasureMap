using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPanning : MonoBehaviour
{
    public float panSpeed = 0.1f;
    private Vector3 dragOrigin;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.ScreenToWorldPoint(dragOrigin);
            transform.position += difference * panSpeed;
        }
    }
}

