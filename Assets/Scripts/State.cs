using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
   
    public int Life { get; protected set; } = 1; //��������
    public float Interval { get; protected set; } = 1;//���ʱ��
    public string nameOfState;

    public GameObject[] moles;
    protected GameManager gameManager;
    public State Next()
    {
        if (Life == 0)
        {
            return GetNextState();
        }
        Life--;
        Generate();
        return null;
    }

    public abstract void Generate();

    public virtual void Init(GameManager gm)
    {
        gameManager = gm;
    }
    public virtual void Hit(int id)
    {
        gameManager.Hit();
    }
    public virtual void Miss(int id)
    {
        gameManager.Miss();
    }
    //��һ��״̬
    public virtual State GetNextState()
    {
        return this;
    }



}
