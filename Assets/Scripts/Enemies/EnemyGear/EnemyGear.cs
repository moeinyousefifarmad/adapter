using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyGear : MonoBehaviour
{
    [SerializeField] private float moveDuration;
    [SerializeField] private Transform Point1;
    [SerializeField] private Transform Point2;
    private bool canMoveTo1;
    private bool canMoveTo2;
    

    private void Awake()
    {
        canMoveTo1 = true;
    }

    private void Update()
    {

        if(transform.position == Point1.position)
        {
            canMoveTo2 = true;
            canMoveTo1 = false;
        } 
        if(transform.position == Point2.position)
        {
            canMoveTo2 = false;
            canMoveTo1 = true;
        } 
        if(canMoveTo1) MoveToPoint1();
        if(canMoveTo2) MoveToPoint2();
    }
    private void MoveToPoint1()
    {
        transform.position = Vector2.MoveTowards(transform.position , Point1.position , moveDuration);
    }
    private void MoveToPoint2()
    {
        transform.position = Vector2.MoveTowards(transform.position , Point2.position , moveDuration);
    }
}
