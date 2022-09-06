using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    private void OnGUI()
    {
        DrawName();
        DrawExit();
    }

    private void DrawName()
    {
        GUILayout.BeginArea(new Rect((Screen.width / 2) - 150, 10, 300, 300));
        GUILayout.BeginHorizontal();
        GUILayout.Label("Name:", GUILayout.Width(40));

        if (Game.IsStarted)
            GUILayout.Label(Game.Nickname, "TextField");
        else
            Game.Nickname = GUILayout.TextField(Game.Nickname);

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void DrawExit()
    {
        GUILayout.BeginArea(new Rect(310, 10, 50, 100));

        if (!Game.IsStarted)
            if (GUILayout.Button("Exit"))
                Application.Quit();

        GUILayout.EndArea();
    }
}
