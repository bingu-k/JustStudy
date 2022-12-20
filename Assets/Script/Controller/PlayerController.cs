using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum PlayerState
    { Idle, Walk, Run, Jump }
    PlayerState _state = PlayerState.Idle;
    PlayerState _prevState = PlayerState.Idle;

    [SerializeField]
    float _walkSpeed = 10.0f;
    [SerializeField]
    float _runSpeed = 20.0f;

    Animator _anim = null;

    void Start()
    {
        Managers.Input.KeyAction -= OnKeyBoard;
        Managers.Input.KeyAction += OnKeyBoard;
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.anyKey == false)
            _state = PlayerState.Idle;
        if (_state != _prevState)
        {
            switch (_state)
            {
                case PlayerState.Idle:
                    _anim.CrossFade("Idle", 0.3f);
                    break;
                case PlayerState.Walk:
                    _anim.CrossFade("Walk", 0.3f);
                    break;
                case PlayerState.Run:
                    _anim.CrossFade("Run", 0.3f);
                    break;
                case PlayerState.Jump:
                    _anim.CrossFade("Jump", 0.3f);
                    break;
            }
            _prevState = _state;
        }
    }

    void OnKeyBoard()
    {
        Vector3 dir = Vector3.zero;
        CalDirection(ref dir);

        CheckKeyDoubleDown();
        if (dir != Vector3.zero)
            Move(dir.normalized);
        else
            _state = PlayerState.Idle;
    }

    void CalDirection(ref Vector3 dir)
    {
        if (Input.GetKey(KeyCode.UpArrow))
            dir += Vector3.forward;
        if (Input.GetKey(KeyCode.DownArrow))
            dir += Vector3.back;
        if (Input.GetKey(KeyCode.RightArrow))
            dir += Vector3.right;
        if (Input.GetKey(KeyCode.LeftArrow))
            dir += Vector3.left;
    }
    enum Dir
    { Up, Down, Left, Right }
    private float[] _lastKeyDown = { 0, 0, 0, 0 };
    private float _threshold = 0.3f;
    void CheckKeyDoubleDown()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (Time.time - _lastKeyDown[(int)Dir.Up] <= _threshold)
                _state = PlayerState.Run;
            _lastKeyDown[(int)Dir.Up] = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Time.time - _lastKeyDown[(int)Dir.Down] <= _threshold)
                _state = PlayerState.Run;
            _lastKeyDown[(int)Dir.Down] = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (Time.time - _lastKeyDown[(int)Dir.Left] <= _threshold)
                _state = PlayerState.Run;
            _lastKeyDown[(int)Dir.Left] = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Time.time - _lastKeyDown[(int)Dir.Right] <= _threshold)
                _state = PlayerState.Run;
            _lastKeyDown[(int)Dir.Right] = Time.time;
        }
    }

    private float _turnDeltaSpeed = 0.1f;
    void Move(Vector3 dir)
    {
        if (_state != PlayerState.Run)
            _state = PlayerState.Walk;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), _turnDeltaSpeed);
        if (_state == PlayerState.Walk)
            transform.position += dir * _walkSpeed * Time.deltaTime;
        else
            transform.position += dir * _runSpeed * Time.deltaTime;
    }
}
