using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class PlayerKeyBindings
{
    public KeyCode MoveLeftKey;
    public KeyCode MoveRightKey;
    public KeyCode MoveUpKey;
    public KeyCode MoveDownKey;
}

public class KeybindManager : MonoBehaviour
{
    //referências aos rótulos dos botões de movimento para os quatro jogadores
    [Header("Player 1 Buttons")]
    [SerializeField] private TextMeshProUGUI moveLeftP1lbl;
    [SerializeField] private TextMeshProUGUI moveRightP1lbl;
    [SerializeField] private TextMeshProUGUI moveUpP1lbl;
    [SerializeField] private TextMeshProUGUI moveDownP1lbl;

    [Header("Player 2 Buttons")]
    [SerializeField] private TextMeshProUGUI moveLeftP2lbl;
    [SerializeField] private TextMeshProUGUI moveRightP2lbl;
    [SerializeField] private TextMeshProUGUI moveUpP2lbl;
    [SerializeField] private TextMeshProUGUI moveDownP2lbl;

    [Header("Player 3 Buttons")]
    [SerializeField] private TextMeshProUGUI moveLeftP3lbl;
    [SerializeField] private TextMeshProUGUI moveRightP3lbl;
    [SerializeField] private TextMeshProUGUI moveUpP3lbl;
    [SerializeField] private TextMeshProUGUI moveDownP3lbl;

    [Header("Player 4 Buttons")]
    [SerializeField] private TextMeshProUGUI moveLeftP4lbl;
    [SerializeField] private TextMeshProUGUI moveRightP4lbl;
    [SerializeField] private TextMeshProUGUI moveUpP4lbl;
    [SerializeField] private TextMeshProUGUI moveDownP4lbl;

    //lista de nomes de botões para todos os jogadores e direções
    private List<string> buttonNames = new List<string>
    {
        "MoveLeftP1Button", "MoveRightP1Button", "MoveUpP1Button", "MoveDownP1Button",
        "MoveLeftP2Button", "MoveRightP2Button", "MoveUpP2Button", "MoveDownP2Button",
        "MoveLeftP3Button", "MoveRightP3Button", "MoveUpP3Button", "MoveDownP3Button",
        "MoveLeftP4Button", "MoveRightP4Button", "MoveUpP4Button", "MoveDownP4Button"
    };

    //lista de configurações de teclas para cada jogador
    public List<PlayerKeyBindings> PlayerKeyBindings = new List<PlayerKeyBindings>();


    private void Start()
    {
        //carrega as configurações de teclas salvas para todos os botões ao iniciar o jogo
        foreach (string buttonName in buttonNames)
        {
            LoadKeybind(buttonName);
        }

        LoadKeyBindings(); //carregar configurações de teclas salvas

        //atualiza os TextMeshProUGUIs com os valores das configurações de teclas ao iniciar o jogo
        UpdateTextMeshProValues();
    }

    private void Update()
    {
        foreach (string buttonName in buttonNames)
        {
            TextMeshProUGUI buttonLabel = GetButtonLabel(buttonName);

            // Verifica se o rótulo do botão está aguardando uma nova entrada de tecla
            if (buttonLabel.text == "Awaiting Input")
            {
                foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKey(keycode))
                    {
                        // Configura a tecla pressionada como a nova tecla para o botão
                        buttonLabel.text = keycode.ToString();

                        // Salva a nova tecla nas preferências do jogador
                        SaveKeybind(buttonName, keycode.ToString());

                        // Atualiza as configurações de teclas com base na nova entrada de tecla
                        UpdateKeyBindingsFromLabels();
                    }
                }
            }
        }
    }

    public void ChangeKey(TextMeshProUGUI buttonLabel)
    {
        //chamado quando um jogador deseja reconfigurar uma tecla, redefine o rótulo do botão para "Awaiting Input"
        buttonLabel.text = "Awaiting Input";
    }

    private TextMeshProUGUI GetButtonLabel(string buttonName)
    {
        //mapeia o nome do botão para o objeto TextMeshProUGUI correspondente
        switch (buttonName)
        {
            case "MoveLeftP1Button":
                return moveLeftP1lbl;
            case "MoveRightP1Button":
                return moveRightP1lbl;
            case "MoveUpP1Button":
                return moveUpP1lbl;
            case "MoveDownP1Button":
                return moveDownP1lbl;

            case "MoveLeftP2Button":
                return moveLeftP2lbl;
            case "MoveRightP2Button":
                return moveRightP2lbl;
            case "MoveUpP2Button":
                return moveUpP2lbl;
            case "MoveDownP2Button":
                return moveDownP2lbl;

            case "MoveLeftP3Button":
                return moveLeftP3lbl;
            case "MoveRightP3Button":
                return moveRightP3lbl;
            case "MoveUpP3Button":
                return moveUpP3lbl;
            case "MoveDownP3Button":
                return moveDownP3lbl;

            case "MoveLeftP4Button":
                return moveLeftP4lbl;
            case "MoveRightP4Button":
                return moveRightP4lbl;
            case "MoveUpP4Button":
                return moveUpP4lbl;
            case "MoveDownP4Button":
                return moveDownP4lbl;

            default:
                return null;
        }
    }

    private void LoadKeybind(string buttonName)
    {
        //carrega a configuração de tecla salva para um botão específico nas preferências do jogador
        string savedKey = PlayerPrefs.GetString(buttonName, "Not Assigned");
        GetButtonLabel(buttonName).text = savedKey;
    }

    private void SaveKeybind(string buttonName, string key)
    {
        //salva a configuração de tecla para um botão específico nas preferências do jogador
        PlayerPrefs.SetString(buttonName, key);
        PlayerPrefs.Save();
    }

    //método para carregar as configurações de teclas
    public void LoadKeyBindings()
    {
        for (int i = 0; i < PlayerKeyBindings.Count; i++)
        {
            string key = "Player" + (i + 1) + "KeyBindings";
            string serializedBindings = PlayerPrefs.GetString(key, string.Empty);

            if (!string.IsNullOrEmpty(serializedBindings))
            {
                PlayerKeyBindings[i] = JsonUtility.FromJson<PlayerKeyBindings>(serializedBindings);
            }
        }
    }

    private void UpdateTextMeshProValues()
    {
        for (int i = 0; i < PlayerKeyBindings.Count; i++)
        {
            TextMeshProUGUI moveLeftLabel = GetButtonLabel("MoveLeftP" + (i + 1) + "Button");
            TextMeshProUGUI moveRightLabel = GetButtonLabel("MoveRightP" + (i + 1) + "Button");
            TextMeshProUGUI moveUpLabel = GetButtonLabel("MoveUpP" + (i + 1) + "Button");
            TextMeshProUGUI moveDownLabel = GetButtonLabel("MoveDownP" + (i + 1) + "Button");

            moveLeftLabel.text = PlayerKeyBindings[i].MoveLeftKey.ToString();
            moveRightLabel.text = PlayerKeyBindings[i].MoveRightKey.ToString();
            moveUpLabel.text = PlayerKeyBindings[i].MoveUpKey.ToString();
            moveDownLabel.text = PlayerKeyBindings[i].MoveDownKey.ToString();
        }
    }

    //função para atualizar as configurações de teclas com base nas labels de texto
    private void UpdateKeyBindingsFromLabels()
    {
        for (int i = 0; i < PlayerKeyBindings.Count; i++)
        {
            TextMeshProUGUI moveLeftLabel = GetButtonLabel("MoveLeftP" + (i + 1) + "Button");
            TextMeshProUGUI moveRightLabel = GetButtonLabel("MoveRightP" + (i + 1) + "Button");
            TextMeshProUGUI moveUpLabel = GetButtonLabel("MoveUpP" + (i + 1) + "Button");
            TextMeshProUGUI moveDownLabel = GetButtonLabel("MoveDownP" + (i + 1) + "Button");

            PlayerKeyBindings[i].MoveLeftKey = (KeyCode)Enum.Parse(typeof(KeyCode), moveLeftLabel.text);
            PlayerKeyBindings[i].MoveRightKey = (KeyCode)Enum.Parse(typeof(KeyCode), moveRightLabel.text);
            PlayerKeyBindings[i].MoveUpKey = (KeyCode)Enum.Parse(typeof(KeyCode), moveUpLabel.text);
            PlayerKeyBindings[i].MoveDownKey = (KeyCode)Enum.Parse(typeof(KeyCode), moveDownLabel.text);
        }

        // Salve todas as configurações de teclas atualizadas
        SaveAllKeyBindings();
    }

    private void SaveAllKeyBindings()
    {
        for (int i = 0; i < PlayerKeyBindings.Count; i++)
        {
            string key = "Player" + (i + 1) + "KeyBindings";
            string serializedBindings = JsonUtility.ToJson(PlayerKeyBindings[i]);
            PlayerPrefs.SetString(key, serializedBindings);
        }

        PlayerPrefs.Save();
    }

}