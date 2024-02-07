using System;
using System.Collections;
using Code.Utils.Pooling;
using UnityEngine;

namespace Code.Game.Card
{
    public class GameCard : MonoBehaviour, IGameCard, IPoolObject
    {
        [SerializeField] private SpriteRenderer card;
        [SerializeField] private SpriteRenderer icon;
        
        [SerializeField] private float flipDuration = 0.2f;
        [SerializeField] private AnimationCurve flipCurve = AnimationCurve.Linear(0, 0, 1, 1);
        
        [SerializeField] private Sprite cardBackSprite;
        [SerializeField] private Sprite cardFrontSprite;

        public Action<GameCard> OnCardClick;
        
        public bool IsMatched { get; private set; }

        private Quaternion _startRotation;
        private Quaternion _endRotation;

        private bool _isFlipping;
        private bool _isFrontSideVisible; // Flag to track if the front side of the card is visible


        private void OnMouseDown()
        {
            OnCardClick?.Invoke(this);
        }

        public void Flip()
        {
            if (_isFlipping)
                return;
            
            // Start flipping animation
            StartCoroutine(AnimateFlip());
        }

        public void SetIcon(Sprite sprite)
        {
            icon.sprite = sprite;
        }

        public bool Match(string cardIconName)
        {
            return icon.sprite.name.Equals(cardIconName);
        }

        public string GetName()
        {
            return icon.sprite.name;
        }

        public void SetAsMatched()
        {
            IsMatched = true;
            
            Pool.ReleaseObject(gameObject);
        }

        private IEnumerator AnimateFlip()
        {
            _isFlipping = true;
            var elapsedTime = 0f;

            _startRotation = transform.rotation;
            _endRotation = _startRotation * Quaternion.Euler(0, 90, 0);

            _isFrontSideVisible = !_isFrontSideVisible;

            while (elapsedTime < flipDuration)
            {
                var t = flipCurve.Evaluate(elapsedTime / flipDuration);
                transform.rotation = Quaternion.Slerp(_startRotation, _endRotation, t);

                if (t >= 0.97f)
                {
                    // Switch card sprite to front side if it's visible, otherwise switch to back side
                    card.sprite = _isFrontSideVisible ? cardFrontSprite : cardBackSprite;
                    icon.gameObject.SetActive(_isFrontSideVisible);
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.rotation = _endRotation;

            // Set the start and end rotations for the second half of the flip
            _startRotation = _endRotation;
            _endRotation = _startRotation * Quaternion.Euler(0, -90, 0);
            elapsedTime = 0f;

            while (elapsedTime < flipDuration)
            {
                var t = flipCurve.Evaluate(elapsedTime / flipDuration);
                transform.rotation = Quaternion.Slerp(_startRotation, _endRotation, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.rotation = _endRotation;
            _isFlipping = false;

        }

        public void OnObjectInit()
        {
            Reset();
        }

        private void Reset()
        {
            _isFlipping = false;
            _isFrontSideVisible = false;
            IsMatched = false;
            card.sprite = cardBackSprite;
            icon.gameObject.SetActive(false);
        }

        public SimpleObjectPool Pool { get; set; }
    }
}