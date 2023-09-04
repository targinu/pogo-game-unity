using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //função para carregar a cena "Pogo"
    public void StartGame()
    {
        SceneManager.LoadScene("Pogo");
    }

    //função para sair do jogo
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
