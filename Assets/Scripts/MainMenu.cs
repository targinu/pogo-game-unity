using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuUI; //referência ao painel de opções principal

    public GameObject StartGameOptions; //referência ao painel de opções de inicialização

    private int numberOfPlayers = 1; //variável para rastrear o número de jogadores selecionados

    public GameObject[] playerSelectionButtons; //array para armazenar os botões de seleção de jogadores

    private GameObject selectedButton; //variável para rastrear o botão selecionado

    //função para carregar a cena "Pogo"
    public void StartGame()
    {
        SceneManager.LoadScene("Pogo", LoadSceneMode.Single); //certifica-se de que o modo de carregamento é definido como Single
    }

    //função para sair do jogo
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    //função para definir o número de jogadores com base no botão clicado
    public void SetNumberOfPlayers(int players)
    {
        numberOfPlayers = players;
        HighlightSelectedButton(playerSelectionButtons[players - 1]);
    }

    //função para destacar o botão selecionado
    private void HighlightSelectedButton(GameObject buttonToHighlight)
    {
        foreach (GameObject button in playerSelectionButtons)
        {
            //restaura a cor padrão de todos os botões (branco)
            button.GetComponent<Button>().image.color = Color.white;
        }

        //destaca o botão selecionado com uma cor diferente (por exemplo, cinza)
        buttonToHighlight.GetComponent<Button>().image.color = Color.gray;

        //atualiza o botão selecionado
        selectedButton = buttonToHighlight;
    }

    //função para abrir o menu de seleção de número de jogadores
    public void OpenStartOptionsMenu()
    {
        StartGameOptions.SetActive(true); //ativa o painel de opções no Unity
        MainMenuUI.SetActive(false); //desativa o painel de configurações principal no Unity
    }

    //função para voltar para o menu principal
    public void BackToMainMenu()
    {
        StartGameOptions.SetActive(false); //desativa o painel de opções no Unity
        MainMenuUI.SetActive(true); //ativa o painel de configurações principal no Unity
    }
}