using UnityEngine;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    //lista de jogadores e suas informações
    public List<PlayerData> players = new List<PlayerData>(); // Lista de jogadores

    //classe que armazena dados de cada jogador
    [System.Serializable]
    public class PlayerData
    {
        public int score = 0; //pontuação do jogador
        public GameObject playerObject; //referência ao objeto do jogador
    }

    private void Start()
    {
        //no início do jogo, atribui os índices aos jogadores com base na ordem na lista
        for (int i = 0; i < players.Count; i++)
        {
            players[i].playerObject.GetComponent<PlayerController>().playerIndex = i;
        }

    }

    //método para adicionar pontuação a um jogador
    public void AddScore(int playerIndex, int amount)
    {
        //verifica se o índice do jogador é válido
        if (playerIndex >= 0 && playerIndex < players.Count)
        {
            //adiciona a quantidade especificada à pontuação do jogador
            players[playerIndex].score += amount;
        }
        else
        {
            //se o índice do jogador for inválido, registra um erro
            Debug.LogError("Invalid playerIndex: " + playerIndex);
        }
    }
}
