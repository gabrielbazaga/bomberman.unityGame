using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bomberman : MonoBehaviour
{

    [SerializeField] private int speed;
    [SerializeField] private int bombPower = 1;
    [SerializeField] private int bombCount = 1;
    [SerializeField] private string playerId;
    [SerializeField] private GameObject playerPrefab;

    private int bombRemaining;

    private float offset = 0.5f;
    private Rigidbody2D rig;
    private Animator anim;
    private Vector2 direction;
    private Vector2 directionFromInput;
    private bool putABombInput = false;
    [SerializeField] private bool isDead;

    [SerializeField] private Transform bombPoint;
    [SerializeField] private GameObject bomb;
    private List<PowerUp> powerUps;
    private GameController gameController;

    void Awake()
    {
        PowerUps = new List<PowerUp>();
        Rig = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        IsDead = false;
        bombRemaining = bombCount;
        gameController = GameController.Instance;
    }

    private void FixedUpdate()
    {
       OnMove();
    }

    void OnMove()
    {
        if (!isDead)
        {
            Rig.MovePosition(rig.position + Direction * Speed * Time.fixedDeltaTime);
        }
    }

    public void OnMoveInputAction(InputAction.CallbackContext context)
    {
        if (gameController.IsStarted)
        {
            Direction = context.ReadValue<Vector2>();
        }

    }

    public void PutABombInputAction(InputAction.CallbackContext context)
    {
        if (gameController.IsStarted)
        {
            PutABomb();
        }
    }

    void PutABomb()
    {
            if(BombRemaining > 0)
            {

                if (!GameController.Instance.BombListPosition.Contains(GetTileCenter()))
                {
                    BombRemaining--;
                    GameObject bomb = Instantiate(Bomb, GetTileCenter(), bombPoint.rotation);
                    bomb.GetComponent<Bomb>().Power = BombPower;
                    GameController.Instance.BombListPosition.Add(GetTileCenter());
                    GameController.Instance.BombList.Add(bomb);
                    Invoke("AddBombRemaining", bomb.GetComponent<Bomb>().TimeToExplode);
                }
            }   
    }

    void AddBombRemaining()
    {
        ++BombRemaining;
    }
    private Vector2 GetTileCenter()
    {
        float centroX = (int)bombPoint.position.x + offset;
        float centroY = (int)bombPoint.position.y + offset;

        return new Vector2(centroX, centroY);
    }

    public void ExplodePlayer()
    {
        if (!IsDead)
        {
            IsDead = true;
            anim.SetInteger("transition", 0);
            anim.SetTrigger("explode");
            Destroy(gameObject, 0.6f);
        }
    }

    public void Win() 
    {
        anim.SetTrigger("wins");
    }

    private void IncreaseOrDecreasePowerUps(PowerUp up)
    {
        if (up != null)
        {
            SoundController.instance.PlayAudio(SoundController.instance.GetPowerUpAudio);
            switch (up.PowerNameEnum.ToString())
            {
                case "FireUp":
                case "FireDown":
                    {
                        if ((up.index > 0 && BombPower == 1) || BombPower >= 2) 
                        {
                            BombPower += up.index;                            
                        }
                        break;
                    }
                case "SpeedUp":
                case "SpeedDown":
                    {   //Speed 1 is too slow, min = 2, max = 9 please :D
                        if ((up.index > 0 && Speed == 2) || Speed >= 3)
                        {
                            if (Speed == 9 && up.index > 0)
                            {
                                break;
                            } 
                            else
                            {
                               Speed += up.index;
                            }

                        }
                        break;
                    }
                case "BombUp":
                case "BombDown":
                    {
                        if ((up.index > 0 && BombCount == 1) || BombCount >= 2)
                        { 
                            BombCount += up.index;
                            BombRemaining += up.index;
                        }                            
                        break;
                    }
            }
            
            Destroy(up.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null)
        {
            if (other.tag == "PowerUp")
            {
                IncreaseOrDecreasePowerUps(other.GetComponent<PowerUp>());
            }
        }
    }

    public Rigidbody2D Rig { get => rig; set => rig = value; }
    public Animator Anim { get => anim; set => anim = value; }
    public int Speed { get => speed; set => speed = value; }
    public Vector2 Direction { get => direction; set => direction = value; }
    public GameObject Bomb { get => bomb; set => bomb = value; }
    public Transform BombPoint { get => bombPoint; set => bombPoint = value; }
    public int BombPower { get => bombPower; set => bombPower = value; }
    public List<PowerUp> PowerUps { get => powerUps; set => powerUps = value; }
    public int BombCount { get => bombCount; set => bombCount = value; }
    public bool IsDead { get => isDead; set => isDead = value; }
    public int BombRemaining { get => bombRemaining; set => bombRemaining = value; }
    public Vector2 DirectionFromInput { get => directionFromInput; set => directionFromInput = value; }
    public bool PutABombInput { get => putABombInput; set => putABombInput = value; }
    public string PlayerId { get => playerId; set => playerId = value; }
    public GameObject PlayerPrefab { get => playerPrefab; set => playerPrefab = value; }
}
