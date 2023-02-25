//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameDevWare.Dynamic.Expressions.CSharp;
using GameFramework.DataTable;
using System;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    [Serializable]
    public class ModelData : AircraftData
    {
        public CampType model_type;
        public RoleData roleData;
        public ModelData(int entityId, int typeId,string type,int level,int stage_index = 0 )
            : base(entityId, typeId, CampType.Player)
        {
            IDataTable<DREntity> dtEntity = GameEntry.DataTable.GetDataTable<DREntity>();
            DREntity drEntity = dtEntity.GetDataRow(typeId);

            roleData = new RoleData();
            roleData.rarity = drEntity.Rarity;
            roleData.type = drEntity.Type;
            roleData.priority = drEntity.Priority;
            roleData.index = drEntity.Id;
            if (type == "Player")
            {
                model_type = CampType.Player;
            }
            else
            {
                model_type = CampType.Enemy;
                IDataTable<DRStage1> dtStage1 = GameEntry.DataTable.GetDataTable<DRStage1>();
                DRStage1 drStage1 = dtStage1.GetDataRow(stage_index);
                roleData.extra_hp = CSharpExpression.Evaluate<int>(string.Format(drStage1.ExtraHp, stage_index, level, roleData.rarity));

            }
            roleData.base_hp = CSharpExpression.Evaluate<int>(string.Format(drEntity.BaseHp, level, roleData.rarity));
            roleData.total_hp = roleData.base_hp + roleData.extra_hp; 
            roleData.dead_offer_exp = CSharpExpression.Evaluate<int>(string.Format(drEntity.BaseHp, roleData.rarity, level));
            roleData.round_use_card_count = drEntity.RoundUseCardCount;
        }

        public void SetHp(int value)
        {
            roleData.total_hp = value;
        }
    }
}
