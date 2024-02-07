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

        private bool _isFlipping;
        private bool _isFrontSideVisible; // Flag to track if the front side of the card is visible

        private Quaternion _halfFlipRotation;
        private Quaternion _fullFlipRotation;

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
            _halfFlipRotation = Quaternion.Euler(0, 90, 0);
            _fullFlipRotation = Quaternion.Euler(0, 180, 0);

            for (var i = 0; i < 2; i++)
            {
                var elapsedTime = 0f;
                var startRotation = transform.rotation;
                var endRotation = i == 0 ? _halfFlipRotation : _fullFlipRotation;

                while (elapsedTime < flipDuration)
                {
                    elapsedTime += Time.deltaTime;
                    var t = flipCurve.Evaluate(elapsedTime / flipDuration);
                    transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);

                    if (i == 0 && t >= 0.5f) // Midway, change the sprite and icon visibility
                    {
                        _isFrontSideVisible = !_isFrontSideVisible;
                        card.sprite = _isFrontSideVisible ? cardFrontSprite : cardBackSprite;
                        icon.gameObject.SetActive(_isFrontSideVisible);
                        break; // Break the loop and continue to the next iteration for the second half of the flip
                    }

                    yield return null;
                }

                transform.rotation = endRotation;
            }

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