using DG.Tweening;
using System;
using TouchScript.Gestures;
using UnityEngine;


namespace Game
{
    public class BirdControll : MonoBehaviour
    {
        private const string IsFlying = nameof(IsFlying);

        [SerializeField] private GameObject _shootFx;
        [SerializeField] private bool _initialFlip;
        [SerializeField, Range(0, 10)] private float _flyDuration = 2.5f;

        public Transform treePoint;
        public GameObject _featherFx;

        private AudioSource _audioSource;
        private GameManager _gameManager;
        private Animator _birdAnimator;

        private bool _isFlying = false;
        public bool isClicked { get; private set; } = false; 

        public bool InitialFlip => _initialFlip;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _birdAnimator = GetComponentInChildren<Animator>();
            _gameManager = FindObjectOfType<GameManager>();
        }

        private void OnEnable() =>
            GetComponent<TapGesture>().Tapped += TappedHandler;

        private void OnDisable() =>
            GetComponent<TapGesture>().Tapped -= TappedHandler;

        private void TappedHandler(object sender, EventArgs e)
        {
            if (isClicked) return; 

            isClicked = true; 
            CreateFx();
            _gameManager.IncrementBirdCount();
            FlyAway();
            TapGesture tapGesture = this.GetComponent<TapGesture>();
            tapGesture.enabled = false; 
        }

        public void FlyAway()
        {
            if (_isFlying) return;
            _isFlying = true;

            if (_birdAnimator != null)
                _birdAnimator.SetBool(IsFlying, true);

            DOVirtual.DelayedCall(0.3f, () =>
            {
                Vector3 targetPosition = CalculateFlyDirection();
                if (Mathf.Abs(transform.rotation.eulerAngles.z) > 0.1f)
                {
                    transform.DORotate(new Vector3(0, transform.rotation.eulerAngles.y, 0), 0.5f).OnComplete(() =>
                    {
                        AnimateFlight(targetPosition);
                    });
                }
                else
                {
                    AnimateFlight(targetPosition);
                }
            });

            CreateFeatherFxAtTree();
        }

        private void AnimateFlight(Vector3 targetPosition)
        {
            transform.DOMoveY(transform.position.y + 0.5f, 0.5f).OnComplete(() =>
            {
                transform.DOMoveX(targetPosition.x, _flyDuration).SetEase(Ease.Linear).OnComplete(() =>
                {
                    Destroy(gameObject);
                });

                transform.DOBlendableMoveBy(new Vector3(0, 0.2f, 0), 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
            });
        }

        private Vector3 CalculateFlyDirection()
        {
            Camera camera = Camera.main;

            Vector3 screenBottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
            Vector3 screenTopRight = camera.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

            float leftEdge = screenBottomLeft.x;
            float rightEdge = screenTopRight.x;

            Vector3 birdPosition = transform.position;

            if (birdPosition.x < (leftEdge + rightEdge) / 2)
            {
                return new Vector3(leftEdge - 10f, birdPosition.y, birdPosition.z);
            }
            else
            {
                return new Vector3(rightEdge + 10f, birdPosition.y, birdPosition.z);
            }
        }

        private void CreateFeatherFxAtTree()
        {
            if (treePoint != null)
            {
                GameObject fx = Instantiate(_featherFx, treePoint.position, Quaternion.identity);
                fx.GetComponent<AudioSource>().Play();
            }
        }

        private void CreateFx()
        {
            GameObject fx = Instantiate(_shootFx, transform.position, Quaternion.identity);
            fx.transform.GetChild(0).GetComponent<AudioSource>().Play();
            fx.transform.GetChild(0).GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.8f, 1.0f);
        }

        public void DoFlip(bool flip)
        {
            _initialFlip = flip;
        }
    }
}
