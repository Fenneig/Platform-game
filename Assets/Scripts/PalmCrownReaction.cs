using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalmCrownReaction : MonoBehaviour
{
    private EdgeCollider2D _edgeCollider;

    private void Awake()
    {
        _edgeCollider = GetComponent<EdgeCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Enter trigger");
        _edgeCollider.isTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("Exit trigger");
        _edgeCollider.isTrigger = false;
    }
}
