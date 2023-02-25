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

        //按出战优先级存储的所有模型
        Dictionary<int, Model> battle_queue = new Dictionary<int, Model>();

        private int current_stage = 1; //当前关卡
        public bool is_turn = true;
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


        Model cur_turn_model; //轮到当前的角色模型

        BattleUI battleUI;
        string[] model_id_s;
        string[] model_pos_s;
        public void Init()
        {
            dtStage1 = GameEntry.DataTable.GetDataTable<DRStage1>();
            dtBattleScene1 = GameEntry.DataTable.GetDataTable<DRBattleScene1>();

            stage1Data = dtStage1.GetDataRow(1);

            InitPlayer();
            InitEnemy();
            status = Status.None;
        }

        //初始化玩家
        public void InitPlayer()
        {
            //获取到自己设置的出战的角色
            if (GameEntry.PlayerData.GetPlayerData().RoleBag.Count > 0)
            {
                foreach (var item in GameEntry.PlayerData.GetPlayerData().RoleBag)
                {
                    if (item.Value.battle_pos != -1)
                    {
                        battleScene1Data = dtBattleScene1.GetDataRow(item.Value.battle_pos);
                        GameEntry.Entity.ShowModel(new ModelData(GameEntry.Entity.GenerateSerialId(), item.Key, "Player", item.Value.level));
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
                GameEntry.Entity.ShowModel(new ModelData(GameEntry.Entity.GenerateSerialId(), model_index,"Enemy",5, current_stage));
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
            status = Status.RoundStart;
            foreach (var item in model)
            {
                Model model;
                model = GameEntry.Entity.GetEntity(item.Key).Logic as Model;
                model.SetPos(new Vector3(item.Value.X, item.Value.Y, item.Value.Z));
                model.gameObject.SetActive(true);
                
            }
        }

        public void SetStage(int value)
        {
            current_stage = value;
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
                battleUI.RoundStart(SortModel,UpdateQueue);
                status = Status.None;
            }
            else if(status == Status.Fighting)
            {
                if(is_turn)
                {
                    foreach (var item in battle_queue)
                    {
                        if (item.Value.cur_state != Model.State.PlayEnd)
                        {
                            battleUI.UpdateBottomCard(item.Value.GetModelData().roleData.index);
                            break;
                        }
                    }
                    is_turn = false;
                }
            }
            else if (status == Status.RoundEnd)
            {
                battleUI.RoundStart();
                status = Status.None;
            }
            else if (status == Status.None)
            {

            }
            //具体逻辑

            //回合结束
        }

        //处理所有模型的出战优先级
        private void SortModel()
        {
            Model temp_model;
            int temp_index = 0;
            if(battle_queue.Count == 0)
            {
                foreach (var item in model)
                {
                    battle_queue.Add(temp_index, GameEntry.Entity.GetEntity(item.Key).Logic as Model);
                    temp_index = temp_index + 1;
                }
                temp_index = 0;

                for (int i = 0; i < battle_queue.Count; i++)
                {
                    //找出优先级最小的那个
                    int priority_1 = battle_queue[i].GetModelData().roleData.priority;
                    int record_index = i;
                    for (int j = i + 1; j < battle_queue.Count; j++)
                    {
                        int priority_2 = battle_queue[j].GetModelData().roleData.priority;
                        temp_index++;
                        temp_index = (int)Mathf.Clamp(temp_index, 0, 1);
                        if (priority_2 <= priority_1 && temp_index == 1)
                        {
                            record_index = j;
                        }
                    }
                    temp_model = battle_queue[record_index];
                    battle_queue[record_index] = battle_queue[i];
                    battle_queue[i] = temp_model;
                }
                for (int i = 0; i < battle_queue.Count; i++)
                {
                    Log.Error(battle_queue[i].GetModelData().roleData.priority);
                }
            }
            else
            {
                //先处理没血的
                temp_index = 0;
                Dictionary<int, Model> temp_queue = new Dictionary<int, Model>();
                for (int i = 0; i < battle_queue.Count; i++)
                {
                    if(battle_queue[i].GetModelData().roleData.total_hp <= 0)
                    {
                        continue;
                    }
                    temp_queue.Add(temp_index, battle_queue[i]);
                    temp_index++;
                }
                battle_queue = temp_queue;

            }


            
            foreach (var item in model)
            {
                Model model_logic = GameEntry.Entity.GetEntity(item.Key).Logic as Model;
            }

        }

        //更新左上角出战队列ToDo
        private void UpdateQueue()
        {

            status = Status.Fighting;
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