using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public List<PlayerData> players = new List<PlayerData>(); // Lista de jogadores

    [System.Serializable]
    public class PlayerData
    {
        public int score = 0;
        public GameObject playerObject;
    }

    private void Start()
    {

        for (int i = 0; i < players.Count; i++)
        {
            players[i].playerObject.GetComponent<PlayerController>().playerIndex = i;
        }

    }

    public void AddScore(int playerIndex, int amount)
    {
        if (playerIndex >= 0 && playerIndex < players.Count)
        {
            players[playerIndex].score += amount;
        }
        else
        {
            Debug.LogError("Invalid playerIndex: " + playerIndex);
        }
    }
}
