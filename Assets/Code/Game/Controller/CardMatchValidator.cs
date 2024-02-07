using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Game.Card;
using Core.Level;
using UnityEngine;

namespace Code.Game.Controller
{
    public class CardMatchValidator : MonoBehaviour
    {
        public Action OnPairMatched;
        public Action OnPairMismatched;
        public Action OnGameOver;

        private List<GameCard> _generatedCards;
        
        private int _remainingTurns;
        private int _allowedTurnsCount;
        private int _currentTurnCount;
        
        private GameCard _firstSelectedCard;
        private GameCard _secondSelectedCard;
        
        private float _matchCheckDelay = 0.6f;
        
        private enum GameState { Ready, CheckingMatch, Paused }
        private GameState _currentState = GameState.Ready;

        public void Initialize(List<GameCard> generatedCards, LevelDataSO levelData)
        {
            _generatedCards = generatedCards;
            _allowedTurnsCount = _remainingTurns = levelData.NumberOfTurns;
            RegisterForCardClick();
        }

        private void RegisterForCardClick()
        {
            foreach (var card in _generatedCards)
            {
                card.OnCardClick += OnCardClick;
            }
        }

        private void OnCardClick(GameCard gameCard)
        {
            if (!IsTurnAllowed()) return;
            if (_currentState != GameState.Ready || gameCard == _firstSelectedCard) return;

            gameCard.Flip();
            if (_firstSelectedCard == null)
            {
                _firstSelectedCard = gameCard;
            }
            else
            {
                _secondSelectedCard = gameCard;
                _currentState = GameState.CheckingMatch;
                Invoke(nameof(CheckForMatch), _matchCheckDelay);
            }
        }

        private void CheckForMatch()
        {
            UpdateOnTurnCount();

            if (_firstSelectedCard.Match(_secondSelectedCard.GetName()))
            {
                HandleMatch();
            }
            else
            {
                HandleMismatch();
            }

            _firstSelectedCard = null;
            _secondSelectedCard = null;
            _currentState = GameState.Ready;

            CheckAboutGameOver();
        }
        
        private void HandleMatch()
        {
            _firstSelectedCard.SetAsMatched();
            _secondSelectedCard.SetAsMatched();
            OnPairMatched?.Invoke();
        }

        private void HandleMismatch()
        {
            _firstSelectedCard.Flip();
            _secondSelectedCard.Flip();
            OnPairMismatched?.Invoke();
        }

        
        private void UpdateOnTurnCount()
        {
            _currentTurnCount++;
            _remainingTurns = _allowedTurnsCount - _currentTurnCount;
        }

        private void CheckAboutGameOver()
        {
            if (!IsTurnAllowed() || IsLevelCleared())
            {
                OnGameOver?.Invoke();
            }
        }
        
        private bool IsTurnAllowed()
        {
            return _remainingTurns > 0;
        }

        private bool IsLevelCleared()
        {
            return _generatedCards.TrueForAll(card => card.IsMatched);
        }
    }
}