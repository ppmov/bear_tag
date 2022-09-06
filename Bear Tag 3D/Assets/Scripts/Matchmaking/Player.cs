using Mirror;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Player
{
    public readonly string name;
    public readonly uint netId;
    public int score;

    public Player() { }

    public Player(string name, uint netId)
    {
        this.name = name;
        this.netId = netId;
        this.score = 0;
    }

    public Player(Player other)
    {
        this.name = other.name;
        this.netId = other.netId;
        this.score = other.score;
    }

    public Player Score()
    {
        Player player = new Player(this);
        player.score++;
        return player;
    }
}
