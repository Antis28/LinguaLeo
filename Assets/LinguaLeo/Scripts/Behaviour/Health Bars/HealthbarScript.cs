using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Managers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LinguaLeo.Scripts.Behaviour.Health_Bars
{
    public class HealthbarScript : MonoBehaviour, IObserver
    {
        #region SerializeFields

        [FormerlySerializedAs("PercentText")]
        [SerializeField]
        private Text percentText = null;

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

        void IObserver.OnNotify(object parametr, GameEvents notificationName)
        {
            switch (notificationName)
            {
                case GameEvents.CorrectAnswer:
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
            GameManager.Notifications.AddListener(this, GameEvents.CorrectAnswer);
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
            UpdateHealth();
        }

        public bool RemoveHealth(int value)
        {
            health -= value;
            if (health <= 0)
            {
                health = 0;
                UpdateHealth();
                return true;
            }

            UpdateHealth();
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
            UpdateHealth(correctAnswers);
        }

        private void UpdateHealth()
        {
            var fillingAmount = health / maxHealth;
            healthbarFilling.fillAmount = fillingAmount;
        }

        private void UpdateHealth(float value)
        {
            health = value;
            float fillingAmount = health / maxHealth;
            healthbarFilling.fillAmount = fillingAmount;
            percentText.text = (int) (fillingAmount * 100) + " %";
        }

        #endregion
    }
}
