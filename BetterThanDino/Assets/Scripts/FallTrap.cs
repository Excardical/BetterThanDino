using UnityEngine;

public class FallTrap : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] stones;
    private float cooldownTimer;

    private void Attack()
    {
        cooldownTimer=0;
        stones[FindStone()].transform.position = firePoint.position;
        stones[FindStone()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }
    private int FindStone()
    {
        for(int i=0;i<stones.Length;i++)
        {
            if(!stones[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }
    // Update is called once per frame
    private void Update()
    {
        cooldownTimer+=Time.deltaTime;
        if(cooldownTimer>=attackCooldown)
        {
            Attack();
        }
    }
}
