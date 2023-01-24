using System;
using System.Collections.Generic;
using System.Text;

namespace RockPaperScissors
{
    public class Player
    {
        string username;
        int score;

        public Player(string username, int score)
        {
            Username = username;
            Score = score;
        }

        public string Username { get => username; set => username = value; }
        public int Score { get => score; set => score = value; }
    }
}
