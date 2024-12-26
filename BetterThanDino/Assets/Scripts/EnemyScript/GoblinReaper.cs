using UnityEngine;

public class GoblinReaper : EnemyBase
{
    [SerializeField] private int idleDamage = 1; // Amount of damage to take when idle

    void Start()
    {
        base.Start();
    }

    void Update()
    {
        base.Update();
    }

    protected override void OnIdleStateEnter()
    {
        base.OnIdleStateEnter();
        TakeDamage(idleDamage); // Use the existing TakeDamage method from EnemyBase
        Debug.Log("Goblin Reaper take damage from idle");
    }
}