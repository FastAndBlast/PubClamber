using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopOutUIEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float hoverScale = 1.25f;

    public float scaleSpeed = 1f;

    Vector3 initialScale;

    bool moused;

    public bool on = true;


    public void Start()
    {
        initialScale = transform.localScale;
    }

    private void Update()
    {
        if (on)
        {
            if (moused)
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale, initialScale * hoverScale, Time.deltaTime * scaleSpeed);
            }
            else
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale, initialScale, Time.deltaTime * scaleSpeed);
            }
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        moused = true;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        moused = false;
    }


}
