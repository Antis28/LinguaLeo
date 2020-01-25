using UnityEngine;

public class ScoreKeeper : MonoBehaviour, IObserver {
    [SerializeField]
    private float score;
    private float priceScore = 1;

    /// <summary>
    /// Выставляет цену за выученое слово в тренировке.
    /// </summary>
    public void SetPriceScore(float price)
    {
        priceScore = price;
    }

    public float ScoreValue
    {
        get
        {
            return score;
        }
    }

    // Use this for initialization
    void Start()
    {
        GameManager.Notifications.AddListener(this, GAME_EVENTS.CorrectAnswer);
        ResetScore();
    }

    public void AddScore(float points)
    {
        score += points;
    }

    public void ResetScore()
    {
        score = 0;
    }

    public void OnNotify(object parametr, GAME_EVENTS notificationName)
    {
        switch (notificationName)
        {
            case GAME_EVENTS.CorrectAnswer:
                AddScore(priceScore);
                print("Верный ответ");
                break;
        }
    }
}
