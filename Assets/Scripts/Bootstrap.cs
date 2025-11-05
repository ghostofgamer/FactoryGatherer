using System;
using System.Collections;
using ResourcesCounterContent;
using SaveContent;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private ResourceHUD _resourceHUD;
    [SerializeField]private Settings _settings;
    
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
        SaveSystem.LoadFromPlayerPrefs();
        _resourceHUD.Init();
        _settings.Init();
        yield return new WaitForSeconds(1f);
        InitCompleted?.Invoke();
    }
}