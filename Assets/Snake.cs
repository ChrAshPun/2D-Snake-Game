using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;
    private List<Transform> _segments = new List<Transform>();
    public Transform segmentPrefab;

    // Start is called before the first frame update
    void Start()
    {
        ResetState();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            _direction = Vector2.up;
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            _direction = Vector2.left;
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            _direction = Vector2.down;
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            _direction = Vector2.right;
        }
    }

    // run physics code here; frame-independent
    private void FixedUpdate()
    {
        // iter from the tail to make each segment move forward
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }

        // position the head last
        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + _direction.x,
            Mathf.Round(this.transform.position.y) + _direction.y,
            0.0f
        );
    }

    private void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;
        _segments.Add(segment);
    }

    private void ResetState()
    {
        for (int i = 1; i < _segments.Count; i++) {
            Destroy(_segments[i].gameObject); // destroy the gameObject in Unity
        }

        _segments.Clear(); // clear refs to the gameObjects that have been destroyed
        _segments.Add(this.transform); // add back the snake head

        this.transform.position = Vector3.zero; // reset position
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food") {
            Grow();
        } else if (other.tag == "Obstacle") {
            ResetState();
        }
    }
}
