using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Define.PlayerState _state = Define.PlayerState.Idle;
    bool _jump = false;

    [SerializeField]
    float _currentSpeed = 0;

    float _walkSpeed = 2.5f;
    float _runSpeed = 5f;

    Vector3 _destPos;

    Animator _anim = null;
    
    void Start()
    {
        Managers.Input.KeyAction -= OnKeyBoard;
        Managers.Input.KeyAction += OnKeyBoard;
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;
        _anim = GetComponent<Animator>();
        _destPos = transform.position;
    }

    void Update()
    {
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Jump") == true && _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            _jump = false;
            _anim.SetBool("Jump", false);
        }
        ChangeAnimation();
        Move(_destPos - transform.position);
    }

    void ChangeAnimation()
    {
        if (_jump == true)
            _anim.SetBool("Jump", true);
        switch (_state)
        {
            case Define.PlayerState.Idle:
                _currentSpeed = 0;
                break;
            case Define.PlayerState.Walk:
                _currentSpeed = _walkSpeed;
                break;
            case Define.PlayerState.Run:
                _currentSpeed = _runSpeed;
                break;
        }
        _anim.SetFloat("Speed", _currentSpeed);
    }

    private float _turnDeltaSpeed = 0.1f;
    void Move(Vector3 dir)
    {
        if (dir.magnitude < 0.01f)
        {
            _state = Define.PlayerState.Idle;
            return;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), _turnDeltaSpeed);
        transform.position += dir.normalized * _currentSpeed * Time.deltaTime;
    }

    #region CallBack Function
    void OnMouseClicked(Define.MouseEvent evt)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int mask = LayerMask.GetMask("Floor");
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, mask))
        {
            if (_state != Define.PlayerState.Run)
                _state = Define.PlayerState.Walk;
            _destPos = hit.point;
        }
    }

    void OnKeyBoard()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _jump = true;
        if (Input.GetKeyDown(KeyCode.LeftShift))
            _state = Define.PlayerState.Run;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            _state = Define.PlayerState.Walk;
    }
    #endregion
}
