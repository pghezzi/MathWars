using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    [SerializeField] float dragSpeed = 1f;
    [SerializeField] float sensitivity = 1f;

    Vector3 dragOrigin;
    Vector3 pos;
    Vector3 move;

    bool first = true;

    void FixedUpdate()
    {
        if (!Input.GetMouseButton(0))
        {
            first = true;
            return;
        }
        
        if (first)
        {
            dragOrigin = Input.mousePosition;
            first = false;
            return;
        }
        pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        move = new Vector3(-dragSpeed * 100 * pos.x, 0, -dragSpeed * 100 * pos.y);
        if (move.magnitude > sensitivity)
        {
            transform.Translate(move, Space.World);
        }
        dragOrigin = Input.mousePosition;
        return;

    }
}
