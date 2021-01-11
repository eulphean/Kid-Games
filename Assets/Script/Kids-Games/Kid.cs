using UnityEngine;
using UnityEngine.AI; 

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

    // Agent that does everything. 
    private double _elapsedTime; 

    // Callback to create new position. 
    private Callback m_calcTarget;
    private Vector3 _target;

    //private const float WalkSpeed = 0.1f; 
    //private const float RunSpeed = 0.18f;
    private const float Walk01 = 0.05f;
    private const float Walk02 = 0.1f;
    private const float Walk03 = 0.12f;
    private const float Run01 = 0.15f;
    private const float Run02 = 0.18f;
    private const float Run03 = 0.30f;

    public float _agentSpeed = 0.0f; 

    private GameObject _debugTargetObject;
    private bool _hasAnimationStarted = false;
    private Renderer _renderer;

    private Quaternion _newRotation; 
    

    void Start()
    {
        // Get helper components. 
        _animator = this.transform.GetChild(0).GetComponent<Animator>();
        _renderer = GetComponent<Renderer>();

        // Set intial state. 
        chooseIdleState(); 
        chooseMotionState();
        setAnimation();

        _elapsedTime = 0; 
    }

    void Update()
    {
        if (!_hasAnimationStarted)
        {
            float t = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (t > 0.9f)
            {
                _hasAnimationStarted = true;
            }
        } else
        {
            // Update positions. 
            var currentPosition = transform.localPosition;
            var targetDirection = (_target - currentPosition).normalized;
            transform.localPosition = transform.localPosition + _agentSpeed * targetDirection * Time.deltaTime;
        }

        // Move the agent to the destination
        // Check if the agnet has reached. 
        if (hasReached())
        {
            print("Agent reached"); 
            if (isAgentInMotion())
            {
                print("Set Idle");
                chooseIdleState();
                _agentSpeed = 0.0f;
                _moveState = MoveState.None;
                _elapsedTime = 0;
                this.setAnimation();
            }
            else if (_elapsedTime > 5)
            {
                print("Waiting");
                chooseIdleState();
                chooseMotionState();
                this.setTarget(m_calcTarget(gameObject.name)); // Callback to create a new target. 
                this.setAnimation();
            }
        }

        transform.localRotation = Quaternion.Lerp(transform.localRotation, _newRotation, _agentSpeed); 

        _elapsedTime = !isAgentInMotion() ? _elapsedTime + Time.deltaTime : 0; 
    }

    bool hasReached()
    {
        var curPos = transform.localPosition;
        var d = Vector3.Distance(_target, curPos);
        return d < 0.05; // Threshold distance.
    }

    void rotateBody()
    {
        var currentPosition = transform.localPosition;
        var targetDirection = (_target - currentPosition).normalized;
        _newRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
    }

    public void setTarget(Vector3 t) {
        _target.Set(t.x, t.y, t.z);
        _debugTargetObject.transform.localPosition = _target;
        this.rotateBody(); 
    }

    public void setTargetObject(GameObject o)
    {
        _debugTargetObject = o; 
    }

    public void setCallback(Callback c) {
        m_calcTarget = c; 
    }

    void chooseIdleState() {
        // Idle01, Idle02, Idle03
        float p = Random.Range(0.0f, 1.0f);
        _idleState = (p >= 0 && p < 0.33) ? IdleState.Idle01 : 
                     (p >= 0.33 && p < 0.66) ? IdleState.Idle02 : 
                      IdleState.Idle03;

        print("Idle State: " + _idleState); 
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

        print("Motion State: " + _moveState);
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
                _agentSpeed = Walk01; 
                break;
            }

            case MoveState.Run01: {
                _animator.SetBool("isRunning01", true); 
                _agentSpeed = Run01;
                break;
            }

            case MoveState.Walk02: {
                _animator.SetBool("isWalking02", true);
                _agentSpeed = Walk02;
                break;
            }

            case MoveState.Run02: {
                _animator.SetBool("isRunning02", true);
                _agentSpeed = Run02; 
                break;
            }

            case MoveState.Walk03: {
                _animator.SetBool("isWalking03", true);
                _agentSpeed = Walk03; 
                break;
            }

            case MoveState.Run03: {
                _animator.SetBool("isRunning03", true);
                _agentSpeed = Run03; 
                break;
            }

            default: {
                break;
            }
        }
    }
}