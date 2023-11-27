using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int pendingScore = 0; //pontuação pendente a ser concedida ao colidir com uma caixa
    private bool canScore = false; //indica se o jogador pode marcar pontos
    private GameManager gameManager; //referência ao GameManager para acessar a pontuação

    private AudioSource audioSource; //componente de áudio para reproduzir o som

    private Renderer playerRenderer; //acessa a cor do jogador

    private Rigidbody rb; //componente Rigidbody do jogador
    private Vector3 targetPosition; //posição do próximo bloco que o jogador vai se mover
    private bool isMoving = false; //indica se o jogador está se movendo
    private bool isJumping = false; //indica se o jogador está pulando

    public float moveSpeed = 5.0f; //velocidade de movimento
    public float jumpForce = 5.0f; //força do pulo

    public AudioClip slimeJump; //som a ser reproduzido ao atingir o chão
    public AudioClip breakingCrate; //som a ser reproduzido ao atingir uma caixa

    public int playerIndex = 0; //atributo para identificar o jogador
    public int PlayerIndex { get; private set; } //propriedade para acessar o índice do jogador
    private KeybindManager keybindManager; //referência ao KeybindManager

    public float speedReductionMultiplier = 0.5f; //fator de redução de velocidade
    public float speedBoostMultiplier = 1.5f; //fator de aumento de velocidade
    private float originalMoveSpeed; //armazena a velocidade original antes dos efeitos
    private bool isUnderPowerUp = false; //indica se o jogador está sob o efeito de um power-up
    private float powerUpDuration = 5.0f; //duração do efeito de power-up em segundos
    private float currentPowerUpTimer = 0.0f; //temporizador atual do efeito de power-up

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); //encontra o GameManager na cena
        rb = GetComponent<Rigidbody>(); //obtém o componente Rigidbody do jogador
        targetPosition = transform.position; //inicializa a posição de destino como a posição atual do jogador
        audioSource = GetComponent<AudioSource>(); //obtém o componente AudioSource do jogador

        //obtém o Renderer do jogador
        playerRenderer = GetComponent<Renderer>();

        //obtém o KeybindManager na cena
        keybindManager = FindObjectOfType<KeybindManager>();

        //cria uma referência ao UIScore associado a este jogador
        UIScore uiScore = GetComponent<UIScore>();
        if (uiScore != null)
        {
            PlayerIndex = playerIndex; //define o valor da propriedade PlayerIndex
            uiScore.SetPlayerIndex(PlayerIndex); //passe PlayerIndex para UIScore

        }

        Vector3[] startingPositions = new Vector3[]
        {
            new Vector3(-3.5f, 0.8f, -3.5f), //jogador 1
            new Vector3(-3.5f, 0.8f, 3.5f),  //jogador 2
            new Vector3(3.5f, 0.8f, 3.5f),  //jogador 3
            new Vector3(3.5f, 0.8f, -3.5f)   //jogador 4
        };

        transform.position = startingPositions[playerIndex];
    }

    private void Update()
    {
        if (!isMoving)
        {
            //use as teclas configuradas pelo KeybindManager para mover o jogador
            float horizontalInput = 0;
            float verticalInput = 0;

            if (playerIndex >= 0 && playerIndex < keybindManager.PlayerKeyBindings.Count)
            {
                horizontalInput = Input.GetKey(keybindManager.PlayerKeyBindings[playerIndex].MoveRightKey)
                    ? 1
                    : (Input.GetKey(keybindManager.PlayerKeyBindings[playerIndex].MoveLeftKey) ? -1 : 0);

                verticalInput = Input.GetKey(keybindManager.PlayerKeyBindings[playerIndex].MoveUpKey)
                    ? 1
                    : (Input.GetKey(keybindManager.PlayerKeyBindings[playerIndex].MoveDownKey) ? -1 : 0);
            }

            //resto do código para movimentação
            if (!isMoving)
            {
                if (horizontalInput != 0 && verticalInput == 0)
                {
                    Move(Vector3.right * Mathf.Sign(horizontalInput)); //move para a direita ou esquerda
                }
                else if (horizontalInput == 0 && verticalInput != 0)
                {
                    Move(Vector3.forward * Mathf.Sign(verticalInput)); //move para frente ou trás
                }


                // Verifica se o jogador está sob o efeito de um power-up
                if (isUnderPowerUp)
                {
                    currentPowerUpTimer -= Time.deltaTime;

                    // Se o temporizador atingir zero, reverte o efeito de power-up
                    if (currentPowerUpTimer <= 0.0f)
                    {
                        ResetPowerUpEffect();
                    }
                }
            }
        }

        if (isJumping)
        {
            //aplica a força de pulo enquanto o jogador está no ar
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = false;
        }
    }

    private void Move(Vector3 direction)
    {
        //calcula a próxima posição de destino
        Vector3 nextPosition = targetPosition + direction;

        if (CanMoveTo(nextPosition))
        {
            targetPosition = nextPosition; //atualiza a posição de destino
            isMoving = true;
            isJumping = true;
        }
    }

    private bool CanMoveTo(Vector3 position)
    {
        //verifica se a próxima posição está dentro dos limites do grid
        float gridSize = 8.0f; //tamanho do grid
        float halfGridSize = gridSize / 2.0f;

        //verifica se a posição está dentro dos limites do grid e retorna true se estiver
        bool isInsideGrid = Mathf.Abs(position.x) <= halfGridSize && Mathf.Abs(position.z) <= halfGridSize;

        if (!isInsideGrid)
        {
            return false; //posição fora do grid
        }

        //verifica se o bloco que o jogador vai pular possui outro jogador pela tag "Player"
        Collider[] colliders = Physics.OverlapSphere(position, 0.2f);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                return false; //não pode pular para um bloco com outro jogador
            }
        }

        return true; //pode mover-se para a posição
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            //move gradualmente o jogador em direção à posição de destino
            Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);

            //atualiza a posição do jogador usando Rigidbody
            rb.MovePosition(newPosition);

            //verifica se chegou à posição de destino
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                //encerra o movimento
                isMoving = false;

                //se houver pontos pendentes e o jogador pode marcar pontos, adiciona-os ao GameManager e ao jogador
                if (pendingScore > 0 && canScore)
                {
                    gameManager.AddScore(PlayerIndex, pendingScore); //use PlayerIndex aqui
                    pendingScore = 0; //limpa os pontos pendentes
                    canScore = false; //reinicia a capacidade de marcar pontos
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grid"))
        {
            Renderer blockRenderer = collision.gameObject.GetComponent<Renderer>();

            //obtem o material do jogador
            Material playerMaterial = playerRenderer.material;

            //verifica se o bloco já não foi pintado da cor do jogador
            if (blockRenderer.material.color != playerMaterial.color)
            {
                blockRenderer.material.color = playerMaterial.color;

                //reproduz o som "slimeJump"
                if (audioSource != null && slimeJump != null)
                {
                    audioSource.PlayOneShot(slimeJump);
                }
            }
        }
        else if (collision.gameObject.CompareTag("Box"))
        {
            canScore = true; //o jogador agora pode marcar pontos

            //reproduz o som "breakingCrate"
            if (audioSource != null && breakingCrate != null)
            {
                audioSource.PlayOneShot(breakingCrate);
            }

            //limpa a área previamente pintada
            GameObject[] paintedBlocks = GameObject.FindGameObjectsWithTag("Grid");
            foreach (GameObject block in paintedBlocks)
            {
                Renderer blockRenderer = block.GetComponent<Renderer>();
                if (blockRenderer.material.color == playerRenderer.material.color)
                {
                    blockRenderer.material.color = Color.white;
                }
            }
        }
    }

    public Color GetPlayerColor()
    {
        return playerRenderer.material.color;
    }

    public void ApplySpeedReduction()
    {
        // Verifica se já está sob o efeito de SpeedReduction
        if (!IsUnderSpeedReduction())
        {
            // Armazena a velocidade original
            originalMoveSpeed = moveSpeed;

            // Aplica o efeito de redução de velocidade
            moveSpeed *= speedReductionMultiplier;

            //define que o jogador está sob o efeito de um power-up
            SetUnderPowerUp();
            Debug.Log("Speed Reduction activated!");
        }
    }

    public void ApplySpeedBoost()
    {
        // Verifica se já está sob o efeito de SpeedBoost
        if (!IsUnderSpeedBoost())
        {
            // Armazena a velocidade original
            originalMoveSpeed = moveSpeed;

            // Aplica o efeito de aumento de velocidade
            moveSpeed *= speedBoostMultiplier;

            // Define que o jogador está sob o efeito de um power-up
            SetUnderPowerUp();
            Debug.Log("Speed Boost activated!");
        }
    }

    public void ResetSpeed()
    {
        // Retorna à velocidade original
        moveSpeed = originalMoveSpeed;
    }

    // Verifica se o jogador está sob o efeito de SpeedReduction
    public bool IsUnderSpeedReduction()
    {
        return moveSpeed < originalMoveSpeed;
    }

    // Verifica se o jogador está sob o efeito de SpeedBoost
    public bool IsUnderSpeedBoost()
    {
        return moveSpeed > originalMoveSpeed;
    }

    public bool IsUnderPowerUp()
    {
        return isUnderPowerUp;
    }

    private void SetUnderPowerUp()
    {
        isUnderPowerUp = true;
        currentPowerUpTimer = powerUpDuration;
    }

    public void ResetPowerUpEffect()
    {
        // Retorna à velocidade original
        moveSpeed = originalMoveSpeed;

        // Reinicia as variáveis de controle de power-up
        isUnderPowerUp = false;
        currentPowerUpTimer = 0.0f;

        Debug.Log("Power-up effect ended.");
    }
}
