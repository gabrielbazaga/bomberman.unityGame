using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private List<Transform> startingPoints;
    [SerializeField] private List<RuntimeAnimatorController> playerAnimators;
    private List<PlayerInput> players = new List<PlayerInput>();
    private PlayerInputManager playerInputManager;

    public List<PlayerInput> Players { get => players; set => players = value; }

    private void Awake()
    {
        playerInputManager = FindObjectOfType<PlayerInputManager>();
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += AddPlayer;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= AddPlayer;
    }

    private void AddPlayer(PlayerInput player)
    {
        player.transform.position = startingPoints[Players.Count].position;
        player.GetComponent<Bomberman>().PlayerId = "Player " + (Players.Count + 1);
        player.GetComponent<Animator>().runtimeAnimatorController = playerAnimators[Players.Count];
        MainMenu.Instance.enterPlayerMenu(Players.Count);
        Players.Add(player);
    }
}
