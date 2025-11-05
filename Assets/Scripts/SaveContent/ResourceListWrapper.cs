using System;
using System.Collections.Generic;

namespace SaveContent
{
    [Serializable]
    public class ResourceListWrapper
    {
        public List<ResourceSaveData> resources;

        public ResourceListWrapper(List<ResourceSaveData> resources)
        {
            this.resources = resources;
        }
    }
}
