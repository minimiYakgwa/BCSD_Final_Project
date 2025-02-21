using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRunAwayMoveController : EnemyMoveController
{
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private PlayerController player;

    private bool isAttack = false;

    private void Update()
    {
        Move();
        IsAttack();
    }

    private void IsAttack()
    {
        if (isAttack || !GameManager.instance.isStart)
            return;

        Collider[] colliders = Physics.OverlapBox(hitBoxCollider.bounds.center, hitBoxCollider.bounds.size / 2, Quaternion.identity, layerMask);
        foreach (Collider collider in colliders)
        {
            if (collider.name == "Player")
            {
                Debug.Log("플레이어 감지!!");
                StartCoroutine(AttackAndDelay());
            }
        }

    }

    private IEnumerator AttackAndDelay()
    {
        enemyAnim.SetTrigger("Attack");
        isAttack = true;

        StartCoroutine(player.ParryTiming());

        yield return new WaitForSeconds(5f);
        isAttack = false;
    }
}
