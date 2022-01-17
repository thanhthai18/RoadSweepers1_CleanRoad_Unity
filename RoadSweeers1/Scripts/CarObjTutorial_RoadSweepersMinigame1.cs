using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarObjTutorial_RoadSweepersMinigame1 : AbstractMyCar_Minigame
{
    public bool isOnFade;

    private void Start()
    {
        isOnFade = true;
        materialBlock = new MaterialPropertyBlock();
        if (isOnFade)
        {
            SetColorForMaterial(0.5f);
        }
    }

    public override void SetColorForMaterial(float alpha)
    {
        base.SetColorForMaterial(alpha);
    }
}
