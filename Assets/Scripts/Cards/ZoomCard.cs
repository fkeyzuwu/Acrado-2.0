using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;

public class ZoomCard : NetworkBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject zoomCard;
    [SerializeField] private CardData cardData;

    private float timer = 0;
    private float timeTillZoom = 0.15f;
    private bool isHovering = false;
    private bool isZooming = false;

    void Update()
    {
        if (isHovering && !isZooming)
        {
            timer += Time.deltaTime;

            if (timer >= timeTillZoom)
            {

                Zoom();
                isZooming = true;
                timer = 0;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!hasAuthority && cardData.state != CardState.Board) return;
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!hasAuthority && cardData.state != CardState.Board) return;

        if (isHovering || isZooming)
        {
            UnZoom();
        }
    }

    private void Zoom()
    {
        zoomCard.SetActive(true);
    }

    private void UnZoom()
    {
        zoomCard.SetActive(false);
        isHovering = false;
        isZooming = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!hasAuthority && cardData.state != CardState.Board) return;

        UnZoom();
    }
}
