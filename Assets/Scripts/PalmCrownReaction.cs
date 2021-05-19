using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalmCrownReaction : MonoBehaviour
{
    private EdgeCollider2D _edgeCollider;
    private BoxCollider2D _boxCollider;
    [SerializeField] private LayerMask _layerMask;

    private void Awake()
    {
        _edgeCollider = GetComponent<EdgeCollider2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_boxCollider.IsTouchingLayers(_layerMask)) _edgeCollider.enabled = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _edgeCollider.enabled = true;
    }
}
