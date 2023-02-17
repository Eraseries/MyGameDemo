using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using System.Reflection;
/// <summary>
/// 战斗面板
/// </summary>
namespace StarForce
{
    public class BattlePanel
    {

         public void Init()
        {
            InitPlayer();
            InitMonster();
        }

        public void InitPlayer()
        {
            int current_stage = 1; //当前关卡
            IDataTable<DRStage1> dtPlayer = GameEntry.DataTable.GetDataTable<DRStage1>();
            for (int i = 1; i <= current_stage; i++)
            {
                Log.Error(i);
                int index = i;
                DRStage1 data = dtPlayer.GetDataRow(index);

                string[] model_id_s = new string[data.EnemyCount];
                int count = data.EnemyCount;
                if(count > 1)
                {
                    model_id_s = data.EnemyModel.Split('/');
                }
                else
                {
                    model_id_s[0] = data.EnemyModel;
                }



                for (int j = 1; j <= data.EnemyCount; j++)
                {
                    int id = j - 1 ;
                    GameEntry.Entity.ShowModel(new ModelData(GameEntry.Entity.GenerateSerialId(), int.Parse(model_id_s[id])));
                }
            }
        }

        public void InitMonster()
        {

        }


        public void Show()
        {
            for (int i = 5; i <= 11; i++)
            {
                int index = i;
                if (GameEntry.Entity.HasEntity(index))
                {
                    IDataTable<DRBattleScene1> dtPlayer = GameEntry.DataTable.GetDataTable<DRBattleScene1>();
                    DRBattleScene1 data = dtPlayer.GetDataRow(index);
                    (GameEntry.Entity.GetEntity(index).Logic as Model).SetPos(new Vector3(data.X, data.Y, data.Z));
                    (GameEntry.Entity.GetEntity(index).Logic as Model).SetDirection(data.Type);
                    GameEntry.Entity.GetEntity(index).gameObject.SetActive(true);
                    if (index == 9)
                    {
                        break;
                    }
                }
            }
        }

        void Awake()
        {

        }
    }

}