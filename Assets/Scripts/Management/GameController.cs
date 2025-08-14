using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private static GameController instance;
    [SerializeField] private GameObject winnerText;

    private List<GameObject> bombList;
    private List<Vector2> bombListPosition;
    private List<GameObject> playerObjects;
    int layerPrefabReference;
    private GameObject winner = null;
    private bool isStarted = false;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        BombList = new List<GameObject>();
        BombListPosition = new List<Vector2>();
    }
    private void Update()
    {
        if (IsStarted)
        {
            UpdateOrderInLayer();
            DeclareWinner();
        }

    }
    private void UpdateOrderInLayer()
    {
        PlayerObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        
        if (PlayerObjects.Count >= 1 && layerPrefabReference == 0) {
            layerPrefabReference = PlayerObjects[0].GetComponent<SpriteRenderer>().sortingOrder;
        }

        if (PlayerObjects.Count >= 2)
        {
            PlayerObjects.Sort((a, b) => a.transform.position.y.CompareTo(b.transform.position.y));

            // Atualiza a Order in Layer de cada objeto na lista
            for (int i = 0; i < PlayerObjects.Count; i++)
            {
                SpriteRenderer spriteRenderer = PlayerObjects[i].GetComponent<SpriteRenderer>();

                // Define a nova Order in Layer de acordo com a posição no eixo Y
                spriteRenderer.sortingOrder = layerPrefabReference - i;
            }
        }
    }

    bool teste = false;
    private void DeclareWinner() 
    {
        if (PlayerObjects.Count > 1) 
        {
            teste = true;
        }

        if (PlayerObjects.Count == 1 && teste)
        {
            Winner = PlayerObjects[0];
            Winner.GetComponent<Bomberman>().Win();
            WinnerText.GetComponent<TextMeshProUGUI>().SetText(Winner.GetComponent<Bomberman>().PlayerId + " venceu!");
            teste = false;
        }
    }

    public static GameController Instance { get => instance; set => instance = value; }
    public List<GameObject> BombList { get => bombList; set => bombList = value; }
    public List<Vector2> BombListPosition { get => bombListPosition; set => bombListPosition = value; }
    public GameObject Winner { get => winner; set => winner = value; }
    public GameObject WinnerText { get => winnerText; set => winnerText = value; }
    public List<GameObject> PlayerObjects { get => playerObjects; set => playerObjects = value; }
    public bool IsStarted { get => isStarted; set => isStarted = value; }
}
