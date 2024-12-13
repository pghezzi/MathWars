using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{

    //Level level;
    private GameObject closest;
    private GameObject currProjectile;
    private float blocksize;

    [SerializeField] string targetsTag;
    [SerializeField] GameObject projectile;
    [SerializeField] float damage;
    [SerializeField] float velocity;

    // Start is called before the first frame update
    void Start()
    {
        currProjectile = null;
        blocksize = GameObject.FindGameObjectWithTag("Level").GetComponent<Level>().block_size;
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update()
    {
        closest = Closest(targetsTag);
    }

    IEnumerator Shoot()
    {
        while (true) {
            if (closest != null && currProjectile == null)
            {
                currProjectile = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y + 18, transform.position.z), transform.rotation);
                currProjectile.transform.localScale = new Vector3(blocksize, blocksize, blocksize);
                Projectile p = currProjectile.GetComponent<Projectile>();
                p.SetupProjectile(closest, damage, blocksize * velocity);
            }
            yield return new WaitForSeconds(1);
        } 
    }

    GameObject Closest(string tag)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);
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
        return closest;
    }

}
