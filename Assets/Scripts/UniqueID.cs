using System;
using UnityEngine;

public class UniqueID : MonoBehaviour
{
    [SerializeField, HideInInspector] private string _id;

    public string ID => _id;

    private void Awake()
    {
        if (string.IsNullOrEmpty(_id))
        {
            _id = Guid.NewGuid().ToString();
            Debug.Log("!новый ID " + _id);
        }
    }
}