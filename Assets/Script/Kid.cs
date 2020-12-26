using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveState {
    None, 
    Run01,
    Walk01,
    Run02,
    Walk02,
    Run03,
    Walk03
}

public enum IdleState {
    Idle01,
    Idle02,
    Idle03
}

public class Kid : MonoBehaviour
{
    private Animator _animator; 
    private MoveState _moveState; 
    private IdleState _idleState; 

    // Start is called before the first frame update
    void Start()
    {
        _idleState = IdleState.Idle01; 
        _moveState = MoveState.None; 
        _animator = this.transform.GetChild(0).GetComponent<Animator>(); 
        setAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("Button Pressed"); 
        }

        if (Input.GetMouseButtonUp(0)) {
            Debug.Log("Button Release"); 
            calculateNextState();
            setAnimation();
        }
    }

    void calculateNextState() {
        if (isAgentInMotion()) {
            chooseIdleState(); 
            _moveState = MoveState.None; 
            Debug.Log("Next Idle State: " + _idleState);
        } else {
            chooseMotionState(); 
            Debug.Log("Next Move State: " + _moveState); 
        }
    }

    void chooseIdleState() {
        // Idle01, Idle02, Idle03
        float p = Random.Range(0.0f, 1.0f);
        _idleState = (p >= 0 && p < 0.33) ? IdleState.Idle01 : 
                     (p >= 0.33 && p < 0.66) ? IdleState.Idle02 : 
                      IdleState.Idle03; 
    }

    void chooseMotionState() {
        // Use probability to calculate a new animation state. 
        float p = Random.Range(0.0f, 1.0f);
        if (p >= 0 && p < 0.33) {
            setMoveState(1);
        } else if (p >= 0.33 && p < 0.66) {
            setMoveState(2);
        } else {
            setMoveState(3); 
        }                      
    }

    void setMoveState(int num) {
        float p = Random.Range(0.0f, 1.0f);
        if (num == 1) {
            _moveState = p <= 0.5 ? MoveState.Walk01 : MoveState.Run01; 
        } else if (num == 2) {
            _moveState = p <= 0.5 ? MoveState.Walk02 : MoveState.Run02;
        } else if (num == 3) {
            _moveState = p <= 0.5 ? MoveState.Walk03 : MoveState.Run03;
        }
    }

    bool isAgentInMotion() {
       return (_moveState == MoveState.Walk01 || _moveState == MoveState.Run01 
                || _moveState == MoveState.Walk02 || _moveState == MoveState.Run02
                    || _moveState == MoveState.Walk03 || _moveState == MoveState.Run03); 
    }

    void setAnimation() {
        switch (_idleState) {
            case IdleState.Idle01: {
                _animator.SetBool("isIdle01", true); 
                _animator.SetBool("isIdle02", false); 
                _animator.SetBool("isIdle03", false); 
                setMoveState(); 
                break;
            }

            case IdleState.Idle02: {
                _animator.SetBool("isIdle02", true);
                _animator.SetBool("isIdle01", false);
                _animator.SetBool("isIdle03", false);
                setMoveState();
                break;
            }

            case IdleState.Idle03: {
                _animator.SetBool("isIdle03", true);
                _animator.SetBool("isIdle01", false);
                _animator.SetBool("isIdle02", false);
                setMoveState();
                break;
            }

            default: {
                break;
            }
        }
    }

    void setMoveState() {
        switch(_moveState) {
            case MoveState.None: {
                _animator.SetBool("isWalking01", false); 
                _animator.SetBool("isRunning01", false);
                _animator.SetBool("isWalking02", false);
                _animator.SetBool("isRunning02", false);
                _animator.SetBool("isWalking03", false);
                _animator.SetBool("isRunning03", false); 
                break; 
            }

            case MoveState.Walk01: {
                _animator.SetBool("isWalking01", true);
                break;
            }

            case MoveState.Run01: {
                _animator.SetBool("isRunning01", true); 
                break;
            }

            case MoveState.Walk02: {
                _animator.SetBool("isWalking02", true);
                break;
            }

            case MoveState.Run02: {
                _animator.SetBool("isRunning02", true);
                break;
            }

            case MoveState.Walk03: {
                _animator.SetBool("isWalking03", true);
                break;
            }

            case MoveState.Run03: {
                _animator.SetBool("isRunning03", true);
                break;
            }

            default: {
                break;
            }
        }
    }
}