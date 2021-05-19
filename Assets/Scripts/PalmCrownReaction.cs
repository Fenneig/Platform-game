using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalmCrownReaction : MonoBehaviour
{
    [SerializeField] private Collider2D _crownCollider;
    [SerializeField] private LayerMask _charactersMask;
    private BoxCollider2D _boxCollider;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_boxCollider.IsTouchingLayers(_charactersMask))
            _crownCollider.enabled = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _crownCollider.enabled = true;
    }
}
