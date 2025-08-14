using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private Animator anim;
    private int timeToExplode;
    private bool explodeNow = false;
    private bool isAlreadyRespawned = false;
    [SerializeField] private List<GameObject> powerUpsPrefabs;
    private BoxCollider2D boxCollider;

    void Start()
    {
        anim = GetComponent<Animator>();
        BoxCollider = GetComponent<BoxCollider2D>();
    }


    public void ExplodeBoxNow(int timeToExplode)
    {
        StartCoroutine(ExplodeBox(timeToExplode));
    }

    IEnumerator ExplodeBox(int timeToExplode)
    {
        if (!ExplodeNow)
        {
            yield return new WaitForSecondsRealtime(timeToExplode);
            anim.SetTrigger("Explode");
            BoxCollider.enabled = false;
            RespawnPowerUp();
            Destroy(gameObject, 1.5f);
        } 
        else
        {
            anim.SetTrigger("Explode");
            BoxCollider.enabled = false;
            RespawnPowerUp();
            Destroy(gameObject, 1.5f);
        }

    }

    void RespawnPowerUp()
    {
        if (!IsAlreadyRespawned)
        {
           int index = PowerUpRespawnController.Instance.SortPowerUp();
            //0 = no respawn
            if (index > 0)
            {
                GameObject powerUp = Instantiate(PowerUpsPrefabs[index], new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                IsAlreadyRespawned = true;
            }
        }
    }

    public bool ExplodeNow { get => ExplodeNow1; set => ExplodeNow1 = value; }
    public int TimeToExplode { get => timeToExplode; set => timeToExplode = value; }
    public List<GameObject> PowerUpsPrefabs { get => powerUpsPrefabs; set => powerUpsPrefabs = value; }
    public bool IsAlreadyRespawned { get => isAlreadyRespawned; set => isAlreadyRespawned = value; }
    public BoxCollider2D BoxCollider { get => boxCollider; set => boxCollider = value; }
    public bool ExplodeNow1 { get => explodeNow; set => explodeNow = value; }
}
