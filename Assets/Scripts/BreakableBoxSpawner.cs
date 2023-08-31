using UnityEngine;

public class BreakableBoxSpawner : MonoBehaviour
{
    public GameObject breakableBoxPrefab; //prefab da caixa quebrável
    public float spawnInterval = 10.0f; //intervalo de tempo entre cada spawn
    public float boxLifetime = 5.0f; //tempo de vida da caixa quebrável
    public int maxConcurrentBoxes = 4; //número máximo de caixas que podem existir simultaneamente
    public float minBoxDistance = 2.0f; //distância mínima entre as caixas

    private float timeSinceLastSpawn = 0.0f;
    private GameObject[] currentBoxes;

    private void Start()
    {
        currentBoxes = new GameObject[maxConcurrentBoxes];
    }

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        for (int i = 0; i < maxConcurrentBoxes; i++)
        {
            if (currentBoxes[i] == null)
            {
                SpawnBox(i);
                timeSinceLastSpawn = 0.0f;
            }
        }
    }

    void SpawnBox(int boxIndex)
    {
        float randomX = Random.Range(-3.5f, 3.5f); //metade do tamanho do grid em X
        float randomZ = Random.Range(-3.5f, 3.5f); //metade do tamanho do grid em Z

        Vector3 spawnPosition = new Vector3(Mathf.Floor(randomX) + 0.5f, 2.0f, Mathf.Floor(randomZ) + 0.5f);

        //verifica a distância mínima entre as caixas existentes
        foreach (var box in currentBoxes)
        {
            if (box != null && Vector3.Distance(box.transform.position, spawnPosition) < minBoxDistance)
            {
                //se a distância for menor que a mínima, tenta novamente
                SpawnBox(boxIndex);
                return;
            }
        }

        currentBoxes[boxIndex] = Instantiate(breakableBoxPrefab, spawnPosition, Quaternion.identity);
        Destroy(currentBoxes[boxIndex], boxLifetime);
    }
}
