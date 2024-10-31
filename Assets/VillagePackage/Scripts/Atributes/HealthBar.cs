using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] CharacterHealth health = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas rootCanvas = null;
   
        void Update()
        {
            if (Mathf.Approximately(health.GetPercentage(), 0)
                || Mathf.Approximately(health.GetPercentage(), 1))
            {
                rootCanvas.enabled= false;
                return;
            }
            rootCanvas.enabled = true;
            foreground.localScale = new Vector3(health.GetPercentage(), 1.0f, 1.0f);
        }
    }
}