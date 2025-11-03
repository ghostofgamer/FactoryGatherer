using ResourcesCounterContent;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField]private ResourceHUD _resourceHUD;
    
    private void Awake()
    {
        // _resourceHUD.Init();
    }

    private void Start()
    {
        Initialization();
    }

    private void Initialization()
    {
        _resourceHUD.Init();
    }
}