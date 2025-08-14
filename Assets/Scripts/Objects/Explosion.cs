using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private bool isTip;
    public int TimeToExplode { get; set; }

    private Animator _animator;
    private bool _explodeImmediately;
    private readonly List<GameObject> _playersInTrigger = new List<GameObject>();

    private const string ANIM_TRIGGER_TIP = "explodeTip";
    private const string ANIM_TRIGGER_MIDDLE = "explodeMiddle";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(ExplosionRoutine());
    }

    public void ExplodeNOW()
    {
        _explodeImmediately = true;
        StopAllCoroutines();
        StartCoroutine(ExplosionRoutine());
    }

    private IEnumerator ExplosionRoutine()
    {
        if (!_explodeImmediately)
        {
            yield return new WaitForSeconds(TimeToExplode);
        }

        string animationTrigger = isTip ? ANIM_TRIGGER_TIP : ANIM_TRIGGER_MIDDLE;
        _animator.SetBool(animationTrigger, true);
        DamagePlayers();
        Destroy(gameObject, 0.6f);
    }
    
    private void DamagePlayers()
    {
        List<GameObject> playersToDamage = new List<GameObject>(_playersInTrigger);
        foreach (var player in playersToDamage)
        {
            player.GetComponent<Bomberman>()?.ExplodePlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_playersInTrigger.Contains(other.gameObject))
            {
                _playersInTrigger.Add(other.gameObject);
            }
        }
        else if (other.CompareTag("Bomb"))
        {
            other.GetComponent<Bomb>()?.TriggerImmediateExplosion();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playersInTrigger.Remove(other.gameObject);
        }
    }
}