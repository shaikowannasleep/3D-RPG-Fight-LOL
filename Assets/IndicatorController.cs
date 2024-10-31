using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AbilityManager))]
public class IndicatorController : MonoBehaviour
{

    public enum SkillType
    {
        RANGED,
        CONE,
        HEAL,
        SKILLSHOT
    }


    [SerializeField]
    private AbilityManager abilityManager;
    public List<Canvas> abilityRangeCanvas = new List<Canvas>();
    public List<Image> abilityRangeCanvasImage = new List<Image>();
  
    public Canvas abiltyRangedCanvas;
    public Image abiltyRangedCanvasImage;


    private Vector3 postion;
    private RaycastHit hit;
    private Ray ray;
    public Vector3 castLocation;

    void Start()
    {
        DisableAllIndicators();
    }

    public void DisableAllIndicators()
    {
        for (int i = 0; i < abilityRangeCanvas.Count; i++)
        {
            abilityRangeCanvas[i].enabled = false;
            abilityRangeCanvasImage[i].enabled = false;
        }
        abiltyRangedCanvas.enabled = false;
        abiltyRangedCanvasImage.enabled = false;
    }

    public void RangedCanvas()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = LayerMask.GetMask("Environment");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.gameObject != this.gameObject)
            {
                postion = hit.point;
            }
        }
        var hitPosDir = (hit.point - transform.position).normalized;
        float distance = Vector3.Distance(hit.point, transform.position);
        distance = Mathf.Min(distance, 6f);

        var newHitPos = transform.position + hitPosDir * distance;
        abiltyRangedCanvas.transform.position = newHitPos;
        castLocation = newHitPos;
        //Debug.Log(castLocation);
    }


    private void Ability2Input()
    {
        if (Input.GetKeyDown(KeyCode.W) && !abilityManager.GetIsAbilityCooldown(1))
        {
            if (abilityManager.GetUseIndicator(1))
            {
                abiltyRangedCanvas.enabled = true;
                abiltyRangedCanvasImage.enabled = true;
                Cursor.visible = false;
            }
            else
            {
                abilityManager.abilities[1].Activate(gameObject);
            }
        }

        if (abiltyRangedCanvasImage.enabled && Input.GetMouseButtonDown(0))
        {
            abiltyRangedCanvas.enabled = false;
            abiltyRangedCanvasImage.enabled = false;
            Cursor.visible = true;

            abilityManager.abilities[1].Activate(gameObject);
        }
    }
    private void Update()
    {
        Ability2Input();
        RangedCanvas();
    }
    public Vector3 GetCurrentCastLocation()
    {
        return castLocation; 
    }
}
