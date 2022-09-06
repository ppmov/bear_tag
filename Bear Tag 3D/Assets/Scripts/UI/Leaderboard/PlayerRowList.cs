using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRowList : MonoBehaviour
{
    [SerializeField]
    private GameObject rowPrefab;
    [SerializeField]
    private float margin;

    private readonly List<PlayerRow> list = new List<PlayerRow>();

    private void FixedUpdate()
    {
        for (int i = 0; i < Game.MaxPlayers; i++)
        {
            if (Game.Party.Count <= i)
            {
                if (list.Count <= i)
                    break;
                else
                    RemoveRow(i);
            }
            else
            {
                if (list.Count <= i)
                    AddRow(Game.Party[i]);
                else
                    list[i].Player = Game.Party[i];
            }
        }
    }

    private void AddRow(Player player)
    {
        PlayerRow row = Instantiate(rowPrefab, transform).GetComponent<PlayerRow>();
        list.Add(row);
        row.Player = player;
        Align();
    }

    private void RemoveRow(int index)
    {
        PlayerRow row = list[index];
        list.RemoveAt(index);
        Destroy(row.gameObject);
        Align();
    }

    private void Align()
    {
        for (int i = 0; i < list.Count; i++)
            list[i].transform.localPosition = new Vector3(0f, i * -margin, 0f);
    }
}
