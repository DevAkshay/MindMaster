using System.Collections.Generic;
using Code.Game.Card;
using Code.Utils;
using Code.Utils.Pooling;
using Core.Level;
using UnityEngine;

namespace Code.Game.Controller
{
    public class CardGridGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private float cardSpacing = 1.6f;

        private readonly float _cardLength = 0.4f;
        private readonly float _edgePadding = 2f;
        private readonly float _topPadding = 1.4f;

        private IObjectPoolManager _objectPoolManager;
        private List<GameCard> _generatedCards;
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _objectPoolManager = ObjectPoolManager.Instance;
            _objectPoolManager.CreatePool(cardPrefab.name, cardPrefab, 25);
        }

        public void Initialize(LevelDataSO levelData)
        {
            _generatedCards = new List<GameCard>(levelData.RowCount * levelData.ColumnCount);
            GenerateCardGrid(levelData.RowCount, levelData.ColumnCount);
            AssignSpritesToCards(levelData);
            AdjustCamera(levelData.RowCount, levelData.ColumnCount);
        }

        public List<GameCard> GetGeneratedCards()
        {
            return _generatedCards;
        }

        private void GenerateCardGrid(int rows, int columns)
        {
            var gridWidth = columns * cardSpacing;
            var gridHeight = rows * cardSpacing;

            var startPosition = new Vector3(-gridWidth / 2 + cardSpacing / 2,
                gridHeight / 2 - cardSpacing / 2 - _topPadding, 0f);

            for (var row = 0; row < rows; row++)
            for (var col = 0; col < columns; col++)
            {
                var position = startPosition + new Vector3(col * cardSpacing, -row * cardSpacing, 0f);
                var card = _objectPoolManager.GetObjectFromPool(cardPrefab.name, position, Quaternion.identity);
                card.transform.SetParent(transform);
                _generatedCards.Add(card.GetComponent<GameCard>());
            }
        }

        private void AdjustCamera(int rows, int columns)
        {
            var totalCardWidth = columns * _cardLength + (columns - 1) * cardSpacing;
            var totalCardHeight = rows * _cardLength + (rows - 1) * cardSpacing;

            var desiredWidth = totalCardWidth + _edgePadding;
            var desiredHeight = totalCardHeight + _edgePadding + _topPadding;

            if (_mainCamera != null)
            {
                var aspectRatio = _mainCamera.aspect;
                _mainCamera.orthographicSize = Mathf.Max(desiredHeight / 2, desiredWidth / (2 * aspectRatio));
            }
        }

        private void AssignSpritesToCards(LevelDataSO levelData)
        {
            var cardSprites = levelData.CardSprites;

            Utilities.Shuffle(levelData.CardSprites);
            var numCardsNeeded = _generatedCards.Count / 2;
            var selectedImages = new List<Sprite>();

            // Select the required number of images for the grid
            for (var i = 0; i < numCardsNeeded; i++)
            {
                selectedImages.Add(cardSprites[i]);
                selectedImages.Add(cardSprites[i]);
            }

            Utilities.Shuffle(selectedImages);

            // Assign each card in the grid a sprite from the shuffled list of images
            for (var i = 0; i < _generatedCards.Count; i++) _generatedCards[i].SetIcon(selectedImages[i]);
        }
    }
}