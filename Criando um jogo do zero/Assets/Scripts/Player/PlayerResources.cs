using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerResources : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textCoin;
    public static PlayerResources Instance { get; private set; }

    private int coins;
    private int life;

    void Awake()
    {
        if(Instance != null)
            return;
        
        Instance = this;
        coins = 0;
        life = 100;
        textCoin.text = $"Coins: {coins}";
    }

    public void AddCoin()
    {
        coins++;
        textCoin.text = "Coins: " + coins;
    }
}
