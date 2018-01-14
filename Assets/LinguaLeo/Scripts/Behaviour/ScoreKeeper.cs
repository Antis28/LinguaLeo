using UnityEngine;

public class ScoreKeeper : MonoBehaviour, Observer {

    private static int score;
    private readonly int currentScore = 1;

    public static int ScoreValue
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
    void Start () {
        GameManager.Notifications.AddListener(this, GAME_EVENTS.CorrectAnswer);
        Reset();
    }

    public void AddScore(int points )
    {
        ScoreValue += points;
    }  

    public static void Reset()
    {
        ScoreValue = 0;
    }

    public void OnNotify(Component sender, GAME_EVENTS notificationName)
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
