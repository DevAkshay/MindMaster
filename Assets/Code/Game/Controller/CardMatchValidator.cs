using System;
using System.Collections;
using System.Collections.Generic;
using Code.Game.Card;
using Core.Level;
using UnityEngine;

namespace Code.Game.Controller
{
    public class CardMatchValidator : MonoBehaviour
    {
        public Action OnPairMatched;
        public Action OnPairMismatched;
        public Action OnGameover;

        private List<GameCard> _generatedCards;
        private LevelDataSO _activeLevelData;
        private readonly Queue<GameCard> _cardQueue = new();

        public void Initialize(List<GameCard> generatedCards, LevelDataSO levelData)
        {
            _generatedCards = generatedCards;
            _activeLevelData = levelData;
            RegisterForCardClick();
        }

        private void RegisterForCardClick()
        {
            foreach (var card in _generatedCards)
            {
                card.OnCardClick += OnCardClick;
                card.OnCardShown += OnCardFlipComplete;
            }
        }

        private void OnCardFlipComplete()
        {
            StartCoroutine(CheckForMatch());
        }

        private void OnCardClick(GameCard gameCard)
        {
            gameCard.Flip();
            _cardQueue.Enqueue(gameCard);
        }

        private IEnumerator CheckForMatch()
        {
            if (_cardQueue.Count < 2) yield break;
            var firstClickedCard = _cardQueue.Dequeue();
            var secondClickedCard = _cardQueue.Dequeue();
            yield return new WaitForSeconds(0.2f);
            if (firstClickedCard.Match(secondClickedCard.GetName()))
            {
                firstClickedCard.SetAsMatched();
                secondClickedCard.SetAsMatched();
                
                firstClickedCard.OnCardClick -= OnCardClick;
                secondClickedCard.OnCardClick -= OnCardClick;
                
                OnPairMatched?.Invoke();
            }
            else
            {
                firstClickedCard.Flip();
                secondClickedCard.Flip();
                
                OnPairMismatched?.Invoke();
            }

            if (_cardQueue.Count >= 2)
                StartCoroutine(CheckForMatch());
            else
                OnGameover?.Invoke();
        }
    }
}