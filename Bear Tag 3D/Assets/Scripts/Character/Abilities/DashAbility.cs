using UnityEngine;

public class DashAbility : Ability
{
    public DashAbility(MonoBehaviour parent, Settings.Ability setup) : base(parent, setup) { }

    protected override void Update()
    {
        Parent.transform.Translate(Setup.distance / Setup.duration * Time.deltaTime * Vector3.forward);
    }
}
