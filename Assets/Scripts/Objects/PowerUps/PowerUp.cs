using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    private string powerName;
    public int index;

    [SerializeField] private PowerUpNameEnum powerNameEnum;

    private Animator anim;

    private void Awake()
    {
        Anim = GetComponent<Animator>();
    }

    public enum PowerUpNameEnum 
    {
        FireUp,
        FireDown,
        SpeedUp,
        SpeedDown,
        BombUp,
        BombDown
    }

    public void ExplodeNow(int timeToExplode)
    {
        StartCoroutine(Explode(timeToExplode));
    }

    public void ActivateDeactivateAnimation(bool condition) 
    {
        Anim.SetBool(PowerNameEnum.ToString(), condition);
    }

    IEnumerator Explode(int timeToExplode)
    {
        yield return new WaitForSecondsRealtime(timeToExplode);
        ActivateDeactivateAnimation(false);
        anim.SetTrigger("Explode");
        Destroy(gameObject, 1.5f);
    }
    public PowerUpNameEnum PowerNameEnum { get => powerNameEnum; set => powerNameEnum = value; }
    public Animator Anim { get => anim; set => anim = value; }
    public string PowerName { get => powerName; set => powerName = value; }
}
