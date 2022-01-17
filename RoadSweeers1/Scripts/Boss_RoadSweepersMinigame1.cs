using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_RoadSweepersMinigame1 : MonoBehaviour
{
    public Coroutine atkCoroutine;
    public bool isStun;
    public GameObject dropFromBossPrefab;
    public SkeletonAnimation anim;
    [SpineAnimation] public string anim_Choang, anim_Idle, anim_TanCong;


    private void Start()
    {
        isStun = false;
        GameController_RoadSweepersMinigame1.instance.StartSpawningBoss();
        //anim.state.Complete += AnimComplete;
        //PlayAnim(anim, anim_Idle, true);
    }

    private void AnimComplete(Spine.TrackEntry trackEntry)
    {
        if (trackEntry.Animation.Name == anim_TanCong)
        {
            PlayAnim(anim, anim_Idle, true);
        }
    }

    public void PlayAnim(SkeletonAnimation anim, string nameAnim, bool loop)
    {
        anim.state.SetAnimation(0, nameAnim, loop);
    }

    public IEnumerator Attack()
    {
        while (!GameController_RoadSweepersMinigame1.instance.isLose && !GameController_RoadSweepersMinigame1.instance.isWin)
        {
            transform.DOPunchPosition(new Vector3(0.1f, 0.1f, 0.1f), 1).OnComplete(() =>
            {
                transform.DOMove(GameController_RoadSweepersMinigame1.instance.roadSweepersObj.transform.position, 3).SetEase(Ease.Linear);
            });
            yield return new WaitForSeconds(5);
        }
    }

    public IEnumerator SpawnDropToSky()
    {
        while (!GameController_RoadSweepersMinigame1.instance.isLose && !GameController_RoadSweepersMinigame1.instance.isWin)
        {
            var tmpDropToSky = Instantiate(dropFromBossPrefab, transform.position, Quaternion.identity);
            tmpDropToSky.transform.DOMoveY(20, 1).OnComplete(() =>
            {
                Destroy(tmpDropToSky);
            });
            yield return new WaitForSeconds(2.5f);
        }
    }

    public void CallSpawnDropToSky()
    {
        StartCoroutine(SpawnDropToSky());
    }

    public void CallAtk()
    {
        StartCoroutine(Attack());
    }

    public void StunFX()
    {
        GetComponent<SpriteRenderer>().DOFade(0, 1).OnComplete(() =>
         {
             GetComponent<SpriteRenderer>().DOFade(1, 1).OnComplete(() =>
             {
                 if (gameObject != null)
                 {
                     StunFX();
                 }
             });
         });
    }


    private void FixedUpdate()
    {
        if (transform.position.x < GameController_RoadSweepersMinigame1.instance.roadSweepersObj.transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
}
