using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int paintedBlockCount = 0; //contagem de blocos pintados
    private int pendingScore = 0; //pontuação pendente a ser concedida ao colidir com uma caixa
    private bool canScore = false; //indica se o jogador pode marcar pontos
    private GameManager gameManager; //referência ao GameManager para acessar a pontuação

    private AudioSource audioSource; //componente de áudio para reproduzir o som

    private Rigidbody rb; //componente Rigidbody do jogador
    private Vector3 targetPosition; //posição do próximo bloco que o jogador vai se mover
    private bool isMoving = false; //indica se o jogador está se movendo
    private bool isJumping = false; //indica se o jogador está pulando

    public float moveSpeed = 5.0f; //velocidade de movimento
    public float jumpForce = 5.0f; //força do pulo

    public AudioClip slimeJump; //som a ser reproduzido ao atingir o chão
    public AudioClip breakingCrate; //som a ser reproduzido ao atingir uma caixa

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); //encontra o GameManager na cena
        rb = GetComponent<Rigidbody>(); //obtém o componente Rigidbody do jogador
        targetPosition = transform.position; //inicializa a posição de destino como a posição atual do jogador
        audioSource = GetComponent<AudioSource>(); //obtém o componente AudioSource do jogador
    }

    private void Update()
    {
        if (!isMoving)
        {
            float horizontalInput = Input.GetAxis("Horizontal"); //entrada horizontal do jogador
            float verticalInput = Input.GetAxis("Vertical"); //entrada vertical do jogador

            //verifica a entrada do jogador para mover-se na horizontal ou vertical
            if (horizontalInput != 0 && verticalInput == 0)
            {
                Move(Vector3.right * Mathf.Sign(horizontalInput)); //move para a direita ou esquerda
            }
            else if (horizontalInput == 0 && verticalInput != 0)
            {
                Move(Vector3.forward * Mathf.Sign(verticalInput)); //move para frente ou trás
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
        float gridSize = 8.0f; // Tamanho do grid
        float halfGridSize = gridSize / 2.0f;

        //verifica se a posição está dentro dos limites do grid e retorna true se estiver
        bool isInsideGrid = Mathf.Abs(position.x) <= halfGridSize && Mathf.Abs(position.z) <= halfGridSize;

        return isInsideGrid;
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
                    gameManager.AddScore(pendingScore);
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

            //verifica se o bloco já não foi pintado de vermelho
            if (blockRenderer.material.color != Color.red)
            {
                //acumula a contagem de blocos pintados
                paintedBlockCount++;

                //adiciona pontuação temporária (não imediatamente)
                pendingScore++;

                blockRenderer.material.color = Color.red;

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
            paintedBlockCount = 0; //reinicia a contagem de blocos pintados

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
                blockRenderer.material.color = Color.white;
            }
        }
    }
}
