using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class TowerPlacingUI : MonoBehaviour
{
    private Level level;
    private Transform towerPlacing;
    private Camera cam;
    private float selected_x;
    private float selected_y;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        level = GameObject.FindGameObjectWithTag("Level").GetComponent<Level>();
        towerPlacing = transform.GetChild(0);
        towerPlacing.GameObject().SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!Input.GetMouseButtonDown(1)) return;
        Vector3 pointOnLine = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
        Vector3 intersect = level.LineIntersection(pointOnLine, pointOnLine - cam.transform.position);
        Vector3 recalc = level.closestValidBlock(intersect.x, intersect.z);
        if (recalc.y == 0)
        {
            selected_x = recalc.x;
            selected_y = recalc.z;
            towerPlacing.GameObject().SetActive(true);
            towerPlacing.position = cam.WorldToScreenPoint(recalc);
        }
        else
        {
            towerPlacing.GameObject().SetActive(false);
        }
    }

    public float[] CurrentPos()
    {
        return new float[] { selected_x , selected_y };
    }
}
