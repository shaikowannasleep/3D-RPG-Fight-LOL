using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Threading.Tasks;
using System;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;

    public List<Ability> abilities = new List<Ability>();
    public List<Image> abilityImages = new List<Image>();
    public List<Text> abilityTexts = new List<Text>();
    public List<KeyCode> abilityKeys = new List<KeyCode>();
    public List<float> abilityCooldowns = new List<float>();
    public List<Button> abilityButtonKeys = new List<Button>();
    public List<Canvas> abilityCanvases = new List<Canvas>();

    private List<bool> isAbilityCooldown = new List<bool>();
    private List<float> currentCooldowns = new List<float>();
  
    void Start()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            abilityImages[i].fillAmount = 0;
            abilityTexts[i].text = "";
            isAbilityCooldown.Add(false);
            currentCooldowns.Add(0f);
            abilityCanvases[i].enabled = false;
            int index = i;
            abilityButtonKeys[i].onClick.AddListener(() => ActivateSkillInGame(index));
            SetupAbilityInput(i);
        }
    }

    private void SetupAbilityInput(int index)
    {
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(abilityKeys[index]) && !isAbilityCooldown[index])
            .Subscribe(_ =>
            {
                if (GetUseIndicator(index))
                {
                    IndicatorController indicatorController = GetComponent<IndicatorController>();
                    if (indicatorController != null)
                    {
                        indicatorController.RangedCanvas(); 
                    }
                }
                else
                {
                    ActivateSkillInGame(index);
                }
                HandleCooldownUI(index);
            }).AddTo(this);
    }

    public bool GetUseIndicator(int index)
    {
        if (index >= 0 && index < abilities.Count)
        {
            return abilities[index].useIndicator; 
        }
        return false; 
    }


    private void ActivateSkillInGame(int index)
    {
        abilities[index].Activate(gameObject);
        m_Animator.SetTrigger($"Skill{index + 1}");
    }

    private async void HandleCooldownUI(int index)
    {
        isAbilityCooldown[index] = true;
        currentCooldowns[index] = abilityCooldowns[index];
        float cooldownDuration = abilityCooldowns[index];

        while (currentCooldowns[index] > 0)
        {
            await Task.Delay(100);
            currentCooldowns[index] -= 0.1f;

            abilityImages[index].fillAmount = currentCooldowns[index] / cooldownDuration;
            abilityTexts[index].text = Mathf.Ceil(currentCooldowns[index]).ToString();
        }

        isAbilityCooldown[index] = false;
        abilityImages[index].fillAmount = 0;
        abilityTexts[index].text = "";
    }
    public bool GetIsAbilityCooldown(int index)
    {
        if (index >= 0 && index < isAbilityCooldown.Count)
        {
            return isAbilityCooldown[index];
        }
        return true;
    } 


}
