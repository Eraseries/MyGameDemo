using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using System.Reflection;
using System.Threading;
/// <summary>
/// 战斗面板
/// </summary>
namespace StarForce
{
    enum Status
    {
        RoundStart,
        Fighting,
        RoundEnd,
        None
    }

    public class BattlePanel
    {
        //存储生成的所有模型(参数一：生成模型的唯一SerialId，参数二：场景数据)
        Dictionary<int, DRBattleScene1> model = new Dictionary<int, DRBattleScene1>();
        private static int current_stage = 1; //当前关卡
        public static BattlePanel instance;
        public static BattlePanel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BattlePanel();
                }
                return instance;
            }
        }
        Status status = Status.None;

        IDataTable<DRStage1> dtStage1;
        DRStage1 stage1Data;

        IDataTable<DRBattleScene1> dtBattleScene1;
        DRBattleScene1 battleScene1Data;

        BattleUI battleUI;
        string[] model_id_s;
        string[] model_pos_s;
        public void Init()
        {
            current_stage = GameEntry.PlayerData.GetPlayerData().curStage;
            GameEntry.PlayerData.GetPlayerData().curStage++;
            dtStage1 = GameEntry.DataTable.GetDataTable<DRStage1>();
            dtBattleScene1 = GameEntry.DataTable.GetDataTable<DRBattleScene1>();

            stage1Data = dtStage1.GetDataRow(current_stage);

            InitPlayer();
            InitEnemy();
            status = Status.None;
        }

        //初始化玩家
        public void InitPlayer()
        {
            //获取到自己设置的出战的角色
            if(GameEntry.PlayerData.GetPlayerData().RoleBag.Count > 0)
            {
                foreach (var item in GameEntry.PlayerData.GetPlayerData().RoleBag)
                {
                    if(item.Value.battle_pos != -1)
                    {
                        battleScene1Data = dtBattleScene1.GetDataRow(item.Value.battle_pos);
                        GameEntry.Entity.ShowModel(new ModelData(GameEntry.Entity.GenerateSerialId(), item.Key));
                        model.Add(GameEntry.Entity.GetSerialId(), battleScene1Data);
                    }
                }
            }
        }

        //初始化敌人
        public void InitEnemy()
        {

            model_id_s = new string[stage1Data.EnemyCount];
            model_pos_s = new string[stage1Data.EnemyCount];

            int count = stage1Data.EnemyCount;
            if (count > 1)
            {
                model_id_s = stage1Data.EnemyModel.Split('、');
                model_pos_s = stage1Data.EnemyPos.Split('、');
            }
            else
            {
                model_id_s[0] = stage1Data.EnemyModel;
                model_pos_s[0] = stage1Data.EnemyPos;
            }


            for (int j = 1; j <= stage1Data.EnemyCount; j++)
            {
                int id = j - 1;

                //加载模型（先从池子里判断有没有该模型，有的话直接取，没有再生成）

                int model_index = int.Parse(model_id_s[id]);
                GameEntry.Entity.ShowModel(new ModelData(GameEntry.Entity.GenerateSerialId(), model_index));
                battleScene1Data = dtBattleScene1.GetDataRow(int.Parse(model_pos_s[id]));
                model.Add(GameEntry.Entity.GetSerialId(), battleScene1Data);
            }
        }

        public void Show()
        {
            status = Status.None;
            if (battleUI == null)
            {
                battleUI = (BattleUI)GameEntry.UI.GetUIForm(UIFormId.BattleUI).UIForm.Logic;
            }

            ShowAllModel();

        }


        private void ShowAllModel()
        {
            foreach (var item in model)
            {
                (GameEntry.Entity.GetEntity(item.Key).Logic as Model).SetPos(new Vector3(item.Value.X, item.Value.Y, item.Value.Z));
                (GameEntry.Entity.GetEntity(item.Key).Logic as Model).SetDirection(item.Value.Type);
                GameEntry.Entity.GetEntity(item.Key).gameObject.SetActive(true);
            }

            Timer timer_1 = Timer.Register(0.1f, () => {
                status = Status.RoundStart;
            }, null, false, true);
        }

        public void TestOperate(int index)
        {
            Attack(index);
        }

        public void Update()
        {
            //回合开始（场上所有玩家和怪物选卡逻辑）

            if(status == Status.RoundStart)
            {
                battleUI.RoundStart();
                status = Status.None;
            }
            else if(status == Status.Fighting)
            {

            }
            else if (status == Status.RoundEnd)
            {

            }
            else if (status == Status.None)
            {

            }
            //具体逻辑

            //回合结束
        }

        public void Attack(int index)
        {
            foreach (var item in model)
            {
                (GameEntry.Entity.GetEntity(item.Key).Logic as Model).Fly(new Vector3(model[index].X, model[index].Y, model[index].Z));
                break;
            }
        }

        public void Close()
        {
            model.Clear();
        }

        public void Reset()
        {
            GameEntry.Entity.HideAllLoadedEntities();
            GameEntry.UI.OpenUIForm(UIFormId.BattleStartUI);
        }
    }

}