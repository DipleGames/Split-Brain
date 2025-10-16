using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MG03_HealthManager : SingleTon<MG03_HealthManager>
{
    [Header("기본설정")]
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float leftCurrentHealth;
    [SerializeField] private float rightCurrentHealth;
    [SerializeField] private float hpDecreasePerSecond;

    [Header("체력바")]
    [SerializeField] private Slider[] HealthBars;


    protected override void Awake()
    {
        base.Awake();
        leftCurrentHealth = maxHealth;
        rightCurrentHealth = maxHealth;
        hpDecreasePerSecond = 0.01f;
        UpdateUI();
    }

    float add = 0.001f;
    float time = 0f;
    void Update()
    {
        time += Time.deltaTime;
        if(time >= 3f)
        {
            hpDecreasePerSecond += add;
            time = 0f;
        }
    }

    void FixedUpdate()
    {
        leftCurrentHealth -= hpDecreasePerSecond;
        rightCurrentHealth -= hpDecreasePerSecond;
        UpdateUI();
    }


    private void UpdateUI()
    {
        HealthBars[0].value = (float)leftCurrentHealth / maxHealth;
        HealthBars[1].value = (float)rightCurrentHealth / maxHealth;
    }

    public void RecoverHealth(float amount, int panelIndex)
    {
        switch(panelIndex)
        {
            case 0:
                leftCurrentHealth = Mathf.Min(leftCurrentHealth + amount, maxHealth);
                break;
            case 1:
                rightCurrentHealth = Mathf.Min(rightCurrentHealth + amount, maxHealth);
                break;
        }
        UpdateUI();
    }
}

