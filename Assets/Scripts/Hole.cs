using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
   
    public Transform GeneratePos;
    public bool IsOccupied;
    public int rolNum = 0;
    public SpriteRenderer behind;
     public SpriteRenderer front;
     private GameManager gameManager;
    public void Init(GameManager gm)
    {
        behind.sortingOrder = rolNum * 3 - 1;
        front.sortingOrder = rolNum * 3 + 1;
        gameManager=gm;
    }
    public bool GenerateAnon(GameObject AnonObject)
    {
        if (IsOccupied)
            return false;

        GameObject go = Instantiate(AnonObject, GeneratePos);
        go.transform.localPosition = Vector3.zero; 
        IsOccupied = true;


        Mole m = go.GetComponent<Mole>();
        m.Init(rolNum * 3,gameManager);
        m.OnDisappear += () =>
        {
            IsOccupied = false;
        };
        return true;
    }
}
