﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilJsonApiSerializer.Serialization.Representations.Resources;

namespace UtilJsonApiSerializer
{
    public interface ISerializerPipelineModule
    {
        void Run(Type resourceType, SingleResource resource);
    }
}
