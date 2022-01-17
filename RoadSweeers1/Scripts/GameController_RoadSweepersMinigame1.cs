using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController_RoadSweepersMinigame1 : MonoBehaviour
{
    public static GameController_RoadSweepersMinigame1 instance;

    public Camera mainCamera;
    public float startSizeCamera;
    public DirtyObj_RoadSweepersMinigame1 dirtyPrefab;
    public DirtyObj_RoadSweepersMinigame1 dirtyObj;
    public MyCar_RoadSweepersMinigame1 roadSweepersObj;
    public bool isWin, isLose, isBegin, isBoss;
    public Image panelScore;
    public GameObject sample1, sample2;
    public GameObject tutorial, tutorial2;
    public GameObject bgTutorial;
    public Coroutine spawnCoroutine;
    public Text txtScore;
    public int stage = 0;
    public int allDirty;
    public Boss_RoadSweepersMinigame1 bossPrefab;
    public Boss_RoadSweepersMinigame1 bossObj;
    public int countDirty;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(instance);

        isWin = false;
        isLose = false;
        isBegin = false;
        isBoss = false;
    }

    private void Start()
    {
        panelScore.gameObject.SetActive(false);
        startSizeCamera = mainCamera.orthographicSize;
        SetSizeCamera();
        mainCamera.orthographicSize *= 2.0f / 5;
        allDirty = 30;
        countDirty = 0;
        Intro();


    }

    void SetSizeCamera()
    {
        float f1 = 16.0f / 9;
        float f2 = Screen.width * 1.0f / Screen.height;

        mainCamera.orthographicSize *= f1 / f2;
    }

    public void StartSpawning()
    {
        isBegin = true;
        spawnCoroutine = StartCoroutine(SpawnDirty());
    }
    public void StartSpawningBoss()
    {
        spawnCoroutine = StartCoroutine(SpawnDirtyBoss());
    }

    void Intro()
    {
        mainCamera.DOOrthoSize(startSizeCamera, 3).SetEase(Ease.Linear).OnComplete(() =>
        {
            tutorial.SetActive(true);
            roadSweepersObj.isStart = true;
            bgTutorial.gameObject.SetActive(true);
            dirtyObj = Instantiate(dirtyPrefab, new Vector2(roadSweepersObj.transform.position.x, roadSweepersObj.transform.position.y - 0.5f), Quaternion.identity);
            sample1.SetActive(true);
            sample2.SetActive(true);
            tutorial.SetActive(true);
            Tutorial1();
            Time.timeScale = 0;
        });
    }

    public void BringTimeScale()
    {
        Time.timeScale = 1;
    }

    void Tutorial1()
    {
        tutorial.transform.DOMoveX(sample1.transform.position.x, 1).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
        {
            tutorial.transform.DOMoveX(sample2.transform.position.x, 1).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
            {
                if (tutorial.activeSelf)
                {
                    Tutorial1();
                }
            });
        });
    }

    void Tutorial2()
    {
        roadSweepersObj.isStart = true;
        bgTutorial.SetActive(true);
        tutorial2.transform.position = roadSweepersObj.transform.position;
        tutorial2.SetActive(true);
        tutorial2.transform.DOMove(dirtyObj.transform.position, 1).SetEase(Ease.InQuart).SetLoops(-1);
    }

    IEnumerator SpawnDirty()
    {
        int ran;
        while (!isLose)
        {
            if (stage == 1)
            {
                ran = Random.Range(2, 4);
            }
            else if (stage == 2)
            {
                ran = Random.Range(4, 6);
            }
            else ran = 0;

            for (int i = 0; i < ran; i++)
            {
                if (roadSweepersObj.score < 30)
                {
                    Instantiate(dirtyPrefab, new Vector2(Random.Range(-roadSweepersObj.boundSizeCam.x + 0.5f, roadSweepersObj.boundSizeCam.x - 0.5f), Random.Range(-roadSweepersObj.boundSizeCam.y + 0.5f, roadSweepersObj.boundSizeCam.y - 2f)), Quaternion.identity);
                }
            }
            yield return new WaitForSeconds(3);
        }
    }

    IEnumerator SpawnDirtyBoss()
    {
        int ran;
        while (!isLose)
        {
            ran = Random.Range(2, 4);
            for (int i = 0; i < ran; i++)
            {
                if (roadSweepersObj.score < 15)
                {
                    countDirty++;
                    Instantiate(dirtyPrefab, new Vector2(Random.Range(-roadSweepersObj.boundSizeCam.x + 0.5f, roadSweepersObj.boundSizeCam.x - 0.5f), Random.Range(-roadSweepersObj.boundSizeCam.y + 0.5f, roadSweepersObj.boundSizeCam.y - 2f)), Quaternion.identity);
                }
                yield return new WaitForSeconds(3);
            }
        }
    }

  


    void Handle_EventEndGame(int indexEnd)
    {
        if (indexEnd == 1)
        {
            Win();
        }
        else if (indexEnd == -1)
        {
            Lose();
        }
    }

    void Handle_EventScore(int score)
    {
        if (!isBoss)
        {
            if (score > 30)
            {
                score = 30;
            }
            txtScore.text = score.ToString() + "/" + allDirty.ToString();
            if (score == 10)
            {
                stage = 2;
            }
            else if (score == 30)
            {
                isBoss = true;
                stage = 3;
                Debug.Log("Boss");
                StopCoroutine(spawnCoroutine);

                countDirty = 0;
                roadSweepersObj.score = 0;
                allDirty = 15;
                txtScore.transform.DOShakeScale(1);
                BringBoss();
            }
        }
        else
        {
            txtScore.text = score.ToString() + "/" + allDirty.ToString();
            if (score < 14)
            {
                bossObj.transform.localScale = new Vector3(bossObj.transform.localScale.x - 0.025f, bossObj.transform.localScale.y - 0.025f, bossObj.transform.localScale.z - 0.025f);
            }

            if (score == 5)
            {
                bossObj.CallAtk();
            }

            if (score >= 14)
            {
                Debug.Log("Stun");
                bossObj.isStun = true;
                roadSweepersObj.isFinishHim = true;
                bossObj.transform.GetChild(0).gameObject.SetActive(true);
                bossObj.StunFX();
                bossObj.StopAllCoroutines();
                StopAllCoroutines();
            }
        }
    }
    void BringBoss()
    {
        roadSweepersObj.SetUpCarStageBoss();
        bossObj = Instantiate(bossPrefab, new Vector3(roadSweepersObj.boundSizeCam.x + 3, 0, 0), Quaternion.identity);
        bossObj.transform.DOMoveX(roadSweepersObj.boundSizeCam.x - 6, 3).OnComplete(() =>
        {
            roadSweepersObj.isStart = true;
            bossObj.CallSpawnDropToSky();
            spawnCoroutine = StartCoroutine(SpawnDirtyBoss());
        });
    }


    public void Win()
    {
        isWin = true;
        Debug.Log("Win");
        StopAllCoroutines();
        roadSweepersObj.transform.DOMoveX(roadSweepersObj.transform.position.x + 30, 5).OnComplete(() => { Destroy(roadSweepersObj.gameObject); });
    }

    public void Lose()
    {
        isLose = true;
        StopAllCoroutines();
        Debug.Log("Thua");
    }


    private void Update()
    {
        if (tutorial.activeSelf)
        {
            if (roadSweepersObj.transform.position.x > 3 || roadSweepersObj.transform.position.x < -3)
            {
                dirtyObj.Drop();
                bgTutorial.SetActive(false);
                tutorial.transform.DOKill();
                tutorial.SetActive(false);
                sample1.SetActive(false);
                sample2.SetActive(false);
                roadSweepersObj.isHoldCar = false;
                roadSweepersObj.isStart = false;
                Invoke(nameof(Tutorial2), 5);
            }
        }
    }


    private void OnEnable()
    {
        MyCar_RoadSweepersMinigame1.Event_EndGame += Handle_EventEndGame;
        MyCar_RoadSweepersMinigame1.Event_Score += Handle_EventScore;
    }
    private void OnDisable()
    {
        MyCar_RoadSweepersMinigame1.Event_EndGame -= Handle_EventEndGame;
        MyCar_RoadSweepersMinigame1.Event_Score -= Handle_EventScore;
    }
}
