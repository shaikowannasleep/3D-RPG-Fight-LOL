using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : ScriptableObject
{
    public Sprite abilityImage;
    public float abilityCooldown;
    public Text abilityTextCoolDown;
    public KeyCode activeKey;
    public string CurrentAnimname;
    public float maxSkillDistance = 10f;
    public bool useIndicator;
    public virtual void Activate(GameObject parent)
    {
      // Debug.Log("Actived 0");
    } 
}
