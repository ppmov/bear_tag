using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private int initial;
    [SerializeField]
    private Text ui;

    public UnityEvent OnPush;

    public int TimeLeft => initial - (int)Mathf.Floor(timeSpent);

    public bool IsRunning
    {
        get => _isRunning;

        private set
        {
            timeSpent = 0;

            if (ui != null)
                ui.gameObject.SetActive(value);

            if (!value)
            {
                UnityAction lastAction = endAction;
                endAction = null;
                lastAction?.Invoke();
            }

            _isRunning = value;
        }
    }

    private bool _isRunning;
    private float timeSpent = 0;
    private UnityAction endAction = null;

    public void Push(UnityAction action = null)
    {
        IsRunning = false;
        endAction = action;
        IsRunning = true;
        OnPush.Invoke();
    }

    private void Update()
    {
        if (!IsRunning)
            return;

        timeSpent += Time.deltaTime;

        if (ui != null)
            ui.text = TimeLeft.ToString();

        if (timeSpent >= initial)
            IsRunning = false;
    }
}