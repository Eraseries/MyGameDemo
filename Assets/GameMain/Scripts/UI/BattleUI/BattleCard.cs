using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class BattleCard : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        Transform init_parent;
        Vector2 init_sizedelta;
        Transform temp_parent;
        RectTransform rectTransform;
        RaycastHit hit;
        public void UpdateCardInfo(RoleData roleData = null)
        {
            if(roleData == null)
            {
                return;
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            rectTransform = gameObject.GetComponent<RectTransform>();
            init_parent = transform.parent;
            temp_parent = transform.parent.parent;
            init_sizedelta = rectTransform.sizeDelta;
            transform.SetParent(temp_parent);

            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 0);
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
            rectTransform.pivot = new Vector2(0f,0f);
            rectTransform.sizeDelta = init_sizedelta;
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition = eventData.position - new Vector2(init_sizedelta.x/2,init_sizedelta.y/2);
            //射线检测怪物 TODO

        }


        public void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(init_parent);
            transform.SetSiblingIndex(GetIndex());

            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = init_sizedelta;
        }

        int GetIndex()
        {
            int index = 1;
            if(transform.name == "Slot_1")
            {
                index = 0;
            }
            else if(transform.name == "Slot_2")
            {
                index = 1;
            }
            else if (transform.name == "Slot_3")
            {
                index = 2;
            }
            else if (transform.name == "Slot_4")
            {
                index = 3;
            }
            else
            {
                index = 0;
            }

            return index;
        }

    }
}
