using UnityEngine;

namespace Code.Game.Controller
{
    public class CardGridGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private int rows;
        [SerializeField] private int columns;
        [SerializeField] private float cardSpacing = 1.6f;

        private void Start()
        {
            GenerateCardGrid();
        }

        private void GenerateCardGrid()
        {
            var gridWidth = columns * cardSpacing;
            var gridHeight = rows * cardSpacing;

            var startX = -gridWidth / 2 + cardSpacing / 2;
            var startY = gridHeight / 2 - cardSpacing / 2;

            for (var row = 0; row < rows; row++)
            for (var col = 0; col < columns; col++)
            {
                var posX = startX + col * cardSpacing;
                var posY = startY - row * cardSpacing;

                var position = new Vector3(posX, posY, 0f);
                var rotation = Quaternion.identity;

                var card = Instantiate(cardPrefab, position, rotation);
            }
        }
    }
}