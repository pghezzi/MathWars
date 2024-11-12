using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCreateButton : MonoBehaviour
{
    public static void onClick(GameObject towerToCreate)
    {
        Debug.Assert(towerToCreate != null);
        Debug.Log("hey");
        GameObject tower = Instantiate(towerToCreate);
        tower.transform.localScale = new UnityEngine.Vector3(10.0f, 10.0f, 10.0f);
    }

}
