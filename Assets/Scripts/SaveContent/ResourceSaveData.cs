using System;

namespace SaveContent
{
    [Serializable]
    public class ResourceSaveData 
    {
        public string resourceName;
        public int amount;

        public ResourceSaveData(string name, int amount)
        {
            resourceName = name;
            this.amount = amount;
        }
    }
}