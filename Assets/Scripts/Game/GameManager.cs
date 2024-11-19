using TMPro;
using UnityEngine;
using DG.Tweening;
using System.IO;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        private int _countBirds = 0;
        private int _timerSecond = 30;
        private float _timerFloat = 0;
        private bool _gameEnded = false;

        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _timerText;

        [SerializeField] private GameObject _winPanel;
        [SerializeField] private GameObject _treeArea;
        [SerializeField] private GameObject _birdsParent;

        [SerializeField] private GameObject _winFX;
        [SerializeField] private GameObject _confettiFx;

        private void Awake()
        {
            LoadTimerFromSettings();
        }

        private void Start()
        {
            UpdateScoreText();
            UpdateTimerText();
        }

        private void Update()
        {
            if (_gameEnded) return;

            if (AllBirdsClicked())
            {
                _timerSecond = 0;
                UpdateTimerText();
                TimeOut();
                return;
            }
            else
            {
                if (_timerSecond > 0)
                {
                    _timerFloat += Time.deltaTime;
                    if (_timerFloat >= 1f)
                    {
                        _timerSecond--;
                        _timerFloat = 0;
                        UpdateTimerText();
                    }
                }
                else
                {
                    _timerSecond = 0;
                    TimeOut();
                }
            }
        }

        private bool AllBirdsClicked()
        {
            foreach (Transform bird in _birdsParent.transform)
            {
                BirdControll birdComponent = bird.GetComponent<BirdControll>();

                if (birdComponent != null && !birdComponent.isClicked)
                {
                    return false;
                }
            }
            return true; 
        }

        private void UpdateScoreText()
        {
            _scoreText.text = $"{_countBirds:D2}";
        }

        private void UpdateTimerText()
        {
            int minutes = _timerSecond / 60;
            int seconds = _timerSecond % 60;
            _timerText.text = "Time: " + string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        public void IncrementBirdCount()
        {
            _countBirds++;
            UpdateScoreText();
        }

        private void TimeOut()
        {
            if (_gameEnded) return;

            _gameEnded = true;

            _timerSecond = 0;
            _timerText.gameObject.SetActive(false);

            SpriteRenderer[] treeSprites = _treeArea.GetComponentsInChildren<SpriteRenderer>();
            foreach (var sprite in treeSprites)
            {
                sprite.DOFade(0, 2f).OnComplete(() => _treeArea.SetActive(false));
            }

            foreach (Transform bird in _birdsParent.transform)
            {
                BirdControll birdComponent = bird.GetComponent<BirdControll>();
                if (birdComponent != null)
                {
                    float directionMultiplier = birdComponent.InitialFlip ? -1 : 1;
                    Vector3 targetPosition = bird.position + new Vector3(10f * directionMultiplier, 0f, 0f);
                    birdComponent.FlyAway();
                }
            }

            DOVirtual.DelayedCall(3.5f, () =>
            {
                _winPanel.transform.DOLocalMove(new Vector3(0, 17, 0), 1f);
            });

            CreateFx();
        }

        private void CreateFx()
        {
            GameObject fx = Instantiate(_winFX, transform.position, Quaternion.identity);
            GameObject confetti = Instantiate(_confettiFx, transform.position, Quaternion.identity);
            fx.transform.GetChild(0).GetComponent<AudioSource>().Play();
        }

        private void LoadTimerFromSettings()
        {
            string path = Path.Combine(Application.dataPath, "../settings.json");

            if (File.Exists(path))
            {
                string jsonContent = File.ReadAllText(path);
                SettingsData settingsData = JsonUtility.FromJson<SettingsData>(jsonContent);

                foreach (var option in settingsData.options)
                {
                    if (option.displayTitle == "Timer" && option.values.Length > 0)
                    {
                        if (int.TryParse(option.values[0].value, out int timerValue))
                        {
                            _timerSecond = timerValue;
                        }
                        break;
                    }
                }
            }
        }
    }
}
