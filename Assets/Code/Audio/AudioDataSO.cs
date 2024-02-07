using System.Linq;
using UnityEngine;

namespace Code.Audio
{
    [CreateAssetMenu(fileName = "AudioDatabase", menuName = "AudioDatabase")]
    public class AudioDataSO : ScriptableObject
    {
        [System.Serializable]
        public class AudioEntry
        {
            public string id;
            public AudioClip clip;
        }

        public AudioEntry[] audioEntries;

        public AudioClip GetAudioClipById(string id)
        {
            return (from audioData in audioEntries where audioData.id == id select audioData.clip).FirstOrDefault();
        }
    }
}