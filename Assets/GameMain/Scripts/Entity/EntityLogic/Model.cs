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

        private Rect m_PlayerMoveBoundary = default(Rect);
        private Vector3 m_TargetPosition = Vector3.zero;
        private Animator animator;

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);
            Name = "Role_" + Id;
            transform.localScale = new Vector3(2.5f, 2.5f, -2.5f);
            animator = transform.GetComponent<Animator>();
            animator.Play("Idle");
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
            base.OnShow(userData);
            gameObject.SetActive(false);
            m_ModelData = userData as ModelData;
            if (m_ModelData == null)
            {
                Log.Error("My aircraft data is invalid.");
                return;
            }
            Name = "Role_" + Id;
            transform.localScale = new Vector3(2.5f, 2.5f, -2.5f);
            gameObject.SetLayerRecursively(Constant.Layer.UIModelLayerId);
            animator.SetFloat("Offset", Random.Range(0.0f, 1.0f));
            animator.SetFloat("Speed", Random.Range(0.6f, 1.5f));
            //ScrollableBackground sceneBackground = FindObjectOfType<ScrollableBackground>();
            //if (sceneBackground == null)
            //{
            //    Log.Warning("Can not find scene background.");
            //    return;
            //}

            //m_PlayerMoveBoundary = new Rect(sceneBackground.PlayerMoveBoundary.bounds.min.x, sceneBackground.PlayerMoveBoundary.bounds.min.z,
            //    sceneBackground.PlayerMoveBoundary.bounds.size.x, sceneBackground.PlayerMoveBoundary.bounds.size.z);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#else
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#endif
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            //if (Input.GetMouseButton(0))
            //{
            //    Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //    m_TargetPosition = new Vector3(point.x, 0f, point.z);

            //    for (int i = 0; i < m_Weapons.Count; i++)
            //    {
            //        m_Weapons[i].TryAttack();
            //    }
            //}

            //Vector3 direction = m_TargetPosition - CachedTransform.localPosition;
            //if (direction.sqrMagnitude <= Vector3.kEpsilon)
            //{
            //    return;
            //}

            //Vector3 speed = Vector3.ClampMagnitude(direction.normalized * m_ModelData.Speed * elapseSeconds, direction.magnitude);
            //CachedTransform.localPosition = new Vector3
            //(
            //    Mathf.Clamp(CachedTransform.localPosition.x + speed.x, m_PlayerMoveBoundary.xMin, m_PlayerMoveBoundary.xMax),
            //    0f,
            //    Mathf.Clamp(CachedTransform.localPosition.z + speed.z, m_PlayerMoveBoundary.yMin, m_PlayerMoveBoundary.yMax)
            //);
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }
        public void SetPos(Vector3 pos)
        {
            transform.position = pos;
        }

        public void SetDirection(string type)
        {
            if (type == "Player")
            {
                transform.localScale = new Vector3(-2.5f, 2.5f, -2.5f);
            }
            else
            {
                transform.localScale = new Vector3(2.5f, 2.5f, -2.5f);
            }
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
