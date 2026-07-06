using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class EnemyChasingMoveController : EnemyMoveController
{
    [SerializeField]
    private PostProcessVolume volume;
    private Vignette vignette;

    private float warningIntensity = 0.662f;
    private float nonWarningIntensity = 0f;

    [SerializeField]
    private LayerMask layerMask;

    private bool isClose = false;

    [SerializeField]
    private BoxCollider warningBox;

    private void Start()
    {
        moveSpeed = GameManager.instance.level * 2 + 2;
        hitBoxCollider.isTrigger = true;
        if (volume.profile.TryGetSettings(out vignette))
        {

        }
    }
    private void Update()
    {
        Move();
        IsClose();
    }

    private void IsClose()
    {
        if (isClose || !GameManager.instance.isStart)
            return;

        Collider[] colliders = Physics.OverlapBox(warningBox.bounds.center, warningBox.bounds.size / 2, Quaternion.identity, layerMask);
        foreach (Collider collider in colliders)
        {
            if (collider.name == "Player")
            {
                Debug.Log("플레이어 감지!!");
                vignette.intensity.Override(warningIntensity);
                return;
            }
        }

        vignette.intensity.Override(nonWarningIntensity);


    }
}
