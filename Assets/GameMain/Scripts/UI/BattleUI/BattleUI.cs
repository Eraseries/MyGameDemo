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
/// 主界面模块
/// </summary>
namespace StarForce
{
    public class BattleUI : UGuiForm
    {
        private Transform content;
        private Transform top;

        [HideInInspector]
        public Button backBtn;

        bool InitModel = false;


        Vector3[] Pos = new Vector3[10];

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            Name = "BattleUI";
            content = transform.Find("Background");
            top = content.Find("Top");
            backBtn = top.Find("BackBtn").GetComponent<Button>();
            AddBtnEvent(backBtn, () => {
                Close(true); 
            
            });// Close();

            for (int i = 1; i <= 9; i++)
            {
                int index = i;
                GameEntry.Entity.ShowModel(new ModelData(GameEntry.Entity.GenerateSerialId(), 100000 + index));
            }
        }

        void ListenInput()
        {
            if(Input.GetMouseButtonDown(0))
            {
                //Log.Error(Input.mousePosition);
            }
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

        }

        void SetPlayer()
        {
            for (int i = 1; i <= 11; i++)
            {
                int index = i;
                if (GameEntry.Entity.GetEntity(index))
                {
                    IDataTable<DRBattleScene1> dtPlayer = GameEntry.DataTable.GetDataTable<DRBattleScene1>();
                    DRBattleScene1 data = dtPlayer.GetDataRow(index);
                    (GameEntry.Entity.GetEntity(index).Logic as Model).SetPos(new Vector3(data.X,data.Y,data.Z));
                    (GameEntry.Entity.GetEntity(index).Logic as Model).SetDirection(data.Type);
                    if (index == 9)
                    {
                        InitModel = true;
                    }
                }

            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            (GameEntry.Procedure.CurrentProcedure as ProcedureBattle1).m_ExitBattle = true;
            base.OnClose(isShutdown, userData);
            for (int i = 1; i <= 9; i++)
            {
                int index = i;
                if (GameEntry.Entity.GetEntity(index))
                {
                    GameEntry.Entity.GetEntity(index).Hide();
                }

            }
        }

        protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if(!InitModel)
            {
                SetPlayer();
            }
        }

    }

}