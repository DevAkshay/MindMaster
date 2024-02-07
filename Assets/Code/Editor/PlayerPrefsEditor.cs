using UnityEditor;
using UnityEngine;

namespace Code.Editor
{
    public class PlayerPrefsEditor
    {
        [MenuItem("Edit/Clear PlayerPrefs")]
        private static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}