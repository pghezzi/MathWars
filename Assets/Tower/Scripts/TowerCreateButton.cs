using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerCreateButton : MonoBehaviour
{
    private TowerPlacingUI tpui;
    private InfoPanelManager infoPanelManager;
    private Button thisButton;
    public int cost;

         
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        thisButton = GetComponent<Button>();
    }

    void Start()
    {
        tpui = gameObject.transform.parent.parent.GetComponent<TowerPlacingUI>();
        infoPanelManager = GameObject.Find("User Interface(Clone)").gameObject.transform.GetChild(1).GetComponent<InfoPanelManager>();
    }
    
    void Update()
    {
        thisButton.interactable = infoPanelManager.canAfford(cost);
    }

    public void onClick(GameObject towerToCreate)
    {
        Debug.Assert(towerToCreate != null);
        //Debug.Log("hey");
        if (!infoPanelManager.canAfford(cost))
        {
            return;
        }
        audioManager.PlaySFX(audioManager.builtTower);
        float[] pos = tpui.CurrentPos();
        GameObject tower = Instantiate(towerToCreate);
        tower.transform.localScale = new UnityEngine.Vector3(10.0f, 10.0f, 10.0f);
        tower.transform.position = new UnityEngine.Vector3(pos[0], 0, pos[1]);
        tpui.deactivate();
        infoPanelManager.loseCoins(cost);
    }

}
