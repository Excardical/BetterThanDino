using UnityEngine;
using System.Collections;

public class GoblinReaper : EnemyBase
{
    [SerializeField] private int idleDamage = 1; // Amount of damage to take when idle
    [SerializeField] private float slowedSpeed = 1f; // How slow it becomes
    [SerializeField] private float slowDuration = 2f; // How long the slow lasts

    private float originalSpeed;
    private bool isSlowed = false;
    
    void Start()
    {
        base.Start();
        originalSpeed = speed;
    }

    void Update()
    {
        base.Update();
    }

    protected override void OnIdleStateEnter()
    {
        base.OnIdleStateEnter();
        TakeDamage(idleDamage); 
        if (!isSlowed)
        {
            StartCoroutine(SlowDownRoutine());
        }
    }
    
    private IEnumerator SlowDownRoutine()
    {
        isSlowed = true;
        speed = slowedSpeed; // Slow down

        yield return new WaitForSeconds(slowDuration);

        speed = originalSpeed; // Return to normal speed
        isSlowed = false;
    }
}