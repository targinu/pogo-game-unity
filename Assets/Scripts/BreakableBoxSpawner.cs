using UnityEngine;

public class BreakableBoxSpawner : MonoBehaviour
{
    public GameObject breakableBoxPrefab; //prefab da caixa que será instanciada
    public float spawnInterval = 10.0f; //intervalo de tempo entre cada spawn
    public float boxLifetime = 5.0f; //tempo de vida de cada caixa
    public int maxConcurrentBoxes = 4; //número máximo de caixas simultâneas
    public float minBoxDistance = 2.0f; //distância mínima entre caixas

    private float timeSinceLastSpawn = 0.0f; //tempo decorrido desde o último spawn
    private GameObject[] currentBoxes; //array para armazenar as caixas atuais no jogo
    private GameObject[] players; //array para armazenar referências aos jogadores

    public AudioSource sfxAudioSource; //referência ao AudioSource para o efeito sonoro
    public AudioClip boxSpawnSFX; //o efeito sonoro a ser reproduzido ao spawnar uma caixa

    private void Start()
    {
        currentBoxes = new GameObject[maxConcurrentBoxes]; //inicializa o array de caixas
        players = GameObject.FindGameObjectsWithTag("Player"); //encontra todos os objetos com a tag "Player"
    }

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime; //atualiza o tempo decorrido desde o último frame

        for (int i = 0; i < maxConcurrentBoxes; i++)
        {
            if (currentBoxes[i] == null) //verifica se a posição atual no array está vazia (sem caixa)
            {
                SpawnBox(i); //chama a função para criar uma nova caixa
                timeSinceLastSpawn = 0.0f; //reseta o tempo decorrido
            }
        }
    }

    void SpawnBox(int boxIndex)
    {
        Vector3 spawnPosition = FindValidSpawnPosition(); //encontra uma posição válida para spawnar a caixa

        if (spawnPosition != Vector3.zero) //verifica se a posição de spawn é válida
        {
            currentBoxes[boxIndex] = Instantiate(breakableBoxPrefab, spawnPosition, Quaternion.identity); //instancia a caixa na posição encontrada
            Destroy(currentBoxes[boxIndex], boxLifetime); //destroi a caixa após o tempo de vida definido

            //toca o efeito sonoro de spawn da caixa
            sfxAudioSource.PlayOneShot(boxSpawnSFX);
        }
    }

    Vector3 FindValidSpawnPosition()
    {
        int maxAttempts = 10; //número máximo de tentativas para encontrar uma posição válida

        while (maxAttempts > 0)
        {
            Vector3 randomPoint = new Vector3(Random.Range(-3.5f, 3.5f), 3.0f, Random.Range(-3.5f, 3.5f)); //gera um ponto aleatório
            float snappedX = Mathf.Floor(randomPoint.x) + 0.5f; //arredonda a coordenada X
            float snappedZ = Mathf.Floor(randomPoint.z) + 0.5f; //arredonda a coordenada Z
            Vector3 spawnPosition = new Vector3(snappedX, randomPoint.y, snappedZ); //define a posição de spawn

            Ray ray = new Ray(spawnPosition + Vector3.up * 10.0f, Vector3.down); //cria um raio para verificar o chão abaixo da posição
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 20.0f, LayerMask.GetMask("Ground"))) //verifica se há chão abaixo
            {
                if (Vector3.Distance(hit.point, spawnPosition) < minBoxDistance) //verifica se a caixa está muito próxima do chão
                {
                    maxAttempts--;
                    continue; //tenta uma nova posição
                }
            }

            Collider[] colliders = Physics.OverlapSphere(spawnPosition, minBoxDistance); //verifica colisões próximas

            bool isValidPosition = true;

            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Box") || collider.CompareTag("Player")) //verifica se há uma caixa ou jogador próximo
                {
                    isValidPosition = false; //a posição não é válida
                    break;
                }
            }

            if (isValidPosition)
            {
                bool isTooClose = false;

                foreach (var box in currentBoxes)
                {
                    if (box != null && Vector3.Distance(box.transform.position, spawnPosition) < minBoxDistance) //verifica se a nova caixa está muito próxima de caixas existentes
                    {
                        isTooClose = true; //a posição não é válida
                        break;
                    }
                }

                if (!isTooClose) //se a nova caixa não estiver muito próxima de outras caixas
                {
                    return spawnPosition; //retorna a posição de spawn válida
                }
            }

            maxAttempts--;
        }

        return Vector3.zero; //retorna uma posição padrão se nenhuma posição válida for encontrada
    }
}