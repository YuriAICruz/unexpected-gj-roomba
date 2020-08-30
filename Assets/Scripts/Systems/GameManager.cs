using System.Collections.Generic;
using System.Linq;
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

            var ps = _players.Where(x => x.isLocalPlayer).ToArray();
            for (int i = 0; i < ps.Length; i++)
            {
                c += ps[i].transform.position;
            }

            c /= ps.Length;

            return c;
        }

        public void SetPlayer(Actor player)
        {
            _players.Add(player);
        }
    }
}