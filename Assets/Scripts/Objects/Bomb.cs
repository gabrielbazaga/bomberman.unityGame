using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private int timeToExplode = 3;
    [SerializeField] private int power = 2;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private LayerMask collisionLayerMask;

    [SerializeField] private Transform[] explosionDirections;

    private Animator _animator;
    private BoxCollider2D _boxCollider;
    private bool _isExploding;
    private bool _explodeNow;

    private const string EXPLOSION_TRIGGER = "explode";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        _boxCollider.enabled = true;
        CalculateExplosionPaths();
        SoundController.instance.PlayAudio(SoundController.instance.PutABombAudio);
        StartCoroutine(ExplosionRoutine());
    }

    private void Update()
    {
        DrawExplosionDebugRays();
    }

    public void TriggerImmediateExplosion()
    {
        _explodeNow = true;
        StartCoroutine(ExplosionRoutine());
    }

    private IEnumerator ExplosionRoutine()
    {
        if (!_explodeNow)
        {
            yield return new WaitForSeconds(timeToExplode);
        }
        else
        {
            TriggerChildExplosions();
        }

        _isExploding = true;
        _animator.SetTrigger(EXPLOSION_TRIGGER);
        SoundController.instance.PlayAudio(SoundController.instance.BombExplosionAudio);

        GameController.Instance.BombList.Remove(gameObject);
        GameController.Instance.BombListPosition.Remove(transform.position);

        Destroy(gameObject, 0.6f);
    }

    private void CalculateExplosionPaths()
    {
        // Instancia a explosão central na própria bomba.
        InstantiateExplosionPiece(transform.position, Quaternion.identity, false);
        
        foreach (var directionTransform in explosionDirections)
        {
            ProcessExplosionInDirection(directionTransform);
        }
    }

    private void ProcessExplosionInDirection(Transform directionTransform)
    {
        Vector2 direction = directionTransform.localPosition.normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, power, collisionLayerMask);

        int explosionLength = (hit.collider == null) ? power : Mathf.RoundToInt(hit.distance);
        bool shouldSpawnTip = (hit.collider == null);

        if (hit.collider != null)
        {
            HandleExplosionCollision(hit.collider);
        }
        
        Quaternion rotation = GetRotationForDirection(directionTransform.name);

        for (int i = 1; i <= explosionLength; i++)
        {
            Vector2 spawnPosition = (Vector2)transform.position + direction * i;
            bool isTip = shouldSpawnTip && (i == explosionLength);
            InstantiateExplosionPiece(spawnPosition, rotation, isTip);
        }
    }
    private void HandleExplosionCollision(Collider2D hitCollider)
    {
        if (hitCollider.CompareTag("Box"))
        {
            hitCollider.GetComponent<Box>()?.ExplodeBoxNow(timeToExplode);
        }
        else if (hitCollider.CompareTag("PowerUp"))
        {
            hitCollider.GetComponent<PowerUp>()?.ExplodeNow(timeToExplode);
        }
    }

    private void InstantiateExplosionPiece(Vector2 position, Quaternion rotation, bool isTip)
    {
        if (explosionPrefab == null) return;

        GameObject piece = Instantiate(explosionPrefab, position, rotation, transform);
        Explosion explosionScript = piece.GetComponent<Explosion>();
        if (explosionScript != null)
        {
            explosionScript.TimeToExplode = timeToExplode;
            explosionScript.IsTip = isTip;
        }
    }

    private Quaternion GetRotationForDirection(string directionName)
    {
        switch (directionName.ToLower())
        {
            case "right": return Quaternion.Euler(0f, 0f, 180f);
            case "top":   return Quaternion.Euler(0f, 0f, 270f);
            case "down":  return Quaternion.Euler(0f, 0f, 90f);
            case "left":
            default:      return Quaternion.identity;
        }
    }
    
    private void TriggerChildExplosions()
    {
        foreach (Explosion explosion in GetComponentsInChildren<Explosion>())
        {
            explosion.ExplodeNOW();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _boxCollider.isTrigger = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_isExploding && other.CompareTag("Player"))
        {
            other.GetComponent<Bomberman>()?.ExplodePlayer();
        }
    }

    private void DrawExplosionDebugRays()
    {
        if (explosionDirections == null) return;
        foreach (Transform side in explosionDirections)
        {
            Debug.DrawRay(transform.position, side.localPosition.normalized * power, Color.red);
        }
    }
}