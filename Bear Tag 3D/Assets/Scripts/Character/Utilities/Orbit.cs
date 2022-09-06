using UnityEngine;

public class Orbit
{
    public enum Direction { forward, up, right }

    private readonly Transform transform;
    private readonly Direction axis;
    private readonly bool isAxisStatic;

    public Settings.Sensivity Options { get; private set; }
    public float Angle { get; private set; }

    public Vector3 Axis => axis switch
    {
        Direction.forward => isAxisStatic ? Vector3.forward : transform.forward,
        Direction.right => isAxisStatic ? Vector3.right : transform.right,
        Direction.up => isAxisStatic ? Vector3.up : transform.up,
        _ => Vector3.zero,
    };

    /// <summary>
    /// Orbit will rotate transform around axis
    /// </summary>
    /// <param name="isAxisStatic">When true, forward axis = Vector3.forward, when false = transform.forward</param>
    public Orbit(Settings.Sensivity options, Transform transform, Direction axis, bool isAxisStatic)
    {
        this.transform = transform;
        this.Options = options;
        this.axis = axis;
        this.isAxisStatic = isAxisStatic;
    }

    public void Turn(float angle)
    {
        angle *= Options.speed;

        if (Angle + angle > Options.Max)
            angle = Options.Max - Angle;
        else
        if (Angle + angle < Options.Min)
            angle = Options.Min - Angle;

        transform.RotateAround(transform.parent.transform.position, Axis, angle);
        Angle += angle;

        if (Mathf.Abs(Angle) >= 180)
            Angle -= Mathf.Sign(Angle) * 360;
    }
}
