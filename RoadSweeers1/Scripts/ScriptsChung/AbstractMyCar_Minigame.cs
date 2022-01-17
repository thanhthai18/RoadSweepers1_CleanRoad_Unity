using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMyCar_Minigame : MonoBehaviour
{
    public MaterialPropertyBlock materialBlock;
    [SerializeField] protected MeshRenderer meshRenderer;
    public Vector2 lastPos;
    public bool isHoldCar = false;
    public Vector2 deltaDistance;
    public Vector2 mousePos;
    public Camera mainCamera;
    public Vector2 boundSizeCam;
    public Vector2 boundSizeMyCar;
    public Coroutine fadeCoroutine;
    public int currentLane;

    //Fade MeshRender
    public virtual void SetColorForMaterial(float alpha)
    {
        materialBlock.SetColor("_Color", new Color(1, 1, 1, alpha));
        meshRenderer.SetPropertyBlock(materialBlock);
    }

    public virtual IEnumerator FadeMeshRender()
    {
        float timeFade = 2;
        while (timeFade > 0)
        {
            float lerp = Mathf.PingPong(Time.time, 0.5f) / 0.5f;
            SetColorForMaterial(lerp);
            yield return null;
        }
        StopCoroutine(fadeCoroutine);
    }
    //Fade MeshRender


    //Awake
    public virtual void SetUpBoundSize()
    {
        if (GetComponent<Collider2D>() != null)
        {
            boundSizeCam = new Vector2(mainCamera.orthographicSize * Screen.width * 1.0f / Screen.height, mainCamera.orthographicSize);
            boundSizeMyCar = new Vector2(GetComponent<Collider2D>().bounds.extents.x, GetComponent<Collider2D>().bounds.extents.y);
        }
    }

    //OnMouseDown
    public virtual void CheckPosMove()
    {
        isHoldCar = true;
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        deltaDistance = mousePos - (Vector2)transform.position;
    }

    //LateUpdate
    public virtual void FlipCar()
    {
        if (isHoldCar)
        {
            if (transform.position.x > lastPos.x)
            {
                transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            if (transform.position.x < lastPos.x)
            {
                transform.localEulerAngles = new Vector3(0, 180, 0);
            }
        }
        lastPos = transform.position;
    }

    //Update
    public virtual void MoveCar()
    {
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector2(Mathf.Clamp(mousePos.x, -boundSizeCam.x + boundSizeMyCar.x + deltaDistance.x, boundSizeCam.x - boundSizeMyCar.x + deltaDistance.x), Mathf.Clamp(mousePos.y, -boundSizeCam.y + boundSizeMyCar.y + deltaDistance.y, boundSizeCam.y - boundSizeMyCar.y + deltaDistance.y));
        transform.position = new Vector2(mousePos.x - deltaDistance.x, mousePos.y - deltaDistance.y);
    }  
    //

    public virtual void MoveCar_UpDown(Vector2 startMousePos, Vector2 endMousePos, int maxLane, float distance)
    {
        if (startMousePos.x == endMousePos.x)
        {
            return;
        }
        else if (startMousePos.y + 0.05f < endMousePos.y && Mathf.Abs(startMousePos.x - endMousePos.x) < 8)
        {
            if (currentLane > 1)
            {
                UpDownAction(1, distance);                
            }
        }
        else if (startMousePos.y > endMousePos.y + 0.05f && Mathf.Abs(startMousePos.x - endMousePos.x) < 8)
        {
            if (currentLane < maxLane)
            {
                UpDownAction(0, distance);               
            }
        }
    }
    public virtual void UpDownAction(int updownBinary, float distance)
    {
        if (updownBinary == 1)
        {
            Debug.Log("Len");
            GetComponent<Collider2D>().enabled = false;
            currentLane--;
            transform.DOMoveY(transform.position.y + distance, 0.1f).OnComplete(() => 
            {
                GetComponent<Collider2D>().enabled = true;
            });
        }
        else if (updownBinary == 0)
        {
            Debug.Log("Xuong");
            GetComponent<Collider2D>().enabled = false;
            currentLane++;
            transform.DOMoveY(transform.position.y - distance, 0.1f).OnComplete(() =>
            {
                GetComponent<Collider2D>().enabled = true;
            }); ;
        }
    }
}
