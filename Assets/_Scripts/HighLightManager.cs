using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HighLightManager : MonoBehaviour
{
    private Transform highlightedObj;
    private Transform selectedObj;
    public LayerMask seletableLayer;

    private Outline highlightOutline;
    private RaycastHit hit;

    [SerializeField] private Camera cameraMain;

    private void Update()
    {
        HoverHighlight();
    }
    public void HoverHighlight()
    {
        if (highlightedObj != null)
        {
            highlightOutline.enabled = false;
            highlightedObj = null;
        }

        Ray ray = cameraMain.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out hit, seletableLayer))
        {
            highlightedObj = hit.transform;

            if (highlightedObj.CompareTag("Enemy") && highlightedObj != selectedObj)
            {
                highlightOutline = highlightedObj.GetComponent<Outline>();
                highlightOutline.enabled = true;
            }
            else
            {
                highlightedObj = null;
            }
        }
    }
    public void SelectedHighlight()
    {
        if (highlightedObj == null) return;
        if (highlightedObj.CompareTag("Enemy"))
        {
            if (selectedObj != null)
            {
                selectedObj.GetComponent<Outline>().enabled = false;

            }
            selectedObj = hit.transform;
            selectedObj.GetComponent<Outline>().enabled = true;
            highlightOutline.enabled = true;
            highlightedObj = null;
        }
    }
    public void DeselectHighlight()
    {
        selectedObj.GetComponent<Outline>().enabled = false;
        selectedObj = null;
    }
}
