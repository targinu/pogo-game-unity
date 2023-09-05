using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; //referência ao painel de pausa no Unity
    public GameObject panelScore; //referência ao painel de pontuação no Unity

    public GameObject settingsMenuUI; //referência ao painel de configuração no Unity
    public GameObject soundMenuUI; //referência ao painel de menu de som no Unity

    public GameObject controlsMenuUI; //referência ao painel de menu de controles no Unity

    private bool isPaused = false; //variável para rastrear se o jogo está pausado ou não

    void Update()
    {
        //verifica se a tecla Esc (Escape) foi pressionada
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (soundMenuUI.activeSelf)
            {
                //se o menu de som estiver ativo, volte para o menu de pausa
                BackToPauseMenu();
            }
            else if (controlsMenuUI.activeSelf)
            {
                //se o menu de configurações de controle estiver ativo, volte para o menu de pausa
                BackToPauseMenu();
            }
            else if (settingsMenuUI.activeSelf)
            {
                //se o menu de configurações estiver ativo, volte para o menu de pausa
                BackToPauseMenu();
            }
            else if (isPaused)
            {
                ResumeGame(); //se o jogo estiver pausado, retoma o jogo
            }
            else
            {
                PauseGame(); //se o jogo não estiver pausado, pausa o jogo
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); //desativa o painel de pausa no Unity
        Time.timeScale = 1f; //restaura o tempo para 1 (jogo em execução)
        isPaused = false; //define a variável de pausa como false
        panelScore.SetActive(true); //ativa o painel de pontuação no Unity
    }

    void PauseGame()
    {
        pauseMenuUI.SetActive(true); //ativa o painel de pausa no Unity
        Time.timeScale = 0f; //define o tempo como 0 (jogo pausado)
        isPaused = true; //define a variável de pausa como true
        panelScore.SetActive(false); //desativa o painel de pontuação no Unity
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; //certifica-se de restaurar o tempo antes de sair (importante para evitar problemas)
        SceneManager.LoadScene("Menu"); //carrega a cena do Menu no Unity
    }

    public void OpenSettingsMenu()
    {
        settingsMenuUI.SetActive(true); //ativa o painel de configurações no Unity
        pauseMenuUI.SetActive(false); //desativa o painel de pausa no Unity
    }

    public void BackToPauseMenu()
    {
        settingsMenuUI.SetActive(false); //desativa o painel de configurações no Unity
        soundMenuUI.SetActive(false); //desativa o painel de som no Unity
        controlsMenuUI.SetActive(false); //desativa o painel de controles no Unity
        pauseMenuUI.SetActive(true); //ativa o painel de pausa no Unity
    }

    public void OpenSoundMenu()
    {
        soundMenuUI.SetActive(true); //ativa o painel de som no Unity
        settingsMenuUI.SetActive(false); //desativa o painel de configurações no Unity
    }

    public void OpenControlsMenu()
    {
        controlsMenuUI.SetActive(true); //ativa o painel de controles no Unity
        settingsMenuUI.SetActive(false); //desativa o painel de configurações no Unity
    }

    public void BackToSettingsMenu()
    {
        soundMenuUI.SetActive(false); //desativa o painel de som no Unity
        controlsMenuUI.SetActive(false); //desativa o painel de controles no Unity
        pauseMenuUI.SetActive(false); //desativa o painel de pausa no Unity
        settingsMenuUI.SetActive(true); //ativa o painel de configurações no Unity
    }
}
