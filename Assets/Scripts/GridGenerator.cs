using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject blockPrefab; //prefab do bloco a ser gerado
    public int gridSizeX = 8; //tamanho do grid em X
    public int gridSizeZ = 8; //tamanho do grid em Z
    public float spacing = 1.0f; //espaçamento entre os blocos

    public Vector3 startingPosition; //posição inicial do grid

    private void Start()
    {
        GenerateGrid(); //chamado ao iniciar o jogo para gerar o grid
    }

    void GenerateGrid()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                //calcula a posição de instância do bloco no grid
                Vector3 spawnPosition = startingPosition + new Vector3(x * spacing, 0, z * spacing);

                //instancia o Prefab do bloco na posição calculada
                Instantiate(blockPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}
