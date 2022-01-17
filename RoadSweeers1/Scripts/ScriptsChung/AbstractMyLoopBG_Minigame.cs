using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MyDirection { up, down, right, left}

public abstract class AbstractMyLoopBG_Minigame : MonoBehaviour
{
    public Vector3 startPos;
    public float speedBG;
    public Vector3 direction;

    //private void Awake()
    //{
    //    startPos = transform.position;
    //}

    //Update

    public virtual void LoopBG(MyDirection enumDirection, float posReset)
    {
        if(enumDirection == MyDirection.up)
        {
            direction = Vector3.up;            
        }
        else if(enumDirection == MyDirection.right)
        {
            direction = Vector3.right;
        }
        else if (enumDirection == MyDirection.down)
        {
            direction = Vector3.down;
        }
        else if (enumDirection == MyDirection.left)
        {
            direction = Vector3.left;
        }

        transform.Translate(direction * speedBG * Time.deltaTime);
        if(enumDirection == MyDirection.up)
        {
            if (transform.position.y > posReset)
            {
                transform.position = startPos;
            }
        }
        else if(enumDirection == MyDirection.down)
        {
            if (transform.position.y < posReset)
            {
                transform.position = startPos;
            }
        }
        else if (enumDirection == MyDirection.left)
        {
            if (transform.position.x < posReset)
            {
                transform.position = startPos;
            }
        }
        else if (enumDirection == MyDirection.right)
        {
            if (transform.position.x > posReset)
            {
                transform.position = startPos;
            }
        }


    }
}
