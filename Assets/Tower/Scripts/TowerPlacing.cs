using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class TowerPlacing : MonoBehaviour
{
    private Level level;
    private Renderer r;
    public GameObject tower;
    private UnityEngine.Vector3 lastpos;
    // Start is called before the first frame update
    void Start()
    {
        GameObject level_obj = GameObject.FindGameObjectWithTag("Level");
        level = level_obj.GetComponent<Level>();
        if (level == null)
        {
            Debug.LogError("Internal error: could not find the Level object - did you remove its 'Level' tag?");
            return;
        }
        if (level_obj != null) {
            Destroy(Globals.currentlyPlacing);
        }
        Globals.currentlyPlacing = gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        Camera cam = Camera.main;
        float dist = UnityEngine.Vector3.Dot(transform.position - cam.transform.position, cam.transform.forward);
        UnityEngine.Vector3 newPos = cam.ScreenToWorldPoint(new UnityEngine.Vector3(Input.mousePosition.x, Input.mousePosition.y, dist));
        UnityEngine.Vector3 recalc = level.closestValidBlock(newPos.x, newPos.z);
        if (recalc.y == 0) {
            recalc.y = 2;
            transform.position = recalc;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (transform.position == lastpos) return;
            level.AddTower(transform.position.x, transform.position.z);
            GameObject temp = Instantiate(tower, transform.position, transform.rotation);
            temp.transform.localScale = new UnityEngine.Vector3(10.0f, 10.0f, 10.0f);
            lastpos = temp.transform.position;
            Destroy(gameObject);
        }
    }
}
