using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float _walkSpeed = 10.0f;
    float _runSpeed = 20.0f;
    float _currentSpeed;

    void Start()
    {
        _currentSpeed = _walkSpeed;

        Managers.Input.KeyAction -= OnKeyBoard;
        Managers.Input.KeyAction += OnKeyBoard;
    }

    void Update()
    {
        if (Input.anyKey == false && IsRun)
        {
            _currentSpeed = _walkSpeed;
            IsRun = false;
        }
        else if (IsRun)
            _currentSpeed = _runSpeed;
    }

    enum Dir
    {
        Up, Down, Left, Right
    }
    private float[] _lastKeyDown = { 0, 0, 0, 0 };
    public bool IsRun { get; private set; } = false;
    private float _threshold = 0.3f;
    void OnKeyBoard()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            IsRun = Time.time - _lastKeyDown[(int)Dir.Up] <= _threshold;
            _lastKeyDown[(int)Dir.Up] = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            IsRun = Time.time - _lastKeyDown[(int)Dir.Down] <= _threshold;
            _lastKeyDown[(int)Dir.Down] = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            IsRun = Time.time - _lastKeyDown[(int)Dir.Left] <= _threshold;
            _lastKeyDown[(int)Dir.Left] = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            IsRun = Time.time - _lastKeyDown[(int)Dir.Right] <= _threshold;
            _lastKeyDown[(int)Dir.Right] = Time.time;
        }
        Vector3 dir = Vector3.zero;
        if (Input.GetKey(KeyCode.UpArrow))
            dir += Vector3.forward;
        if (Input.GetKey(KeyCode.DownArrow))
            dir += Vector3.back;
        if (Input.GetKey(KeyCode.RightArrow))
            dir += Vector3.right;
        if (Input.GetKey(KeyCode.LeftArrow))
            dir += Vector3.left;
        if (dir != Vector3.zero)
            Move(dir.normalized);
    }

    private float _turnDeltaSpeed = 0.1f;
    void Move(Vector3 dir)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), _turnDeltaSpeed);
        transform.position += dir * _currentSpeed * Time.deltaTime;
    }
}
