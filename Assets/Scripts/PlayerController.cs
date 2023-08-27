using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed = 1.0f; // Velocidade de movimento
    public float jumpForce = 7.0f;

    private bool isMoving = false;
    private Vector3 targetPosition;

    private void Start()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (!isMoving)
        {
            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

            if (input != Vector3.zero)
            {
                targetPosition = transform.position + input;

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

            rb.MovePosition(newPosition);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                targetPosition = transform.position;
                isMoving = false;
            }
        }
    }
}
