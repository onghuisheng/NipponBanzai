using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EntityEnemy : EntityLivingBase
{

    protected virtual void DoSpawnAnimation(ParticleHandler.ParticleType _particleType)
    {
        B_isAIEnabled = false;

        GameObject model = transform.GetChild(0).gameObject;

        if (model != null && model.name == "Model")
        {
            const float modelYOffset = -1.5f;
            const float modelTweenDuration = 3;
            const float portalTweenDuration = 0.5f; // Make sure this is always lesser than modelTweenDuration

            GameObject portal = ParticleHandler.GetInstance().SpawnParticle(_particleType, transform, new Vector3(0, 0.01f, 0), Vector3.zero, new Vector3(90, 0, 0), 0);
            portal.transform.DOScale(2, portalTweenDuration);

            model.transform.localPosition = new Vector3(0, modelYOffset, 0);
            model.transform.DOLocalMoveY(0, modelTweenDuration).SetEase(Ease.Linear).OnComplete(() =>
            {
                B_isAIEnabled = true;

                portal.transform.parent = null;
                portal.transform.DOScale(0, portalTweenDuration * 3).OnComplete(() =>
                {
                    Destroy(portal);
                });
            });
        }

    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
