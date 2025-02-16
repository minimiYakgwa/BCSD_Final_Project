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
        if (isAttack)
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

        /*Debug.Log("공격 딜레이 카운트 :");
        Debug.Log("5");
        yield return new WaitForSeconds(1f);
        Debug.Log("4");
        yield return new WaitForSeconds(1f);
        Debug.Log("3");
        yield return new WaitForSeconds(1f);
        Debug.Log("2");
        yield return new WaitForSeconds(1f);
        Debug.Log("1");
        yield return new WaitForSeconds(1f);
        Debug.Log("0");*/
        yield return new WaitForSeconds(5f);
        isAttack = false;
    }
}
