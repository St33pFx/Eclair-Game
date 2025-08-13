using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltips : MonoBehaviour
{
    [SerializeField] private GameObject Tooltip_1;
    [SerializeField] private GameObject Tooltip_2;
    [SerializeField] private GameObject Tooltip_3;



    [SerializeField] private GameObject Tooltip_1_trigger;
    [SerializeField] private GameObject Tooltip_2_trigger;
    [SerializeField] private GameObject Tooltip_3_trigger;



    private void Update()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResultList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultList);

        if (raycastResultList.Count == 1)
        {
            Tooltip_1.SetActive(false);
            Tooltip_2.SetActive(false);
            Tooltip_3.SetActive(false);

        }

        for (int i = 0; i < raycastResultList.Count; i++)
        {
            if (raycastResultList[i].gameObject == Tooltip_1_trigger)
            {
                Tooltip_1.SetActive(true);
            }
            else if (raycastResultList[i].gameObject == Tooltip_2_trigger)
            {
                Tooltip_2.SetActive(true);
            }
            else if (raycastResultList[i].gameObject == Tooltip_3_trigger)
            {
                Tooltip_3.SetActive(true);
            }

        }
    }
}

