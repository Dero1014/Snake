using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public GameObject Body;

    [Range(0,1)]
    public float Speed;

    Vector2 direction;
    private float _time;

    private Vector3 _lastPosition;
    private Vector3 _startPosition;
    private Vector2 _oldDirection;
    private List<Transform> _segments = new List<Transform>();

    public static Snake instance;

    private void Start()
    {
        instance = this;
        ResetState();
    }

    // Change direction but keep moving
    void Update()
    {
        Vector2 newPosition = transform.position;
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (x != 0)
            direction = new Vector2(x, 0);
        else if (y != 0)
            direction = new Vector2(0, y);

        if (direction == -_oldDirection)
            direction = _oldDirection;

        _oldDirection = direction;


        if (Ticker())
        {
            newPosition = (newPosition + direction);
            newPosition.y = Mathf.Round(newPosition.y);
            newPosition.x = Mathf.Round(newPosition.x);
            if (_segments.Count > 0)
            {
                print($"TICKED: POSITION {transform.position} : POSITION {_segments[0].position}");
            }

            transform.position = newPosition;
            if (_segments.Count > 0)
            {
                print($"NEWPOS: POSITION {transform.position} : POSITION {_segments[0].position}");
            }
            for (int i = 0; i < _segments.Count ; i++)
            {
                _startPosition = _segments[i].position;
                _segments[i].position = _lastPosition;
                _lastPosition = _startPosition;
            }
            _lastPosition = transform.position;
            if (_segments.Count > 0)
            {
                print($"AFTPOS: POSITION {transform.position} : POSITION {_segments[0].position}");
            }
        }
    }

    public bool Ticker()
    {
        _time += Time.deltaTime;
        if (_time >= Speed)
        {
            _time = 0;
            return true;
        }
        return false;
    }

    void Grow()
    {
        var clone = Instantiate(Body);
        if (_segments.Count() == 0)
        {
            clone.transform.position = (transform.position - (Vector3)direction);
        }
        else
        {
            clone.transform.position = _segments[_segments.Count - 1].position;
        }
        _segments.Add(clone.transform);
    }

    void ResetState()
    {
        foreach (var segment in _segments)
        {
            Destroy(segment.gameObject);
        }
        _segments.Clear();
        transform.position = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Apple")
        {
            Grow();
        }

        if (other.gameObject.name != "Apple" && other.gameObject.name != "Bounds")
        {
            print("Dead on " + other.gameObject.name);
            ResetState();
        }
    }

}
