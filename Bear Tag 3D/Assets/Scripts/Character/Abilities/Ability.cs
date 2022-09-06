using System;
using System.Collections;
using UnityEngine;

public abstract class Ability
{
    public MonoBehaviour Parent { get; private set; }
    public Settings.Ability Setup { get; private set; }

    public event Action OnUsing;
    public event Action OnRelease;

    public bool IsCooling { get; protected set; } = false;
    public bool IsHandling { get; protected set; } = false;

    protected readonly Settings.Ability setup;

    public Ability(MonoBehaviour parent, Settings.Ability setup)
    {
        Parent = parent;
        Setup = setup;
    }

    public bool TryUse()
    {
        if (IsCooling)
            return false;

        Parent.StartCoroutine(Handling());
        Parent.StartCoroutine(Cooldown());
        return true;
    }

    protected abstract void Update();

    private IEnumerator Handling()
    {
        IsHandling = true;
        OnUsing?.Invoke();
        float spent = 0;

        while (spent < Setup.duration)
        {
            yield return new WaitForEndOfFrame();
            spent += Time.deltaTime;
            Update();
        }

        IsHandling = false;
        OnRelease?.Invoke();
    }

    private IEnumerator Cooldown()
    {
        IsCooling = true;
        yield return new WaitForSeconds(Setup.cooldown);
        IsCooling = false;
    }
}