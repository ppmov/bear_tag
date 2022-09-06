using UnityEngine;
using UnityEngine.UI;

public class PlayerRow : MonoBehaviour
{
    [SerializeField]
    private Text nickname;
    [SerializeField]
    private Text score;

    public Player Player { get; set; }

    private void FixedUpdate()
    {
        nickname.text = Player.name;
        score.text = Player.score.ToString();
    }
}
