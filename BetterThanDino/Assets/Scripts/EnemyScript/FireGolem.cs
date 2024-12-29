using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGolem : EnemyBase
{
    private Animator anim;
    private bool isDying = false; // 防止重复执行死亡逻辑
    private bool localIsPostAttackPausing = false; // 本地攻击暂停标志\
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if(transform.position.x - target.position.x < 0) transform.eulerAngles = new Vector3(0, 180, 0);
        if(transform.position.x - target.position.x > 0) transform.eulerAngles = new Vector3(0, 0, 0);
    }

    public override void TakeDamage(int damage)
    {
        if (localIsPostAttackPausing) return; // 检查本地暂停标志
        base.TakeDamage(damage); // 调用父类的受伤逻辑

        if (!isDying && anim != null)
        {
            anim.SetTrigger("Hit"); // 触发受击动画
        }
    }
    
    public void StartLocalPostAttackPause(float duration)
    {
        StartCoroutine(LocalPostAttackPause(duration));
    }

    private IEnumerator LocalPostAttackPause(float duration)
    {
        localIsPostAttackPausing = true;
        yield return new WaitForSeconds(duration);
        localIsPostAttackPausing = false;
    }
    
    protected override void Die()
    {
        if (isDying) return; // 避免重复触发死亡逻辑
        isDying = true;

        // 使用 ChangeState 方法切换到死亡状态
        ChangeState(EnemyState.Death);

        if (anim != null)
        {
            anim.SetTrigger("Die"); // 触发死亡动画
        }

        // 使用动画触发器，通过 Animation Event 或自动过渡实现
        StartCoroutine(DestroyAfterDeathAnimation());
    }
    
    private IEnumerator DestroyAfterDeathAnimation()
    {
        // 等待一段合理时间，假设死亡动画长度为 2 秒
        yield return new WaitForSeconds(1.5f);

        // 销毁对象
        Destroy(gameObject);
    }
}
