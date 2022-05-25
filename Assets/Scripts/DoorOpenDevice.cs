using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenDevice : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    public float _speed = 5.0f;

    private Vector3 startPos;
    private Vector3 finalPos;

    private bool _open;

    void Start()
    {
        startPos = transform.position;
        finalPos = startPos + _offset;
    }

    public void Activate()
    {
        if (!_open)
        {
            float time = (finalPos - transform.position).magnitude / _speed;
            LeanTween.moveLocal(gameObject, finalPos, time);
            _open = true;
        }
    }

    public void Deactivate()
    {
        if (_open)
        {
            float time = (startPos - transform.position).magnitude / _speed;
            LeanTween.moveLocal(gameObject, startPos, time);
            _open = false;
        }    
    }

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
