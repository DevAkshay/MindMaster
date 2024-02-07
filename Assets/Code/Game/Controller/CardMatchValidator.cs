using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Game.Card;
using Code.Game.Core;
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
        
        private int _remainingAttempts;
        private int _allowedAttemptCount;
        private int _currentAttemptCount;
        
        private GameCard _firstSelectedCard;
        private GameCard _secondSelectedCard;
        
        private readonly float _matchCheckDelay = 0.6f;
        private readonly float _initialCardPeekDuration = 3.0f;
        
        private enum GameState { Ready, CheckingMatch, Paused }
        private GameState _currentState = GameState.Ready;

        public void Initialize(List<GameCard> generatedCards, LevelDataSO levelData)
        {
            _generatedCards = generatedCards;
            _allowedAttemptCount = _remainingAttempts = levelData.NumberOfAttempts;
            StartCoroutine(StartLevelCardPeek());
        }

        private IEnumerator StartLevelCardPeek()
        {
            yield return new WaitForSeconds(0.5f);
            foreach (var card in _generatedCards) card.Flip();
            yield return new WaitForSeconds(_initialCardPeekDuration);

            foreach (var card in _generatedCards)
            {
                card.Flip();
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
            UpdateAttemptCount();

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

        
        private void UpdateAttemptCount()
        {
            _currentAttemptCount++;
            _remainingAttempts = _allowedAttemptCount - _currentAttemptCount;
            GameEvents.NotifyAttemptCountUpdated(_remainingAttempts);
        }

        private void CheckAboutGameOver()
        {
            if (!IsTurnAllowed() || IsLevelCleared())
            {
                Reset();
                OnGameOver?.Invoke();
            }
        }
        
        private void Reset()
        {
            _remainingAttempts = 0;
            _allowedAttemptCount = 0;
            _currentAttemptCount = 0;
        }
        
        private bool IsTurnAllowed()
        {
            return _remainingAttempts > 0;
        }

        private bool IsLevelCleared()
        {
            return _generatedCards.TrueForAll(card => card.IsMatched);
        }
    }
}