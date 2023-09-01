using TMPro;
using UnityEngine;

public class UIScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText; //referência ao componente TextMeshProUGUI no Inspector
    private GameManager gameManager;  //referência ao GameManager para acessar a pontuação

    private int playerIndex; //índice do jogador associado a este UIScore

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (playerIndex >= 0 && playerIndex < gameManager.players.Count) //verifique se o índice é válido
        {
            scoreText.text = "SCORE P" + (playerIndex + 1) + ": " + gameManager.players[playerIndex].score.ToString();
        }
    }


    public void SetPlayerIndex(int index)
    {
        playerIndex = index;
    }
}
