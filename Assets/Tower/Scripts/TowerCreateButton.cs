using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCreateButton : MonoBehaviour
{
    private TowerPlacingUI tpui;
    private InfoPanelManager infoPanelManager;

    void Start()
    {
        tpui = gameObject.transform.parent.parent.GetComponent<TowerPlacingUI>();
        infoPanelManager = GameObject.Find("User Interface(Clone)").gameObject.transform.GetChild(1).GetComponent<InfoPanelManager>();
    }

    public void onClick(GameObject towerToCreate)
    {
        float[] pos = tpui.CurrentPos();
        GameObject tower = Instantiate(towerToCreate);
        tower.transform.localScale = new UnityEngine.Vector3(10.0f, 10.0f, 10.0f);
        tower.transform.position = new UnityEngine.Vector3(pos[0], 0, pos[1]);
        tpui.deactivate();
        infoPanelManager.gainCoins(10);
    }

}
