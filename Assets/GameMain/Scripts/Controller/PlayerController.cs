using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
namespace StarForce
{
    public class PlayerController : UGuiForm
    {
        private enum RoleState
        {
            Idle,
            Walk,
            Run,
            Fly,
            Dead,
            Rebirth,
            NormalAttack,
            SkillAttack
        }
        private RoleState pre_state;
        private RoleState current_state = RoleState.Idle;
        private AnimatorStateInfo animInfo;
        private bool moving = false;
        private bool attacking = false;
        public bool is_run = false;
        Animator animator;
        void Awake()
        {
            animator = transform.GetComponent<Animator>();
        }

        void Start()
        {
            StartCoroutine("PlayAnim");
        }

        void Update()
        {
            InputListen();
        }

        void OnEnable()
        {
            StartCoroutine("PlayAnim");
        }

        void OnDisable()
        {
            StopCoroutine("PlayAnim");
        }

        void InputListen()
        {
            animInfo = animator.GetCurrentAnimatorStateInfo(0);

            float horizontalmove = Input.GetAxis("Horizontal");//浮点型 -1~1;
            float facedirection = Input.GetAxisRaw("Horizontal");//整型 -1 0 1

            if(!attacking)
            {
                //人物移动
                if (horizontalmove != 0)
                {
                    moving = true;
                    if (is_run)
                    {
                        transform.Translate(-facedirection * Vector3.left * Time.deltaTime * 2, Space.World);
                        current_state = RoleState.Run;
                    }
                    else
                    {
                        transform.Translate(-facedirection * Vector3.left * Time.deltaTime, Space.World);
                        current_state = RoleState.Walk;
                    }
                }
                else
                {
                    if (moving)
                    {
                        moving = false;
                        current_state = RoleState.Idle;
                    }
                }
                //人物转身
                if (facedirection != 0)
                {
                    transform.localScale = new Vector3(-facedirection * 300, 300, -300);
                }
            }


            //普通攻击
            if(Input.GetKeyDown(KeyCode.J))
            {
                attacking = true;
                current_state = RoleState.NormalAttack;
            }
            if(animInfo.IsName("Attack") && animInfo.normalizedTime >= 1.0f)
            {
                attacking = false;
                current_state = RoleState.Idle;
            }

            //技能攻击
            if(Input.GetKeyDown(KeyCode.K))
            {
                attacking = true;
                current_state = RoleState.SkillAttack;
            }
            if (animInfo.IsName("Skill") && animInfo.normalizedTime >= 1.0f)
            {
                attacking = false;
                current_state = RoleState.Idle;
            }

            //死亡
            if(Input.GetKeyDown(KeyCode.L))
            {
                current_state = RoleState.Dead;
            }

            //复活
            if (Input.GetKeyDown(KeyCode.U))
            {
                current_state = RoleState.Rebirth;
            }

        }

        void ChangeAnimState(RoleState roleState)
        {
            switch (roleState)
            {
                case RoleState.Idle:
                    animator.Play("Idle");
                    break;
                case RoleState.Walk:
                    animator.Play("Walk");
                    break;
                case RoleState.Fly:

                    break;
                case RoleState.NormalAttack:
                    animator.Play("Attack");
                    break;
                case RoleState.SkillAttack:
                    animator.Play("Skill");
                    break;
                case RoleState.Run:
                    animator.Play("Run");
                    break;
                case RoleState.Dead:
                    animator.Play("Dead");
                    break;
                case RoleState.Rebirth:
                    animator.Play("Rebirth");
                    break;
                default:
                    break;
            }
            pre_state = roleState;
        }

        //播放对用动作
        IEnumerator PlayAnim()
        {
            while (true)
            {
                yield return 0;
                if (pre_state != current_state )
                {
                    ChangeAnimState(current_state);
                }
            }
        }

    }
}
