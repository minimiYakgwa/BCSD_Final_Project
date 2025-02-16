using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMoveController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private Transform[] turnPoints;

    [SerializeField]
    protected Animator enemyAnim;

    [SerializeField]
    protected BoxCollider hitBoxCollider;

    private int count = 0;

    protected void Move()
    {
        Vector3 direction = turnPoints[count].position - transform.position;
        direction.y = 0f;
        float distance = direction.sqrMagnitude;
        direction.Normalize();

        if (distance <= 0.1f)
        {
            if (count >= turnPoints.Length-1)
                count = 0;
            else
                count++;
            transform.rotation = Quaternion.Euler(0f, 90f * count, 0f);
        }
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(hitBoxCollider.bounds.center, hitBoxCollider.bounds.size);
    }
}
