//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class Model : Aircraft
    {
        [SerializeField]
        private ModelData m_ModelData = null;

        private Vector3 m_TargetPosition = Vector3.zero;
        private Animator animator;

        private Vector3 moving_direction = Vector3.one;
        private Vector3 back_direction = Vector3.one;
        private Vector3 target_pos = Vector3.one;
        private Vector3 orign_pos = Vector3.zero;
        private float timer = 0.1f;
        public int model_type;  //角色类型
        public GameObject select;
        public enum State
        {
            Dead,
            Rebirth,
            MovingTarget,
            MovingOrign,
            Attack,
            Skill,
            Idle,
            OperateFinsh,
            PlayEnd,
            None
        }

        public State cur_state = State.Idle;

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);
            Name = "Role_" + Id;
            transform.localScale = new Vector3(2.5f, 2.5f, -2.5f);
            select = transform.Find("select").gameObject;
            select.SetActive(false);
            animator = transform.GetComponent<Animator>();
            animator.SetFloat("Offset", Random.Range(0.0f, 1.0f));
            animator.SetFloat("Speed", Random.Range(0.6f, 1.5f));
            gameObject.SetActive(false);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnShow(object userData)
#else
        protected internal override void OnShow(object userData)
#endif
        {
            gameObject.SetActive(false);
            m_ModelData = userData as ModelData;
            if (m_ModelData == null)
            {
                Log.Error("My aircraft data is invalid.");
                return;
            }

            if (m_ModelData.model_type == CampType.Player)
            {
                transform.localScale = new Vector3(-2.5f, 2.5f, -2.5f);
            }
            else
            {
                transform.localScale = new Vector3(2.5f, 2.5f, -2.5f);
            }

            Name = "Role_" + Id;
            gameObject.SetLayerRecursively(Constant.Layer.UIModelLayerId);
            animator.SetFloat("Offset", Random.Range(0.0f, 1.0f));
            animator.SetFloat("Speed", Random.Range(0.6f, 1.5f));
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#else
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#endif
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if(cur_state == State.Idle)
            {
                if(!animator.isActiveAndEnabled)
                {
                    return;
                }
                animator.updateMode = AnimatorUpdateMode.Normal;
                animator.Play("Idle");
                cur_state = State.None;
            }
            else if(cur_state == State.MovingTarget)
            {
                animator.Play("Run");
                transform.Translate((-8f - GameEntry.Base.GameSpeed * 2f) * moving_direction * Time.unscaledDeltaTime, Space.World);
                if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(target_pos.x, target_pos.y)) <= 0.65f)
                {
                    transform.position = new Vector3(target_pos.x, target_pos.y, 0);
                    //animator.updateMode = AnimatorUpdateMode.UnscaledTime;
                    cur_state = State.Attack;

                }
            }
            else if (cur_state == State.MovingOrign)
            {
                animator.Play("Run");
                transform.Translate((-8f - GameEntry.Base.GameSpeed * 2f) * back_direction * Time.unscaledDeltaTime, Space.World);
                if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(orign_pos.x, orign_pos.y)) <= 0.65f)
                {
                    transform.position = new Vector3(orign_pos.x, orign_pos.y, 0);
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    cur_state = State.OperateFinsh;
                }
            }
            else if (cur_state == State.Attack)
            {
                animator.Play("Attack");
                timer -= Time.unscaledDeltaTime;
                if(timer <= 0)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime * (1 /(Mathf.Clamp(GameEntry.Base.GameSpeed - 0.9f,1,1.2f))) >= 1)
                    {
                        cur_state = State.MovingOrign;
                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                        timer = 0.1f;
                    }
                }
            }
            else if (cur_state == State.Dead)
            {
                animator.Play("Dead");
                cur_state = State.None;
            }
            else if (cur_state == State.Skill)
            {
                animator.Play("Skill");
                cur_state = State.None;
            }
            else if (cur_state == State.Rebirth)
            {
                animator.Play("Rebirth");
                cur_state = State.None;
            }
            else if (cur_state == State.OperateFinsh)
            {
                cur_state = State.Idle;
            }
            else if (cur_state == State.None)
            {
                
            }
            else if (cur_state == State.PlayEnd)
            {

            }
            else
            {

            }
        }

        public ModelData GetModelData()
        {
            return m_ModelData;
        }


        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }
        public void SetPos(Vector3 pos)
        {
            transform.position = pos;
            orign_pos = pos;
        }
        public void Fly(Vector3 pos)
        {
            Vector3 end_pos = new Vector3(pos.x - 1.5f , pos.y, pos.z);
            cur_state = State.MovingTarget;

            moving_direction = transform.position - end_pos;
            back_direction = end_pos - transform.position;
            //单位化（长度为1的向量）
            back_direction = back_direction.normalized;
            moving_direction = moving_direction.normalized;
            transform.position = new Vector3(transform.position.x, transform.position.y, target_pos.z);
            target_pos = end_pos;
        }

        public void SetSelect(bool bo)
        {
            select.SetActive(bo);
        }

        void OnEnable()
        {
            if (animator)
            {
                animator.SetFloat("Offset", Random.Range(0.0f, 1.0f));
                animator.SetFloat("Speed", Random.Range(0.6f, 1.5f));
            }
        }

    }
}
