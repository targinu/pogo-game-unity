using UnityEngine;

public class BreakableBox : MonoBehaviour
{
    private int paintedBlocksHit = 0; //contador de blocos pintados atingidos
    private GameManager gameManager; //referência ao GameManager para acessar a pontuação

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); //encontra o GameManager na cena
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) //verifica se colidiu com o jogador
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>(); //obtém o componente PlayerController do jogador
            Color playerColor = player.GetPlayerColor(); //obtém a cor do jogador

            GameObject[] paintedBlocks = GameObject.FindGameObjectsWithTag("Grid"); //encontra todos os blocos do tipo "Grid" na cena
            paintedBlocksHit = 0; //reinicializa o contador de blocos pintados atingidos

            foreach (GameObject block in paintedBlocks)
            {
                Renderer blockRenderer = block.GetComponent<Renderer>();
                if (blockRenderer.material.color == playerColor) //verifica se a cor do bloco é a mesma do jogador
                {
                    paintedBlocksHit++; //incrementa o contador de blocos pintados atingidos
                }
            }

            //adiciona a pontuação ao jogador com base na quantidade de blocos pintados atingidos
            gameManager.AddScore(player.PlayerIndex, paintedBlocksHit);

            Destroy(gameObject); //destroi a caixa após a colisão
        }
    }
}
