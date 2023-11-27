using System;
using UnityEngine;

public class PowerUpBox : MonoBehaviour
{
    // Enumeração dos tipos de power-up
    public enum PowerUpType
    {
        None,
        SpeedBoost,
        SpeedReduction,
        // Adicione mais tipos de power-up conforme necessário
    }

    public PowerUpType powerUpType; // Tipo de power-up associado a esta caixa

    private void Start()
    {
        // Inicializa o tipo de power-up aleatoriamente ao criar a caixa
        powerUpType = GetRandomPowerUpType();
    }

    private PowerUpType GetRandomPowerUpType()
    {
        // Gera um valor entre 0 e 1
        float randomValue = UnityEngine.Random.value;

        // Define a chance para cada tipo de power-up
        float speedBoostChance = 0.75f; //75%
        float speedReductionChance = 0.25f; //25%
        // Adicione mais tipos e suas chances conforme necessário

        // Compara o valor aleatório com as chances para determinar o tipo de power-up
        if (randomValue < speedBoostChance)
        {
            return PowerUpType.SpeedBoost;
        }
        else if (randomValue < speedBoostChance + speedReductionChance)
        {
            return PowerUpType.SpeedReduction;
        }
        else
        {
            return PowerUpType.None; // Pode adicionar outros tipos aqui
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Verifica se colidiu com o jogador
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>(); // Obtém o componente PlayerController do jogador

            // Verifica se já existe um poder ativo no jogador
            if (player.IsUnderPowerUp())
            {
                // Reseta o efeito atual antes de aplicar um novo
                player.ResetPowerUpEffect();
            }

            // Aplica o efeito do power-up ao jogador
            ApplyPowerUpEffect(player);

            Destroy(gameObject); // Destroi a caixa de power-up após a colisão
        }
    }

    void ApplyPowerUpEffect(PlayerController player)
    {
        // Aplica o efeito do power-up com base no tipo
        switch (powerUpType)
        {
            case PowerUpType.SpeedBoost:
                player.ApplySpeedBoost(); // Adicione o método ApplySpeedBoost ao PlayerController
                break;

            case PowerUpType.SpeedReduction:
                player.ApplySpeedReduction(); // Adicione o método ApplySpeedReduction ao PlayerController
                break;

                // Adicione mais casos conforme necessário para outros tipos de power-up
        }
    }
}
