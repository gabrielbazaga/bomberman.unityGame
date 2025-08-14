using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpRespawnController : MonoBehaviour
{
    [SerializeField] private static PowerUpRespawnController instance;
    [SerializeField] private int dropChance;
    private int digit;

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
    }

    public int SortPowerUp()
    {
        int randomNumber = Random.Range(0, 101);
        int powerType = 0;
        if (randomNumber <= dropChance)
        {
            powerType = Random.Range(0, 6);
            return powerType;
           
        } 
            //0 is no power to drop
            return powerType;       
    }

    public static PowerUpRespawnController Instance { get => instance; set => instance = value; }
    public int DropChance { get => dropChance; set => dropChance = value; }
    public int Digit { get => digit; set => digit = value; }
}
