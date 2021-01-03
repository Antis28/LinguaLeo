using UnityEngine;

namespace LinguaLeo.Scripts.Behaviour
{
    public class SymbolicLivesCounter : MonoBehaviour
    {
        #region Public variables

        public GameObject[] hearts;

        #endregion

        #region Private variables

        private int lives;

        #endregion

        #region Unity events

        // Use this for initialization
        private void Start()
        {
            hearts = new GameObject[transform.childCount];
            for (int i = 0; i < transform.childCount; i++) { hearts[i] = transform.GetChild(i).gameObject; }

            lives = hearts.Length;
        }

        #endregion

        #region Public Methods

        public bool AddLife(int amount = 1)
        {
            if (lives < hearts.Length)
            {
                lives++;
                UpdateLives();
                return true;
            }

            return false;
        }

        public bool LoseLife(int amount = 1)
        {
            lives--;
            if (lives > 0)
            {
                UpdateLives();
                return false;
            }

            lives = 0;
            UpdateLives();
            return true;
        }

        #endregion

        #region Private Methods

        // Update is called once per frame
        private void Update() { }

        private void UpdateLives()
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                if (i < lives)
                    hearts[i].SetActive(true);
                else
                    hearts[i].SetActive(false);
            }
        }

        #endregion
    }
}
