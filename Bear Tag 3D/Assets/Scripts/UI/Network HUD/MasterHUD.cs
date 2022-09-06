using UnityEngine;

public class MasterHUD : MonoBehaviour
{
    [SerializeField]
    private Behaviour nameHud;
    [SerializeField]
    private Behaviour netHud;

    private void Start()
    {
        nameHud.enabled = true;
        netHud.enabled = false;
    }

    private void Update()
    {
        if (Game.IsStarted)
            if (Input.GetKeyDown(KeyCode.Escape))
                netHud.enabled = nameHud.enabled = !nameHud.enabled;

        if (!Game.IsStarted)
            netHud.enabled = Game.Nickname != string.Empty;

        Cursor.visible = nameHud.enabled;
    }
}
