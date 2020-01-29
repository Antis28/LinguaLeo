using UnityEngine;

namespace LinguaLeo.Scripts.Behaviour
{
    public class SymbolicLivesCounter : MonoBehaviour {

        public GameObject[] hearts;
        private int lives;

        // Use this for initialization
        void Start () {
            hearts = new GameObject[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                hearts[i] = transform.GetChild(i).gameObject;
            }

            lives = hearts.Length;
        }
	
        // Update is called once per frame
        void Update () {
		
        }

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
    }
}
