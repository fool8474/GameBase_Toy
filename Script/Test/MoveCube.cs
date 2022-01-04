using System;
using System.Collections.Generic;
using System.Linq;
using Script.Event;
using Script.Manager.Util.Log;
using UniRx;
using UnityEngine;

namespace Script.Test
{
    public enum MoveCubeState
    {
        NONE,
        IDLE,
        MOVE,
        FORCE
    }

    public enum TestMoveDirection
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    
    public interface ITestBaseState
    {
        void EnterState();
        void UpdateState();
        void EndState();
    }
    
    public class MoveCubeStateIdle : ITestBaseState
    {
        private MoveCube _cube;

        public MoveCubeStateIdle(MoveCube targetObject)
        {
            _cube = targetObject;
        }
        
        public void EnterState()
        {
            _cube.ChangeColor.Execute(Color.magenta);
        }

        public void UpdateState()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _cube.StateChange.Execute(MoveCubeState.FORCE);
                return;
            }

            if (Input.GetKeyDown(KeyCode.W) ||
                Input.GetKeyDown(KeyCode.A) ||
                Input.GetKeyDown(KeyCode.S) ||
                Input.GetKeyDown(KeyCode.D))
            {
                _cube.StateChange.Execute(MoveCubeState.MOVE);
            }
        }

        public void EndState()
        {
        }
    }

    public class MoveCubeStateMove : ITestBaseState
    {
        private MoveCube _cube;
        private Dictionary<KeyCode, TestMoveDirection> _moveDirectionDic;
        
        public MoveCubeStateMove(MoveCube targetObject)
        {
            _cube = targetObject;

            _moveDirectionDic = new Dictionary<KeyCode, TestMoveDirection>
            {
                {KeyCode.W, TestMoveDirection.UP}, {KeyCode.A, TestMoveDirection.LEFT},
                {KeyCode.S, TestMoveDirection.DOWN}, {KeyCode.D, TestMoveDirection.RIGHT}
            };
        }

        public void EnterState()
        {
            _cube.ChangeColor.Execute(Color.blue);
        }

        public void UpdateState()
        {
            foreach (var kvp in _moveDirectionDic.Where(kvp => Input.GetKey(kvp.Key)))
            {
                _cube.MoveEvent.Execute(kvp.Value);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _cube.StateChange.Execute(MoveCubeState.FORCE);
                return;
            }
            
            // Check walk state is over
            if (_moveDirectionDic.Keys.Any(Input.GetKey))
            {
                return;
            }

            _cube.StateChange.Execute(MoveCubeState.IDLE);
        }

        public void EndState()
        {
        }
    }

    public class MoveCubeStateForce : ITestBaseState
    {
        private MoveCube _cube;

        public MoveCubeStateForce(MoveCube targetObject)
        {
            _cube = targetObject;
        }
        
        public void EnterState()
        {
            _cube.ChangeColor.Execute(Color.green);
        }

        public void UpdateState()
        {
            if (Input.GetKey(KeyCode.Space) == false)
            {
                _cube.StateChange.Execute(MoveCubeState.IDLE);
                return;
            }

            if (Input.GetKeyDown(KeyCode.W) ||
                Input.GetKeyDown(KeyCode.A) ||
                Input.GetKeyDown(KeyCode.S) ||
                Input.GetKeyDown(KeyCode.D))
            {
                _cube.StateChange.Execute(MoveCubeState.MOVE);
            }
        }

        public void EndState()
        {
        }
    }

    public class MoveCubeStateMachine
    {
        private Dictionary<MoveCubeState, ITestBaseState> _stateDic;
        private MoveCubeState _currState = MoveCubeState.NONE;

        public MoveCubeStateMachine()
        {
            _stateDic = new Dictionary<MoveCubeState, ITestBaseState>();
        }

        public void AddState(MoveCubeState stateType, ITestBaseState state)
        {
            if (_stateDic.ContainsKey(stateType) == false)
            {
                _stateDic.Add(stateType, state);
            }
        }

        public void UpdateState()
        {
            Log.DF(LogCategory.STATE_MACHINE, "Curr state is ... {0}", _currState);
            if (_currState != MoveCubeState.NONE)
            {
                _stateDic[_currState].UpdateState();
            }
        }

        public void ChangeState(MoveCubeState stateType)
        {
            if (IsInvalidState(stateType))
            {
                return;
            }

            if (_currState != MoveCubeState.NONE)
            {
                _stateDic[_currState].EndState();
            }

            _currState = stateType;
            _stateDic[_currState].EnterState();
        }

        public bool IsInvalidState(MoveCubeState stateType)
        {
            return _currState == stateType || !_stateDic.ContainsKey(stateType);
        }
    }
    
    public class MoveCube : MonoBehaviour, IDisposable
    {
        public float MoveSpeed = 1.0f;
        public GameObject _model;

        private MoveCubeStateMachine _stateMachine;
        private Animator _animator;
        private Material _material;

        #region Event
        public IEventCommand<Color> ChangeColor => _changeColor;
        public IEventCommand<MoveCubeState> StateChange => _stateChange;
        public IEventCommand<TestMoveDirection> MoveEvent => _moveEvent;

        private readonly EventCommand<Color> _changeColor = new EventCommand<Color>();
        private readonly EventCommand<MoveCubeState> _stateChange = new EventCommand<MoveCubeState>();
        private readonly EventCommand<TestMoveDirection> _moveEvent = new EventCommand<TestMoveDirection>();
        #endregion

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private Dictionary<MoveCubeState, string> _animationTriggerDic;
        
        private void Start()
        {
            GetComponent();
            InitState();
            AddEvent();
        }

        private void GetComponent()
        {
            _animator = _model.GetComponent<Animator>();
            _material = _model.GetComponent<MeshRenderer>().material;
        }

        private void ChangeState(MoveCubeState state)
        {
            Log.DF(LogCategory.ANIMATION, "State Change Check {0}", state);
            if (_stateMachine.IsInvalidState(state))
            {
                return;
            }
            
            Log.DF(LogCategory.ANIMATION, "State Change OK {0}", state);
            _animator.SetTrigger(_animationTriggerDic[state]);
            _stateMachine.ChangeState(state);
        }

        private void AddEvent()
        {
            MoveEvent.Subscribe(MoveDirection).AddTo(_disposable);
            StateChange.Subscribe(ChangeState).AddTo(_disposable);
            ChangeColor.Subscribe(ChangeMaterialColor).AddTo(_disposable);
        }
        
        private void InitState()
        {
            _stateMachine = new MoveCubeStateMachine();
            _stateMachine.AddState(MoveCubeState.IDLE, new MoveCubeStateIdle(this));
            _stateMachine.AddState(MoveCubeState.MOVE, new MoveCubeStateMove(this));
            _stateMachine.AddState(MoveCubeState.FORCE, new MoveCubeStateForce(this));

            _animationTriggerDic = new Dictionary<MoveCubeState, string>
            {
                {MoveCubeState.IDLE, "ToIdle"}, 
                {MoveCubeState.MOVE, "ToMove"}, 
                {MoveCubeState.FORCE, "ToForce"}
            };
            
            _stateMachine.ChangeState(MoveCubeState.IDLE);
        }
        
        private void Update()
        {
            _stateMachine.UpdateState();
        }

        private void ChangeMaterialColor(Color color)
        {
            _material.color = color;
        }
        
        private void MoveDirection(TestMoveDirection moveDirection)
        {
            switch (moveDirection)
            {
                case TestMoveDirection.UP:
                    transform.position += new Vector3(0,0, MoveSpeed * Time.deltaTime);
                    break;
                case TestMoveDirection.DOWN:
                    transform.position -= new Vector3(0,0, MoveSpeed * Time.deltaTime);
                    break;
                case TestMoveDirection.LEFT:
                    transform.position -= new Vector3(MoveSpeed * Time.deltaTime,0, 0);
                    break;
                case TestMoveDirection.RIGHT:
                    transform.position += new Vector3(MoveSpeed * Time.deltaTime,0, 0);
                    break;
            }
        }

        public void Dispose()
        {
            _changeColor?.Dispose();
            _stateChange?.Dispose();
            _moveEvent?.Dispose();
            _disposable?.Dispose();
        }
    }
}
