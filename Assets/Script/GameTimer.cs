using UnityEngine;
using UnityEngine.UI;


public class GameTimer : MonoBehaviour
{
    public float duration;
    public Text timerText;
    public static GameTimer instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de GameOverManager dans la scène");
            return;
        }

        instance = this;
    }
   
    public void StartTimer()
    {
        UpdateTimerDisplay();
        InvokeRepeating(nameof(UpdateTimer), 1f, 1f);
    }

    private void UpdateTimer()
    {
        duration -= 1f;
        if (duration <= 0f)
        {
            duration = 0f;
            CancelInvoke(nameof(UpdateTimer));
            GameOverManager.instance.GameOver();
        }
        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(duration / 60f);
        int seconds = Mathf.FloorToInt(duration % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }


}
