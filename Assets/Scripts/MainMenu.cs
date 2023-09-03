using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Função para carregar a cena "Pogo"
    public void StartGame()
    {
        SceneManager.LoadScene("Pogo");
    }

    // Função para sair do jogo
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
