using UnityEngine;

namespace Code.Game.Card
{
    public interface IGameCard
    {
        public void Flip();
        public void SetIcon(Sprite sprite);
        public void Match();
        public string GetName();
    } 
}

