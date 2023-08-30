using TMPro;
using UnityEngine;

public class UIScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText; //referência ao componente TextMeshProUGUI no Inspector
    private GameManager gameManager;  //referência ao GameManager para acessar a pontuação

    private void Start()
    {
        //encontra uma instância do GameManager na cena e armazena a referência
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        //atualiza o texto da pontuação usando a referência ao TextMeshProUGUI e a pontuação do GameManager
        scoreText.text = "Score: " + gameManager.score;
    }
}