using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EndGameWindow : MonoBehaviour
{
    public UnityEvent atOpening;
    public UnityEvent atClosing;

    [SerializeField]
    private Text winText;
    [SerializeField]
    private Timer timer;

    private Player winner = null;

    private bool IsDrawn
    {
        get => _isDrawn;
        set
        {
            if (value && !IsDrawn)
            {
                timer.Push(Game.Restart);
                atOpening?.Invoke();
            }
            else
            if (!value && IsDrawn)
                atClosing?.Invoke();

            _isDrawn = value;

            if (Game.HasWinner)
                winText.text = winner.name + " is the winner!";
        }
    }
    private bool _isDrawn;

    private void FixedUpdate()
    {
        winner = Game.GetWinner();

        if (IsDrawn && !Game.HasWinner)
            IsDrawn = false;

        if (!IsDrawn && Game.HasWinner)
            IsDrawn = true;
    }
}
