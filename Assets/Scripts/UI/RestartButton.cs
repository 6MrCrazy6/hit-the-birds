using DG.Tweening;
using System;
using TouchScript.Gestures;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI 
{ 
    public class RestartButton : MonoBehaviour
    {
        [SerializeField] private GameObject buttonRestart;

        private float clickAnimationDuration = 0.3f;
        private float delayBeforeSwitch = 1.5f;
        private float inputCooldownDuration = 1f;

        private AudioSource audioSource;
        private bool isInputEnabled = true;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            GetComponent<TapGesture>().Tapped += tappedHandler;
        }

        private void OnDisable()
        {
            GetComponent<TapGesture>().Tapped -= tappedHandler;
        }

        private void tappedHandler(object sender, EventArgs e)
        {
            if (!isInputEnabled)
                return;

            DeactivateButton();
            OnButtonClick();
        }

        private void OnButtonClick()
        {
            audioSource.Play();

            buttonRestart.transform.DOScale(0.9f, clickAnimationDuration).OnComplete(() =>
            {
                buttonRestart.transform.DOScale(1f, clickAnimationDuration).OnComplete(() =>
                {
                    DOVirtual.DelayedCall(delayBeforeSwitch, ChangeScene);
                });
            });
        }

        private void ChangeScene()
        {
            isInputEnabled = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
            Invoke(nameof(EnableInput), inputCooldownDuration);
        }

        private void EnableInput()
        {
            isInputEnabled = true;
        }

        private void DeactivateButton()
        {
            RestartButton startGameButton = FindObjectOfType<RestartButton>();
            TapGesture button = startGameButton.GetComponent<TapGesture>();

            startGameButton.enabled = false;
            button.enabled = false;
        }
    }
}
