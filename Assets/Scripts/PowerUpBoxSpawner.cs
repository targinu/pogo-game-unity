using UnityEngine;

public class PowerUpBoxSpawner : MonoBehaviour
{
    public GameObject powerUpBoxPrefab; // Prefab da caixa de power-up que será instanciada
    public float spawnInterval = 10.0f; // Intervalo de tempo entre cada spawn
    public float boxLifetime = 5.0f; // Tempo de vida de cada caixa de power-up
    public int maxConcurrentBoxes = 4; // Número máximo de caixas de power-up simultâneas
    public float minBoxDistance = 2.0f; // Distância mínima entre caixas de power-up

    private float timeSinceLastSpawn = 0.0f; // Tempo decorrido desde o último spawn
    private GameObject[] currentBoxes; // Array para armazenar as caixas de power-up atuais no jogo
    private GameObject[] players; // Array para armazenar referências aos jogadores

    public AudioSource sfxAudioSource; // Referência ao AudioSource para o efeito sonoro
    public AudioClip boxSpawnSFX; // Efeito sonoro a ser reproduzido ao spawnar uma caixa de power-up

    private void Start()
    {
        currentBoxes = new GameObject[maxConcurrentBoxes]; // Inicializa o array de caixas de power-up
        players = GameObject.FindGameObjectsWithTag("Player"); // Encontra todos os objetos com a tag "Player"
    }

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime; // Atualiza o tempo decorrido desde o último frame

        for (int i = 0; i < maxConcurrentBoxes; i++)
        {
            if (currentBoxes[i] == null) // Verifica se a posição atual no array está vazia (sem caixa de power-up)
            {
                SpawnBox(i); // Chama a função para criar uma nova caixa de power-up
                timeSinceLastSpawn = 0.0f; // Reseta o tempo decorrido
            }
        }
    }

    void SpawnBox(int boxIndex)
    {
        Vector3 spawnPosition = FindValidSpawnPosition(); // Encontra uma posição válida para spawnar a caixa de power-up

        if (spawnPosition != Vector3.zero) // Verifica se a posição de spawn é válida
        {
            currentBoxes[boxIndex] = Instantiate(powerUpBoxPrefab, spawnPosition, Quaternion.identity); // Instancia a caixa de power-up na posição encontrada
            Destroy(currentBoxes[boxIndex], boxLifetime); // Destrói a caixa de power-up após o tempo de vida definido

            // Toca o efeito sonoro de spawn da caixa de power-up
            sfxAudioSource.PlayOneShot(boxSpawnSFX);
        }
    }

    Vector3 FindValidSpawnPosition()
    {
        int maxAttempts = 10; // Número máximo de tentativas para encontrar uma posição válida

        while (maxAttempts > 0)
        {
            Vector3 randomPoint = new Vector3(Random.Range(-3.5f, 3.5f), 3.0f, Random.Range(-3.5f, 3.5f)); // Gera um ponto aleatório
            float snappedX = Mathf.Floor(randomPoint.x) + 0.5f; // Arredonda a coordenada X
            float snappedZ = Mathf.Floor(randomPoint.z) + 0.5f; // Arredonda a coordenada Z
            Vector3 spawnPosition = new Vector3(snappedX, randomPoint.y, snappedZ); // Define a posição de spawn

            Ray ray = new Ray(spawnPosition + Vector3.up * 10.0f, Vector3.down); // Cria um raio para verificar o chão abaixo da posição
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 20.0f, LayerMask.GetMask("Ground"))) // Verifica se há chão abaixo
            {
                if (Vector3.Distance(hit.point, spawnPosition) < minBoxDistance) // Verifica se a caixa de power-up está muito próxima do chão
                {
                    maxAttempts--;
                    continue; // Tenta uma nova posição
                }
            }

            Collider[] colliders = Physics.OverlapSphere(spawnPosition, minBoxDistance); // Verifica colisões próximas

            bool isValidPosition = true;

            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Box") || collider.CompareTag("Player")) // Verifica se há uma caixa ou jogador próximo
                {
                    isValidPosition = false; // A posição não é válida
                    break;
                }
            }

            if (isValidPosition)
            {
                bool isTooClose = false;

                foreach (var box in currentBoxes)
                {
                    if (box != null && Vector3.Distance(box.transform.position, spawnPosition) < minBoxDistance) // Verifica se a nova caixa de power-up está muito próxima de caixas de power-up existentes
                    {
                        isTooClose = true; // A posição não é válida
                        break;
                    }
                }

                if (!isTooClose) // Se a nova caixa de power-up não estiver muito próxima de outras caixas de power-up
                {
                    return spawnPosition; // Retorna a posição de spawn válida
                }
            }

            maxAttempts--;
        }

        return Vector3.zero; // Retorna uma posição padrão se nenhuma posição válida for encontrada
    }
}
