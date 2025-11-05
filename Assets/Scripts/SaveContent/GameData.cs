using System;
using System.Collections.Generic;
using FactoryContent;

namespace SaveContent
{
    [Serializable]
    public class GameData
    {
        public List<FactoryData> factories = new List<FactoryData>();
        public List<ResourceSaveData> resources = new();
        public float volumeSound = 0.5f;
    }
}