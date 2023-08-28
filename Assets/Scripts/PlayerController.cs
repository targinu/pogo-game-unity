using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed = 5.0f; //velocidade de movimento
    public float jumpForce = 5.0f; //altura do pulo

    public AudioClip slimeJump; //referencia ao som de pulo

    private AudioSource audioSource; //componente AudioSource

    private bool isGrounded = false; //indica se o jogador está no chão

    private bool isMoving = false;
    private Vector3 targetPosition;

    private void Start()
    {
        targetPosition = transform.position;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {

        //verifica se o jogador está no chão
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        if (!isMoving)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Verifica se o jogador está tentando se mover apenas na horizontal ou vertical
            if (horizontalInput != 0 && verticalInput == 0)
            {
                Vector3 direction = new Vector3(horizontalInput, 0, 0).normalized;
                targetPosition = transform.position + direction;

                // Aplica o pulo ao movimento (adiciona força no eixo Y)
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                isMoving = true;
            }
            else if (horizontalInput == 0 && verticalInput != 0)
            {
                Vector3 direction = new Vector3(0, 0, verticalInput).normalized;
                targetPosition = transform.position + direction;

                // Aplica o pulo ao movimento (adiciona força no eixo Y)
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                isMoving = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);

            //atualiza a posição do jogador usando Rigidbody
            rb.MovePosition(newPosition);

            //verifica se chegou à posição de destino
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                //atualiza a posição de destino para a posição atual
                targetPosition = transform.position;

                //encerra o movimento
                isMoving = false;

                //toca o som de pulo se o jogador estiver no chão
                if (isGrounded && audioSource != null && slimeJump != null)
                {
                    audioSource.PlayOneShot(slimeJump);
                }
            }
        }
    }
}