using UnityEngine;
using UnityEngine.UI;

namespace Code.Game.Card
{
    public class GameCard : MonoBehaviour, IGameCard
    {
        [SerializeField] private SpriteRenderer cardImg; 
        public void Flip()
        {
        }

        public void SetIcon(Sprite sprite)
        {
            cardImg.sprite = sprite;
        }

        public void Match()
        {
        }

        public string GetName()
        {
            return "";
        }
    }
}
