using UnityEngine;

namespace Code.Game.Controller
{
    public class CardGridGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private int rows;
        [SerializeField] private int columns;
        [SerializeField] private float cardSpacing = 1.6f;

        private readonly float _cardLength = 0.3f;
        private readonly float _edgePadding = 2f;
        private readonly float _topPadding = 1.5f;

        private void Start()
        {
            GenerateCardGrid();
            AdjustCamera();
        }

        private void GenerateCardGrid()
        {
            var gridWidth = columns * cardSpacing;
            var gridHeight = rows * cardSpacing;

            var startX = -gridWidth / 2 + cardSpacing / 2;
            var startY = gridHeight / 2 - cardSpacing / 2 - _topPadding;

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

        private void AdjustCamera()
        {
            var cam = Camera.main;
            var totalCardWidth = columns * _cardLength + (columns - 1) * cardSpacing;
            var totalCardHeight = rows * _cardLength + (rows - 1) * cardSpacing;

            var desiredWidth = totalCardWidth + _edgePadding;
            var desiredHeight = totalCardHeight + _edgePadding + _topPadding;
            if (cam != null)
            {
                var aspectRatio = cam.aspect;
                var newOrthographicSize = Mathf.Max(desiredHeight / 2, desiredWidth / (2 * aspectRatio));
                cam.orthographicSize = newOrthographicSize;
            }
        }
    }
}