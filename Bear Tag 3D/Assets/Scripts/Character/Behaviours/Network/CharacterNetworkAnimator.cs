using Mirror;
using System.Collections.Generic;

public class CharacterNetworkAnimator : NetworkAnimator
{
    // animation bools and triggers
    public const string idle = "Idle";
    public const string sit = "Sit";
    public const string run = "Run Forward";
    public const string right = "WalkForward";
    public const string left = "WalkForward";
    public const string back = "WalkBackward";
    public const string dash = "Attack5";

    private List<string> parameters;
    private Character _character;

    private Character Character
    {
        get
        {
            if (_character == null)
            {
                _character = GetComponent<Character>();

                if (_character != null)
                    _character.Dash.OnUsing += Dash;
            }

            return _character;
        }
    }

    private void Start()
    {
        if (!isLocalPlayer)
            return;

        parameters = new List<string>()
        { idle, sit, run, right, left, back, dash };
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Character == null)
            return;

        // no animations when dash
        if (Character.Dash.IsHandling)
            return;

        // set bools in priority
        if (Character.MoveValue > 0)
            Animate(run);
        else
        if (Character.MoveValue < 0)
            Animate(back);
        else
        if (Character.RotateValue > 0)
            Animate(right);
        else
        if (Character.RotateValue < 0)
            Animate(left);
        else
        if (Character.IsSitting)
            Animate(sit);
        else
            Animate(idle);
    }

    private void Dash()
    {
        // uncheck all bools
        Animate(string.Empty);
        // network set trigger
        SetTrigger(dash);
    }

    private void Animate(string name)
    {
        foreach (string param in parameters)
            animator.SetBool(param, param == name);
    }
}
