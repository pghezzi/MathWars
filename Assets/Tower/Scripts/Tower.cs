using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{

    //Level level;
    GameObject closest;
    string tag;
    [SerializeField] GameObject projectile;
    GameObject currProjectile;
    [SerializeField] float damage;
    [SerializeField] float range;
    [SerializeField] float velocity;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update()
    {

        closest = Closest(tag);
    }

    IEnumerator Shoot()
    {
        while (true) {
            if (closest != null && currProjectile == null)
            {
                
                currProjectile = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y + 18, transform.position.z), transform.rotation);
                Projectile p = currProjectile.GetComponent<Projectile>();
                p.SetupProjectile(closest, damage, velocity);
            }
            yield return new WaitForSeconds(1);
        } 
    }

    GameObject Closest(string tag)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("GroundEnemy");
        float min = float.MaxValue;
        GameObject closest = null;
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (distance < min)
            {
                min = distance;
                closest = enemy;
            }
        }
        if (min > range) return null;
        return closest;
    }

}
