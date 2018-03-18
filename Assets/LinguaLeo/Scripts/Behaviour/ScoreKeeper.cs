using UnityEngine;

public class ScoreKeeper : MonoBehaviour, Observer {
    [SerializeField]
    private int score;
    private readonly int currentScore = 1;

    public  int ScoreValue
    {
        get
        {
            return score;
        }

        set
        {
            score = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        GameManager.Notifications.AddListener(this, GAME_EVENTS.CorrectAnswer);
        ResetScore();
    }

    public void AddScore(int points)
    {
        ScoreValue += points;
    }

    public void ResetScore()
    {
        ScoreValue = 0;
    }

    public void OnNotify(UnityEngine.Object parametr, GAME_EVENTS notificationName)
    {
        switch (notificationName)
        {
            case GAME_EVENTS.CorrectAnswer:
                AddScore(currentScore);
                print("Верный ответ");
                break;
        }
    }
}
