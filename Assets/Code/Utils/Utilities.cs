using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Utils
{
    public static class Utilities
    {
        public static void Shuffle<T>(List<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = Random.Range(0, n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        public static IEnumerator FadeInCoroutine(float duration, CanvasGroup canvasGroup)
        {
            canvasGroup.gameObject.SetActive(true);

            canvasGroup.alpha = 0f;

            var elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = 1f;
        }

        public static IEnumerator MoveYCoroutine(Transform obj, float targetY, float duration, float delay,
            Action onComplete)
        {
            yield return new WaitForSeconds(delay);
            var startTime = Time.time;
            var endTime = startTime + duration;

            var position = obj.localPosition;
            var startPosition = position;
            var endPosition = new Vector3(position.x, targetY, position.z);

            while (Time.time < endTime)
            {
                var t = (Time.time - startTime) / duration;

                var easeT = 1 - (1 - t) * (1 - t);

                obj.localPosition = Vector3.Lerp(startPosition, endPosition, easeT);

                yield return null;
            }

            obj.localPosition = endPosition;
            onComplete?.Invoke();
        }

        public static IEnumerator Delay(float delayInSeconds, Action finishedCallback)
        {
            yield return new WaitForSeconds(delayInSeconds);
            finishedCallback();
        }

        public static Color GetColorFromSpriteCenter(Sprite sprite, float offsetX = 0.05f, float offsetY = 0.05f)
        {
            var texture = sprite.texture;
            var rect = sprite.textureRect;

            if (!texture.isReadable)
            {
                Debug.LogError("Texture is not readable. Make sure Read/Write is enabled in import settings.");
                return Color.white;
            }

            // Calculate offset position from the center point within the sprite's textureRect
            var sampleX = Mathf.Clamp(Mathf.FloorToInt(rect.x + rect.width / 2 + rect.width * offsetX), 0,
                texture.width - 1);
            var sampleY = Mathf.Clamp(Mathf.FloorToInt(rect.y + rect.height / 2 + rect.height * offsetY), 0,
                texture.height - 1);

            var offsetColor = texture.GetPixel(sampleX, sampleY);
            var darkerColor = offsetColor * 0.9f; //making it slight darker

            return darkerColor;
        }
    }
}