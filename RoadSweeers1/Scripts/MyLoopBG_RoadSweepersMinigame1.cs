using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyLoopBG_RoadSweepersMinigame1 : AbstractMyLoopBG_Minigame
{
    private void Awake()
    {
        speedBG = 1;
        startPos = transform.position;
    }

    private void Update()
    {
        if(!GameController_RoadSweepersMinigame1.instance.isLose && !GameController_RoadSweepersMinigame1.instance.isWin && GameController_RoadSweepersMinigame1.instance.isBegin && GameController_RoadSweepersMinigame1.instance.stage != 3)
        {
            LoopBG(MyDirection.left, -18.87f);
        }
    }
}
