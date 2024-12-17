using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AoeTower : MonoBehaviour
{

    //Level level;
    private GameObject best;
    private GameObject currProjectile;
    private float blocksize;
    public static float surroundingDistance;

    public AudioSource launch;

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
        best = Best(targetsTag);
    }
 
    IEnumerator Shoot()
    {
        while (true) {
            if (best != null && currProjectile == null)
            {
                currProjectile = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y + 18, transform.position.z), transform.rotation);
                currProjectile.transform.localScale = new Vector3(blocksize, blocksize, blocksize);
                Projectile p = currProjectile.GetComponent<Projectile>();
                launch.Play();
                p.SetupProjectile(best, damage, blocksize * velocity, true);
                currProjectile = null;
            }
            yield return new WaitForSeconds(1.8f);
        } 
    }

    GameObject Best(string tag)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);
        float max = -1;
        GameObject best = null;
        foreach (GameObject enemy in enemies)
        {
           Collider[] nearbyEnemies = Physics.OverlapSphere(enemy.transform.position,surroundingDistance);
           nearbyEnemies = nearbyEnemies.Where(c => c.gameObject.tag.Contains("Enemy")).ToArray();
           Debug.Log(nearbyEnemies.Length);
            if (nearbyEnemies.Length > max)
            {
                max = nearbyEnemies.Length;
                best = enemy;
            }
        }
        return best;
    }

}
