using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public EnemyMovement[] enemies;

    private bool playerTurn = true;

    public void EndPlayerTurn()
    {
        if (!playerTurn)
            return;

        playerTurn = false;
        EnemyTurn();
    }

    void EnemyTurn()
    {
        foreach (EnemyMovement enemy in enemies)
        {
            enemy.MoveTowardPlayer();
        }

        playerTurn = true;
    }

    public bool IsPlayerTurn()
    {
        return playerTurn;
    }
}