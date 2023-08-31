using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBarController : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerAttack attack;
    private Slider slider;
    void Start()
    {
        slider = GetComponentInChildren<Slider>();
        
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = 1 - (attack.phantomAttackCDLeft / attack.phantomAttackCD);
    }
}
