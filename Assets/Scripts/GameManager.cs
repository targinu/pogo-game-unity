using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int score = 0; //a pontuação do jogo, inicializada como 0

    public void AddScore(int amount)
    {
        score += amount; //adiciona a quantidade especificada à pontuação atual
    }
}
