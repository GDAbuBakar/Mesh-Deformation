using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceListener : MonoBehaviour
{
    public Slicer slicer;

    private Vector3 oldPos;
    private Vector3 curPos;
    private Vector3 orgDir;
    private Vector3 curDir;
    private Transform collTransform;
    private bool isEntered;
    private bool canCut = true;


    private void OnTriggerEnter(Collider other)
    {
        //slicer.isTouched = true;

        collTransform = other.transform;

        oldPos = transform.position;
        isEntered = true;
    }

    private void OnTriggerStay(Collider other)
    {
        curPos = transform.position;

        if(isEntered)
        {
            if (curPos != oldPos)
            {
                orgDir = curPos - oldPos;
                orgDir.Normalize();
                isEntered = false;
            }
        }

        if (curPos.y > oldPos.y || curPos.x < oldPos.x)
            canCut = false;

        curDir = (curPos - oldPos).normalized;

        if (curDir != orgDir)
            canCut = false;

        //print($"{orgDir}, {curDir}");
    }

    private void OnTriggerExit(Collider other)
    {
        if (transform.position.y > oldPos.y || transform.position.x < oldPos.x)
            canCut = false;

        if (canCut)
        {
            slicer.isTouched = true;
            print("done");
        }

        orgDir = new Vector3();
        curDir = new Vector3();

        canCut = true;
    }
}
