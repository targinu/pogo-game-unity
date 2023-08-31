using UnityEngine;

public class BreakableBox : MonoBehaviour
{
    private int paintedBlockCount = 0; //contagem de blocos pintados quando o jogador colide
    private GameManager gameManager; //referência ao GameManager para acessar a pontuação

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) //verifica se colidiu com o jogador
        {
            //calcula os pontos com base na quantidade de blocos pintados
            gameManager.AddScore(paintedBlockCount);
            Destroy(gameObject); //destroi a caixa
        }
    }

    public void SetPaintedBlockCount(int count)
    {
        paintedBlockCount = count;
    }
}
