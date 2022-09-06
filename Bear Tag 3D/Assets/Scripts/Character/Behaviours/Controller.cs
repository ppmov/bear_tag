using UnityEngine;

public class Controller : MonoBehaviour
{
    private Character character;

    private void Start() => character = GetComponentInParent<Character>();

    private void Update()
    {
        if (character == null)
            return;

        character.MoveValue = Input.GetAxis("Vertical");
        character.RotateValue = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
            character.IsSitting = true;

        if (Input.GetKeyUp(KeyCode.Mouse0))
            character.Dash.TryUse();
    }

    private void LateUpdate()
    {
        if (character == null)
            return;

        character.CameraTilt.Turn(Input.GetAxis("Mouse Y"));
        character.CameraRotation.Turn(Input.GetAxis("Mouse X"));
    }
}
