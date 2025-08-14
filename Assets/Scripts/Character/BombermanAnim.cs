using UnityEngine;

[RequireComponent(typeof(Bomberman), typeof(Animator))]
public class BombermanAnim : MonoBehaviour
{
    private Bomberman _player;
    private Animator _animator;

    private enum AnimationState
    {
        WalkDown = 1,
        WalkUp = 2,
        WalkLeft = 3,
        WalkRight = 4,
        IdleDown = 10,
        IdleUp = 20,
        IdleLeft = 30,
        IdleRight = 40
    }

    private AnimationState _currentIdleState = AnimationState.IdleDown;
    
    private static readonly int TransitionState = Animator.StringToHash("transition");

    private void Awake()
    {
        _player = GetComponent<Bomberman>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        if (_player.Direction == Vector2.zero)
        {
            SetAnimationState(_currentIdleState);
        }
        else
        {
            if (Mathf.Abs(_player.Direction.x) > Mathf.Abs(_player.Direction.y))
            {
                // Movimento horizontal é dominante
                if (_player.Direction.x > 0)
                {
                    SetAnimationState(AnimationState.WalkRight);
                    _currentIdleState = AnimationState.IdleRight;
                }
                else
                {
                    SetAnimationState(AnimationState.WalkLeft);
                    _currentIdleState = AnimationState.IdleLeft;
                }
            }
            else
            {
                // Movimento vertical é dominante ou igual
                if (_player.Direction.y > 0)
                {
                    SetAnimationState(AnimationState.WalkUp);
                    _currentIdleState = AnimationState.IdleUp;
                }
                else
                {
                    SetAnimationState(AnimationState.WalkDown);
                    _currentIdleState = AnimationState.IdleDown;
                }
            }
        }
    }

    private void SetAnimationState(AnimationState state)
    {
        _animator.SetInteger(TransitionState, (int)state);
    }
}