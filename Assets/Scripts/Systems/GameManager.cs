using System.Collections.Generic;
using Roomba.Systems.Actors;
using UnityEngine;

namespace Roomba.Systems
{
    public class GameManager
    {
        private List<Actor> _players;

        public Vector3 Center => GetCenter();

        public GameManager()
        {
            _players = new List<Actor>();
        }

        private Vector3 GetCenter()
        {
            var c = Vector3.zero;
            
            for (int i = 0; i < _players.Count; i++)
            {
                c += _players[i].transform.position;
            }

            c /= _players.Count;

            return c;
        }

        public void SetPlayer(Actor player)
        {
            _players.Add(player);
        }
    }
}