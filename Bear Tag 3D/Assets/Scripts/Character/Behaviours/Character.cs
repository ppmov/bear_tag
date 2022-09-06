using UnityEngine;

public class Character : MonoBehaviour
{
    public Ability Dash { get; private set; }
    public Orbit CameraTilt { get; private set; }
    public Orbit CameraRotation { get; private set; }

    public float MoveValue { get; set; } = 0f;
    public float RotateValue { get; set; } = 0f;
    public bool IsSitting { get; set; } = false;

    private void Awake()
    {
        Camera cam = Camera.main;
        CameraTilt = new Orbit(Settings.VerticalCamera, cam.transform, Orbit.Direction.right, false);
        CameraRotation = new Orbit(Settings.HorizontalCamera, cam.transform, Orbit.Direction.up, true);
        Dash = new DashAbility(this, Settings.Dash);
    }

    private void Update()
    {
        UpdateRotation();
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        if (Dash.IsHandling)
            MoveValue = 0; // no move when dash
        else
            transform.Translate((MoveValue > 0 ? Settings.MoveSpeed : Settings.WalkSpeed) * Time.deltaTime * MoveValue * Vector3.forward);

        if (Mathf.Abs(MoveValue) > 0 || Dash.IsHandling)
            IsSitting = false;
    }

    private void UpdateRotation()
    {
        if (CameraRotation == null)
            return;

        if (Dash.IsHandling)
            CameraRotation.Turn(-CameraRotation.Angle * Time.deltaTime);

        // rotate transform at angle
        transform.Rotate(Vector3.up, RotateValue * Settings.RotateVelocity * Time.deltaTime);
        // rotate camera at inversed angle
        CameraRotation.Turn(-RotateValue * Settings.RotateVelocity / CameraRotation.Options.speed * Time.deltaTime);
    }
}
