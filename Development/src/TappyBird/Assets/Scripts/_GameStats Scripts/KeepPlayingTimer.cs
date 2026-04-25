using System.Collections;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class KeepPlayingTimer : MonoBehaviour
{
    [SerializeField] private Image _timerCircle;
    [SerializeField] private TextMeshProUGUI _timerText;

    private int _timerValue;
    private float _timerCircleFillAmount;

    private void OnEnable()
    {
        _timerValue = GameControl.GameControlInstance.KeepPlayingTimerAmount;
        _timerCircleFillAmount = (float)_timerValue;
        _timerText.text = $"{_timerValue}";

        StartTimer();
    }

    private void StartTimer()
    {
        StartCoroutine(TimerCountdown());
    }

    private void StopTimer()
    {
        StopCoroutine(TimerCountdown());
        gameObject.SetActive(false);
    }

    private IEnumerator TimerCountdown()
    {
        while (_timerCircleFillAmount > 0)
        {
            _timerCircleFillAmount -= Time.deltaTime;
            _timerCircle.fillAmount = _timerCircleFillAmount / GameControl.GameControlInstance.KeepPlayingTimerAmount;

            if (_timerValue > (int)_timerCircleFillAmount)
            {
                _timerText.text = $"{_timerValue}";
                _timerValue--;
            }

            yield return null;
        }

        StopTimer();
    }
}
