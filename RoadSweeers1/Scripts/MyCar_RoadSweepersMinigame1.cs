using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyCar_RoadSweepersMinigame1 : AbstractMyCar_Minigame
{
    public int indexEnd;
    public bool isStart;
    public Vector3 startScale;
    public int score = 0;
    public GameObject crashFX;
    public bool isFinishHim;

    public static event Action<int> Event_EndGame;
    public static event Action<int> Event_Score;

    public SkeletonAnimation anim;
    [SpineAnimation] public string anim_DonDep, anim_PhunNuoc, anim_Idle, anim_Thua;



    private void Awake()
    {
        SetUpBoundSize();
        indexEnd = 0; // -1 event lose, 1 event win
    }


    private void Start()
    {
        isStart = false;
        isFinishHim = false;
        lastPos = transform.position;
        startScale = transform.localScale;
        anim.state.Complete += AnimComplete;
        PlayAnim(anim, anim_Idle, true);

    }

    private void AnimComplete(Spine.TrackEntry trackEntry)
    {
        if (trackEntry.Animation.Name == anim_DonDep)
        {
            PlayAnim(anim, anim_Idle, true);
        }
    }

    public void PlayAnim(SkeletonAnimation anim, string nameAnim, bool loop)
    {
        anim.state.SetAnimation(0, nameAnim, loop);
    }

    public void SetUpCarStageBoss()
    {
        isStart = false;
        transform.DOMove(new Vector2(-boundSizeCam.x + 2, 0), 2).SetEase(Ease.Linear);
        score = 0;
    }

    private void OnMouseDown()
    {
        if (isStart)
        {
            CheckPosMove();
            if (GameController_RoadSweepersMinigame1.instance.tutorial.activeSelf)
            {
                GameController_RoadSweepersMinigame1.instance.BringTimeScale();
            }
        }
    }

    void CheckTutorial2()
    {
        if (GameController_RoadSweepersMinigame1.instance.tutorial2.activeSelf)
        {
            GameController_RoadSweepersMinigame1.instance.tutorial2.transform.DOKill();
            GameController_RoadSweepersMinigame1.instance.tutorial2.SetActive(false);
            GameController_RoadSweepersMinigame1.instance.bgTutorial.SetActive(false);
            GameController_RoadSweepersMinigame1.instance.StartSpawning();
            GameController_RoadSweepersMinigame1.instance.stage = 1;
            GameController_RoadSweepersMinigame1.instance.panelScore.gameObject.SetActive(true);
        }
    }

    private void OnMouseUp()
    {
        isHoldCar = false;
    }

    private void LateUpdate()
    {
        FlipCar();
    }

    private void Update()
    {
        if (isHoldCar && indexEnd != -1 && indexEnd != 1)
        {
            MoveCar();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            indexEnd = -1;
            Event_EndGame?.Invoke(indexEnd);
            isHoldCar = false;
            Destroy(collision.gameObject);
            PlayAnim(anim, anim_Thua, false);
        }

        if (collision.gameObject.CompareTag("Tree") && indexEnd != -1)
        {
            Destroy(collision.gameObject);
            if (isStart && !isFinishHim)
            {
                score++;                
            }
            Event_Score?.Invoke(score);
            transform.DOKill();
            transform.localScale = startScale;
            transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f).SetEase(Ease.Linear);
            CheckTutorial2();
        }

        if (collision.gameObject.CompareTag("Trash"))
        {
            if (!collision.GetComponent<Boss_RoadSweepersMinigame1>().isStun)
            {

                PlayAnim(anim, anim_Thua, false);
                indexEnd = -1;
                Event_EndGame?.Invoke(indexEnd);
                isHoldCar = false;
            }
            else
            {
                collision.GetComponent<BoxCollider2D>().enabled = false;
                score++;
                Event_Score?.Invoke(score);
                var tmpCrash = Instantiate(crashFX, collision.transform.position, Quaternion.identity);
                Destroy(tmpCrash, 1);
                collision.GetComponent<SpriteRenderer>().DOFade(0, 1).OnComplete(() =>
                {
                    Destroy(collision.gameObject);
                    indexEnd = 1;
                    Event_EndGame?.Invoke(indexEnd);
                    isHoldCar = false;
                });
            }
        }
    }
}
