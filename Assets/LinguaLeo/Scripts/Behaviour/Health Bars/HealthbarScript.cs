using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarScript : MonoBehaviour, Observer
{
    public int maxHealth = 100;
    [SerializeField]
    private Text PercentText = null;
    [SerializeField]
    Animator addbrainvalue = null;

    private Image healthbarFilling;
    private int health;

    // Use this for initialization
    void Start()
    {
        GameManager.Notifications.AddListener(this, GAME_EVENTS.CorrectAnswer);
        healthbarFilling = this.GetComponent<Image>();
        maxHealth = GameManager.WorkoutManager.GetBrainTasks();
        UpdateBrain();
    }

    private void UpdateBrain()
    {
        updateHealth(GameManager.WorkoutManager.GetCorrectAnswers());
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddHealth(1);
            addbrainvalue.Play("Add to Brain");
            Invoke("UpdateBrain", 2);
            return;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            RemoveHealth(1);
            return;
        }
    }

    public void AddHealth(int value)
    {
        health += value;
        if (health > maxHealth)
            health = maxHealth;
        updateHealth();
    }
    public bool RemoveHealth(int value)
    {
        health -= value;
        if (health <= 0)
        {
            health = 0;
            updateHealth();
            return true;
        }
        updateHealth();
        return false;
    }
    
    private void updateHealth()
    {
        var fillingAmount = 1f * health / maxHealth;
        healthbarFilling.fillAmount = fillingAmount;
    }
    private void updateHealth(int value)
    {
        health = value;
        float fillingAmount = 1f * health / maxHealth;
        healthbarFilling.fillAmount = fillingAmount;
        PercentText.text = (int)(fillingAmount * 100) + " %";
    }


    void Observer.OnNotify(object parametr, GAME_EVENTS notificationName)
    {
        switch (notificationName)
        {
            case GAME_EVENTS.CorrectAnswer:
                addbrainvalue.Play("addingBrainValue");
                Invoke("UpdateBrain", 1);
                break;
        }
    }
}
