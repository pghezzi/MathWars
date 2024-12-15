using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTower : MonoBehaviour
{
    // Start is called before the first frame update
    private InfoPanelManager infoPanelManager;
    void Start()
    {
        infoPanelManager = GameObject.Find("User Interface(Clone)").gameObject.transform.GetChild(1).GetComponent<InfoPanelManager>();
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            infoPanelManager.gainCoins(10);
        }
    }
}
