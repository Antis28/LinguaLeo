using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Manegers;
using UnityEngine;
using UnityEngine.UI;

namespace LinguaLeo.Scripts.Behaviour.Health_Bars
{
    public class HealthbarScript : MonoBehaviour, IObserver
    {
        #region SerializeFields

        [SerializeField]
        private Text PercentText = null;

        [SerializeField]
        private Animator addbrainvalue = null;

        #endregion

        #region Public variables

        public int maxHealth = 100;

        #endregion

        #region Private variables

        private Image healthbarFilling;
        private float health;

        #endregion

        #region Events

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

        #endregion

        #region Unity events

        // Use this for initialization
        private void Start()
        {
            GameManager.Notifications.AddListener(this, GAME_EVENTS.CorrectAnswer);
            healthbarFilling = GetComponent<Image>();
            maxHealth = GameManager.WorkoutManager.GetBrainTasks();
            UpdateBrain();
        }

        #endregion

        #region Public Methods

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

        #endregion

        #region Private Methods

        private void UpdateBrain()
        {
            //var correctAnswers = GameManager.WorkoutManager.GetCorrectAnswers();
            var correctAnswers = GameManager.ScoreKeeper.ScoreValue;
            updateHealth(correctAnswers);
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
            PercentText.text = (int) (fillingAmount * 100) + " %";
        }

        #endregion
    }
}
