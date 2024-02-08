using System;
using System.Collections;
using Code.Utils;
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

        private readonly float _scaleDuration = 0.5f;

        private bool _isFlipping;
        private bool _isFrontSideVisible;
        private Vector3 _cachedCardScale;

        private Quaternion _halfFlipRotation;
        private Quaternion _fullFlipRotation;

        public Action<GameCard> OnCardClick;
        public bool IsMatched { get; private set; }

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
            StartCoroutine(MoveAndFadeOut());
        }

        public void ReleaseCardToPool()
        {
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
                        card.color = _isFrontSideVisible
                            ? Utilities.GetColorFromSpriteCenter(icon.sprite)
                            : Color.white;
                        icon.gameObject.SetActive(_isFrontSideVisible);
                        break; // Break the loop and continue to the next iteration for the second half of the flip
                    }

                    yield return null;
                }

                transform.rotation = endRotation;
            }

            _isFlipping = false;
        }

        private IEnumerator MoveAndFadeOut()
        {
            var transform1 = card.transform;
            var originalScale = Vector3.one * transform1.localScale.x;
            var targetScale = originalScale * 1.1f;

            // Scale up
            float elapsedTime = 0;
            while (elapsedTime < _scaleDuration)
            {
                if (elapsedTime < _scaleDuration / 2)
                    transform.localScale = Vector3.one * Mathf.Lerp(originalScale.x, targetScale.x,
                        elapsedTime / (_scaleDuration / 2));
                else
                    transform.localScale = Vector3.one * Mathf.Lerp(targetScale.x, 0,
                        (elapsedTime - _scaleDuration / 2) / (_scaleDuration / 2));

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localScale = Vector3.zero;

            ReleaseCardToPool();
        }


        public void OnObjectReturned()
        {
            card.transform.localScale = _cachedCardScale;
        }

        public void OnObjectInit()
        {
            Reset();
            _cachedCardScale = card.transform.localScale;
        }

        private void Reset()
        {
            _isFlipping = false;
            _isFrontSideVisible = false;
            IsMatched = false;
            card.sprite = cardBackSprite;
            card.color = Color.white;
            icon.gameObject.SetActive(false);
        }

        public SimpleObjectPool Pool { get; set; }
    }
}