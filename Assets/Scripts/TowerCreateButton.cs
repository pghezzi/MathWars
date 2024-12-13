using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCreateButton : MonoBehaviour
{
    private TowerPlacingUI tpui;

    void Start()
    {
        tpui = gameObject.transform.parent.parent.GetComponent<TowerPlacingUI>();
    }

    public void onClick(GameObject towerToCreate)
    {
        Debug.Assert(towerToCreate != null);
        Debug.Log("hey");
        float[] pos = tpui.CurrentPos();
        GameObject tower = Instantiate(towerToCreate);
        tower.transform.localScale = new UnityEngine.Vector3(10.0f, 10.0f, 10.0f);
        tower.transform.position = new UnityEngine.Vector3(pos[0], 0, pos[1]);
    }

}
