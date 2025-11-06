using System;
using System.Collections;
using ResourcesCounterContent;
using SaveContent;
using UI.Screens;
using UnityEngine;

namespace Initialization
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private ResourceHUD _resourceHUD;
        [SerializeField] private Settings _settings;
        [SerializeField] private LoadingScreen _loadingScreen;

        private WaitForSeconds _waitForSeconds = new WaitForSeconds(1f);

        public event Action InitCompleted;

        private void Start()
        {
            Initialization();
        }

        private void Initialization()
        {
            StartCoroutine(StartInitialization());
        }

        private IEnumerator StartInitialization()
        {
            _loadingScreen.Show();
            SaveSystem.LoadFromPlayerPrefs();
            _loadingScreen.SetProgress(0.3f);
            _resourceHUD.Init();
            _settings.Init();
            _loadingScreen.SetProgress(0.6f);
            yield return _waitForSeconds;
            _loadingScreen.SetProgress(1f);
            InitCompleted?.Invoke();
            yield return _loadingScreen.FadeOut();
        }
    }
}