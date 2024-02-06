using System.Collections.Generic;
using Code.Game.Card;
using Code.Level;
using Code.Player;
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

        private readonly float _cardLength = 0.3f;
        private readonly float _edgePadding = 2f;
        private readonly float _topPadding = 1.5f;

        private IObjectPoolManager _objectPoolManager;
        private List<GameCard> _generatedCards;
        
        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            _objectPoolManager = ObjectPoolManager.Instance;
            _objectPoolManager.CreatePool(cardPrefab.name, cardPrefab, 25);

            var levelData = LevelManager.Instance.GetLevel(PlayerDataManager.Instance.currentLevel);
            GenerateCardGrid(levelData.RowCount, levelData.ColumnCount);
            AssignSpritesToCards(levelData);
            AdjustCamera(levelData.RowCount, levelData.ColumnCount);
        }

        private void GenerateCardGrid(int rows, int columns)
        {
            var gridWidth = columns * cardSpacing;
            var gridHeight = rows * cardSpacing;

            var startX = -gridWidth / 2 + cardSpacing / 2;
            var startY = gridHeight / 2 - cardSpacing / 2 - _topPadding;
            
            _generatedCards = new List<GameCard>();
            for (var row = 0; row < rows; row++)
            for (var col = 0; col < columns; col++)
            {
                var posX = startX + col * cardSpacing;
                var posY = startY - row * cardSpacing;

                var position = new Vector3(posX, posY, 0f);
                var rotation = Quaternion.identity;

                var card = _objectPoolManager.GetObjectFromPool(cardPrefab.name, position, rotation);
                card.transform.SetParent(transform);
                var gameCard = card.GetComponent<GameCard>();
                _generatedCards.Add(gameCard);
            }
        }

        private void AdjustCamera(int rows, int columns)
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
                // Add a duplicate for matching pairs
                selectedImages.Add(cardSprites[i]); 
            }

            // Shuffle the list of selected images
            Utilities.Shuffle(selectedImages);
            
            // Assign each card in the grid a sprite from the shuffled list of images
            for (var i = 0; i < _generatedCards.Count; i++)
            {
                _generatedCards[i].SetIcon(selectedImages[i]);
            }
        }
    }

}