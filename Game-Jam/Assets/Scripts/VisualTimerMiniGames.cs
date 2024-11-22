using UnityEngine;
using UnityEngine.UI;

public class VisualTimerMiniGames : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float totalGameTime = 15f; // Temps total en secondes
    private float timeRemaining;

    [Header("UI Elements")]
    [SerializeField] private Image[] timerIndicators; // Les 5 indicateurs (100%, 75%, ... 0%)
    private int currentIndicatorIndex = 0; // L'indicateur actuellement affiché

    void Start()
    {
        timeRemaining = totalGameTime;
        UpdateTimerIndicators(); // Initialisation du premier indicateur
    }

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            // Calcul du pourcentage restant
            float percentage = timeRemaining / totalGameTime;

            // Passer au prochain indicateur si nécessaire
            if (currentIndicatorIndex < timerIndicators.Length - 1)
            {
                float nextThreshold = 1f - (currentIndicatorIndex + 1) / (float)(timerIndicators.Length - 1);
                if (percentage <= nextThreshold)
                {
                    SwitchToNextIndicator();
                }
            }
        }
    }

    private void UpdateTimerIndicators()
    {
        // Activer uniquement l'indicateur courant
        for (int i = 0; i < timerIndicators.Length; i++)
        {
            timerIndicators[i].enabled = (i == currentIndicatorIndex);
        }
    }

    private void SwitchToNextIndicator()
    {
        // Désactiver l'indicateur actuel et activer le suivant
        timerIndicators[currentIndicatorIndex].enabled = false;
        currentIndicatorIndex++;
        timerIndicators[currentIndicatorIndex].enabled = true;
    }

    public float GetTimeRemaining()
    {
        return timeRemaining;
    }

    public void ResetTimer()
    {
        timeRemaining = totalGameTime;
        currentIndicatorIndex = 0;
        UpdateTimerIndicators();
    }
}
