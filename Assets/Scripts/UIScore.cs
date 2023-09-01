using TMPro;
using UnityEngine;

public class UIScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText; //referência ao componente TextMeshProUGUI no Inspector
    public int playerIndex; //índice do jogador associado a este UIScore
    private GameManager gameManager;  //referência ao GameManager para acessar a pontuação

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        //verifica se o índice é válido
        if (playerIndex >= 0 && playerIndex < gameManager.players.Count)
        {
            scoreText.text = "SCORE P" + (playerIndex + 1) + ": " + gameManager.players[playerIndex].score.ToString();
        }
    }

    //função para definir o playerIndex a partir do Inspector
    public void SetPlayerIndex(int index)
    {
        playerIndex = index;
    }
}
