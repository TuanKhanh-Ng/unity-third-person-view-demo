using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenDevice : MonoBehaviour
{
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 finalPos;

    private bool _open;

    public void Operate()
    {
        if (!LeanTween.isTweening(gameObject))
        {
            if (_open)
            {
                LeanTween.moveLocal(gameObject, startPos, 1.0f);
            }
            else
            {
                LeanTween.moveLocal(gameObject, finalPos, 1.0f);
            }
            _open = !_open;
        }
    }
}
