using System;
using UnityEngine;

namespace TarodevController
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [SerializeField] private ScriptableStats _stats;
        private Rigidbody2D _rb;
        private CapsuleCollider2D _col;
        private FrameInput _frameInput;
        private Vector2 _frameVelocity;
        private bool _cachedQueryStartInColliders;

        #region Interface

        public Vector2 FrameInput => _frameInput.Move;
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;

        #endregion

        private float _time;

        private static int _looking_direction;
        private static int looking_direction
        {
            get { return _looking_direction; }
            set
            {
                if (_looking_direction != value)
                {
                    _looking_direction = value;
                    WorldController.OnPlayerDirectionChanged(_looking_direction);
                }
            }
        }

        private void Awake()
        {
            _looking_direction = 1;
            WorldController.OnPlayerDirectionChanged(_looking_direction);
            _rb = GetComponent<Rigidbody2D>();
            _rb.freezeRotation = true;
            _col = GetComponent<CapsuleCollider2D>();

            _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
        }

        private void Update()
        {
            _time += Time.deltaTime;
            GatherInput();
        }

        private void GatherInput()
        {
            _frameInput = new FrameInput
            {
                JumpDown = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.C),
                JumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.C),
                Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
            };

            if (_stats.SnapInput)
            {
                _frameInput.Move.x = Mathf.Abs(_frameInput.Move.x) < _stats.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.x);
                _frameInput.Move.y = Mathf.Abs(_frameInput.Move.y) < _stats.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.y);
            }

            if (_frameInput.JumpDown)
            {
                _jumpToConsume = true;
                _timeJumpWasPressed = _time;
            }
        }

        private void FixedUpdate()
        {
            CheckCollisions();
            HandleJump();
            HandleDirection();
            HandleGravity();
            if(_frameVelocity.x > 0.0f && looking_direction == -1) looking_direction = 1;
            if(_frameVelocity.x < 0.0f && looking_direction == 1) looking_direction = -1;
            ApplyMovement();
        }

        #region Collisions
        
        private float _frameLeftGrounded = float.MinValue;
        private float _frameLeftOnWall = float.MinValue;
        private bool _grounded;
        private String _onWall;
        private String _lastWall;
        private bool _wallGliding;
        bool _wallHitLeft;
        bool _wallHitRight;

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;
            // Ground and Ceiling
            bool groundHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.down, _stats.GrounderDistance, ~_stats.PlayerLayer);
            bool ceilingHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.up, _stats.GrounderDistance, ~_stats.PlayerLayer);

            _wallHitLeft = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.left, _stats.GrounderDistance, LayerMask.GetMask("ClimbableTile"));
            _wallHitRight = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.right, _stats.GrounderDistance, LayerMask.GetMask("ClimbableTile"));


            // Hit a Ceiling
            if (ceilingHit) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);

            // Landed on the Ground
            if (!_grounded && groundHit)
            {
                _grounded = true;
                _coyoteUsable = true;
                _bufferedJumpUsable = true;
                _endedJumpEarly = false;
                _lastWall = "none";
                GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
            }
            // Left the Ground
            else if (_grounded && !groundHit)
            {
                _grounded = false;
                _frameLeftGrounded = _time;
                GroundedChanged?.Invoke(false, 0);
            }

            // On a wall (for wallclimb)
            if((_wallHitLeft || _wallHitRight) && _onWall == "none" && _frameInput.Move.x != 0.0f && !_grounded && !(_lastWall == "left" && _wallHitLeft) && !(_lastWall == "right" && _wallHitRight)){
                if(_frameInput.Move.x > 0.0f){
                    _onWall = "right";
                    _lastWall = "right";
                }
                if(_frameInput.Move.x < 0.0f){
                    _onWall = "left";
                    _lastWall = "left";
                } 
                _coyoteUsable = true;
                _bufferedJumpUsable = true;
                _endedJumpEarly = false;
            }
            else if((!(_wallHitLeft || _wallHitRight) && _onWall != "none") || _frameInput.Move.x == 0.0f || _grounded){
                _onWall = "none";
                _frameLeftOnWall = _time;
            }
            Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
        }

        #endregion


        #region Jumping

        private bool _jumpToConsume;
        private bool _bufferedJumpUsable;
        private bool _endedJumpEarly;
        private bool _coyoteUsable;
        private float _timeJumpWasPressed;

        private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _stats.JumpBuffer;
        private bool CanUseCoyote => _coyoteUsable && !_grounded && _onWall == "none" && (_time < _frameLeftGrounded + _stats.CoyoteTime || _time < _frameLeftOnWall + _stats.CoyoteTime ) ;

        private void HandleJump()
        {
            if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && _rb.velocity.y > 0) _endedJumpEarly = true;

            if (!_jumpToConsume && !HasBufferedJump) return;

            if (_grounded || CanUseCoyote || _onWall != "none") ExecuteJump();

            _jumpToConsume = false;
        }

        private void ExecuteJump()
        {   
            _endedJumpEarly = false;
            _timeJumpWasPressed = 0;
            _bufferedJumpUsable = false;
            _coyoteUsable = false;
            _frameVelocity.y = _stats.JumpPower;
            if(_lastWall == "left") _frameVelocity.x += _stats.JumpPower * 0.8f; 
            if(_lastWall  == "right") _frameVelocity.x -= _stats.JumpPower * 0.8f; 
            if(_grounded && _wallHitLeft){
                _lastWall = "left"; 
            } 
            if(_grounded && _wallHitRight) _lastWall = "right"; 

            Jumped?.Invoke();
        }

        #endregion

        #region Horizontal

        private void HandleDirection()
        {
            if (_frameInput.Move.x == 0)
            {
                var deceleration = _grounded ? _stats.GroundDeceleration : _stats.AirDeceleration;
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime);
            }
        }

        #endregion

        #region Gravity

        private void HandleGravity()
        {
            if (_grounded && _frameVelocity.y <= 0f)
            {
                _frameVelocity.y = _stats.GroundingForce;
            }
            else
            {   
                float inAirGravity;
                bool onSomeWall = _onWall == "left" || _onWall == "right";
                
                if(onSomeWall){
                    inAirGravity = _stats.FallAcceleration / 10.0f;
                    if(!_wallGliding){
                        _frameVelocity.y = -3.0f;
                        _wallGliding = true;
                    }
                }
                else{
                    _wallGliding = false;
                    inAirGravity = _stats.FallAcceleration;
                } 
                
                
                if (_endedJumpEarly && _frameVelocity.y > 0) inAirGravity *= _stats.JumpEndEarlyGravityModifier;
                _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
            }
        }

        #endregion

        private void ApplyMovement() => _rb.velocity = _frameVelocity;


        public void hitByEnemy(Vector3 enemyPos, float attack_power)
        {
            float direction = Mathf.Sign(transform.position.x - enemyPos.x);
            _frameVelocity += new Vector2(direction * attack_power*1.3f, attack_power);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_stats == null) Debug.LogWarning("Please assign a ScriptableStats asset to the Player Controller's Stats slot", this);
        }
#endif
    }

    public struct FrameInput
    {
        public bool JumpDown;
        public bool JumpHeld;
        public Vector2 Move;
    }

    public interface IPlayerController
    {
        public event Action<bool, float> GroundedChanged;

        public event Action Jumped;
        public Vector2 FrameInput { get; }
    }
}