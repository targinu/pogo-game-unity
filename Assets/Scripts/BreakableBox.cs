using UnityEngine;

public class BreakableBox : MonoBehaviour
{
    public int pointsOnBreak = 50; // Pontos a serem concedidos ao quebrar a caixa
    private GameManager gameManager; // Referência ao GameManager para acessar a pontuação

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Verifica se colidiu com o jogador
        {
            // Calcula os pontos com base na distância percorrida pelo jogador até a caixa
            float distance = Vector3.Distance(collision.transform.position, transform.position);
            int earnedPoints = Mathf.RoundToInt(distance); // Arredonda para o valor inteiro mais próximo

            gameManager.AddScore(earnedPoints); // Adiciona os pontos ao GameManager
            Destroy(gameObject); // Destroi a caixa

            Debug.Log("Caixa Destruida" + earnedPoints);
        }
    }
}
