//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using UnityEngine;

namespace StarForce
{
    [Serializable]
    public class ModelData : AircraftData
    {
        public ModelData(int entityId, int typeId)
            : base(entityId, typeId, CampType.Player)
        {
            
        }
    }
}
