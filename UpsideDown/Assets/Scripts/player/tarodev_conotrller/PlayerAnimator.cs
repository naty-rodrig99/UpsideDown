using UnityEngine;
using gamespace;

namespace TarodevController
{
    /// <summary>
    /// VERY primitive animator example.
    /// </summary>
    public class PlayerAnimator : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Animator _anim;

        //[SerializeField] private SpriteRenderer _sprite;

        //[Header("Settings")] [SerializeField, Range(1f, 3f)]
        private float _maxIdleSpeed = 2;

        [SerializeField] private float _maxTilt = 5;
        [SerializeField] private float _tiltSpeed = 20;

        [Header("Particles")] [SerializeField] private ParticleSystem _jumpParticles;
        [SerializeField] private ParticleSystem _launchParticles;
        [SerializeField] private ParticleSystem _moveParticles;
        [SerializeField] private ParticleSystem _landParticles;

        [Header("Audio Clips")] [SerializeField]
        private AudioClip[] _footsteps;

        private AudioSource _source;
        private IPlayerController _player;
        private bool _grounded;
        private ParticleSystem.MinMaxGradient _currentGradient;

        WorldType _current_world;

        void UpdateWorld(WorldType type){
            _current_world = type;
            if(_current_world == WorldType.GoodWorld)
            {
                _anim.SetBool("good_world", true);
            }
            else
            {
                _anim.SetBool("good_world", false);
            }
        }

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
            _player = GetComponentInParent<IPlayerController>();
        }

        private void OnEnable()
        {
            _player.Jumped += OnJumped;
            _player.GroundedChanged += OnGroundedChanged;
            WorldController.OnWorldChanged += UpdateWorld;

            //_moveParticles.Play();
             _moveParticles.Stop();
        }

        private void OnDisable()
        {
            _player.Jumped -= OnJumped;
            _player.GroundedChanged -= OnGroundedChanged;
            WorldController.OnWorldChanged -= UpdateWorld;

            _moveParticles.Stop();
        }

        private void Update()
        {
            if (_player == null) return;

            DetectGroundColor();

            //HandleSpriteFlip();

            HandleIdleSpeed();

            HandleCharacterTilt();

            HandleMovementAnimations();
        }

        private void HandleSpriteFlip()
        {
            //if (_player.FrameInput.x != 0) _sprite.flipX = _player.FrameInput.x < 0;
        }

        private void HandleIdleSpeed()
        {
            var inputStrength = Mathf.Abs(_player.FrameInput.x);
            //_anim.SetFloat(IdleSpeedKey, Mathf.Lerp(1, _maxIdleSpeed, inputStrength));
            //_moveParticles.transform.localScale = Vector3.MoveTowards(_moveParticles.transform.localScale, Vector3.one * inputStrength, 2 * Time.deltaTime);
        }

        private void HandleCharacterTilt()
        {
            var runningTilt = _grounded ? Quaternion.Euler(0, 0, _maxTilt * _player.FrameInput.x) : Quaternion.identity;
            //_anim.transform.up = Vector3.RotateTowards(_anim.transform.up, runningTilt * Vector2.up, _tiltSpeed * Time.deltaTime, 0f);
        }

        private void HandleMovementAnimations()
        {
            float horizontalInput = _player.FrameInput.x;
            
            if (horizontalInput == 0 && _current_world == WorldType.GoodWorld)
            {
                _anim.SetBool("good_Right", false);
                _anim.SetBool("good_Left", false);
            }
            else if (horizontalInput > 0 && _current_world == WorldType.GoodWorld)
            {
                _anim.SetBool("good_Right", true);
            } else if(horizontalInput < 0 && _current_world == WorldType.GoodWorld)
            {
                _anim.SetBool("good_Left", true);
            } else if (horizontalInput == 0 && _current_world == WorldType.BadWorld)
            {
                _anim.SetBool("bad_Right", false);
                _anim.SetBool("bad_Left", false);
            } else if (horizontalInput > 0 && _current_world == WorldType.BadWorld)
            {
                _anim.SetBool("bad_Right", true);
            } else if (horizontalInput < 0 && _current_world == WorldType.BadWorld)
            {
                _anim.SetBool("bad_Left", true);
            }
        }

        private void OnJumped()
        {
            //_anim.SetTrigger(JumpKey);
            //_anim.ResetTrigger(GroundedKey);

            if (_grounded) // Avoid coyote
            {
                SetColor(_jumpParticles);
                SetColor(_launchParticles);
                _jumpParticles.Play();
            }
        }

        private void OnGroundedChanged(bool grounded, float impact)
        {
            _grounded = grounded;
            
            if (grounded)
            {
                DetectGroundColor();
                SetColor(_landParticles);

                //_anim.SetTrigger(GroundedKey);
                _source.PlayOneShot(_footsteps[Random.Range(0, _footsteps.Length)]);
                //_moveParticles.Play();

                _landParticles.transform.localScale = Vector3.one * Mathf.InverseLerp(0, 40, impact);
                _landParticles.Play();
                _anim.SetBool("isJumping", false);
            }
            else
            {
                _moveParticles.Stop();
                _anim.SetBool("isJumping", true);
            }
        }

        private void DetectGroundColor()
        {
            var hit = Physics2D.Raycast(transform.position, Vector3.down, 2);

            if (!hit || hit.collider.isTrigger || !hit.transform.TryGetComponent(out SpriteRenderer r)) return;
            var color = new Color(0.2f, 0.8f, 0.3f, 1.0f);
            _currentGradient = new ParticleSystem.MinMaxGradient(color * 0.9f, color * 1.2f);
            //SetColor(_moveParticles);
        }

        private void SetColor(ParticleSystem ps)
        {
            var main = ps.main;
            
            if(_current_world == WorldType.GoodWorld){
                main.startColor = new Color(0.2039f, 0.5647f, 0.2392f, 1.0f);
            }
            if(_current_world == WorldType.BadWorld){
                main.startColor = new Color(0.2549f, 0.3176f, 0.1882f, 1.0f);
            }
        }

        private static readonly int GroundedKey = Animator.StringToHash("Grounded");
        private static readonly int IdleSpeedKey = Animator.StringToHash("IdleSpeed");
        private static readonly int JumpKey = Animator.StringToHash("Jump");
    }
}