using System;
using UnityEngine;

namespace FactoryContent
{
    [Serializable]
    public class FactoryData
    {
        public int ID;
        public Vector3 Position;   
        public string ResourceType;
        public int StoredAmount;
    }
}