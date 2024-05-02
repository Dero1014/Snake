using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    public Transform following;

    private Vector3 _oldPosition;

    private float _speed;
    private float _time;

    // Update is called once per frame
    void Update()
    {
        if (following != null)
        {
            if (Ticker())
            {
                transform.position = _oldPosition;
            }
            _oldPosition = following.position;
        }
    }

    public void GenerateBody(Transform target, float speed, float time)
    {
        following = target;
        _oldPosition = following.position;
        _speed = speed;
        _time = time;
    }

    bool Ticker()
    {
        _time += Time.deltaTime;
        if (_time >= _speed)
        {
            _time = 0;
            return true;
        }
        return false;
    }
}
