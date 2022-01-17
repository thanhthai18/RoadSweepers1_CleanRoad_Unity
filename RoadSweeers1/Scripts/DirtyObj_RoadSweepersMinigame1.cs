using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyObj_RoadSweepersMinigame1 : MonoBehaviour
{
    public GameObject dropObj;
    public GameObject dropPrefab;
    public float speed;

    private void Start()
    {
        speed = 1;
        if (GameController_RoadSweepersMinigame1.instance.isBegin)
        {
            if (GameController_RoadSweepersMinigame1.instance.stage != 3)
            {
                Destroy(gameObject, 10);
            }
        }

        if (!GameController_RoadSweepersMinigame1.instance.tutorial.activeSelf)
        {
            Drop();
        }
    }

    public void Drop()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().DOColor(Color.black, 2.5f).SetEase(Ease.InQuart).OnComplete(() =>
        {
            dropObj = Instantiate(dropPrefab, new Vector2(transform.position.x, GameController_RoadSweepersMinigame1.instance.mainCamera.orthographicSize + 1), Quaternion.identity);
            dropObj.transform.parent = transform;
            dropObj.transform.DOScale(new Vector3(dropObj.transform.localScale.x, dropObj.transform.localScale.y, dropObj.transform.localScale.z), 1.2f).SetEase(Ease.Linear).OnComplete(() =>
            {
                dropObj.GetComponent<CircleCollider2D>().enabled = true;
            });
            dropObj.transform.DOMoveY(transform.position.y + 1, 1.5f).SetEase(Ease.InQuart).OnComplete(() =>
            {
                Destroy(dropObj);
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
            });
        });
    }

    private void Update()
    {
        if (GameController_RoadSweepersMinigame1.instance.isBegin && !GameController_RoadSweepersMinigame1.instance.isWin && !GameController_RoadSweepersMinigame1.instance.isLose && GameController_RoadSweepersMinigame1.instance.stage != 3)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);

        }
    }

}
