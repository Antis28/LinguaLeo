﻿using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Manegers;
using UnityEngine;
using UnityEngine.UI;

namespace LinguaLeo.Scripts.Behaviour.Health_Bars
{
    public class HealthbarScript : MonoBehaviour, IObserver
    {
        public int maxHealth = 100;
        [SerializeField]
        private Text PercentText = null;
        [SerializeField]
        Animator addbrainvalue = null;

        private Image healthbarFilling;
        private float health;

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
            //var correctAnswers = GameManager.WorkoutManager.GetCorrectAnswers();
            var correctAnswers = GameManager.ScoreKeeper.ScoreValue;
            updateHealth(correctAnswers);
        }

        public void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Q))
            //{
            //    AddHealth(1);
            //    addbrainvalue.Play("Add to Brain");
            //    Invoke("UpdateBrain", 2);
            //    return;
            //}
            //if (Input.GetKeyDown(KeyCode.W))
            //{
            //    RemoveHealth(1);
            //    return;
            //}
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
            var fillingAmount = health / maxHealth;
            healthbarFilling.fillAmount = fillingAmount;
        }
        private void updateHealth(float value)
        {
            health = value;
            float fillingAmount = health / maxHealth;
            healthbarFilling.fillAmount = fillingAmount;
            PercentText.text = (int)(fillingAmount * 100) + " %";
        }


        void IObserver.OnNotify(object parametr, GAME_EVENTS notificationName)
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
}
