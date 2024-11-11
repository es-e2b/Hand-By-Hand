namespace Assets.Scripts.Editors
{
    using TMPro;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class UIFontSetter
    {
        public const string PATH_FONT="Assets/TextMesh Pro/Fonts/NanumSquareRoundEB SDF30.asset";
        [MenuItem("CustomEditor/Fonts/ChangeTextMeshPro")]
        public static void ChangeFontInTextMeshPro()
        {
            GameObject[] rootObj=GetSceneRootObjects();
            for(int i=0;i<rootObj.Length;i++)
            {
                GameObject gbj = (GameObject)rootObj[i] as GameObject;
                Component[] com=gbj.transform.GetComponentsInChildren(typeof(TextMeshProUGUI), true);
                foreach(TextMeshProUGUI txt in com)
                {
                    txt.font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(PATH_FONT);
                    txt.fontStyle = FontStyles.Normal;
                }
            }
        }
        [MenuItem("CustomEditor/Fonts/ChangeTextMeshProInPrefab")]
        public static void ChangeFontInTextMeshProInPrefab()
        {
            TMP_FontAsset fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(PATH_FONT);
            
            GameObject[] rootObjects = GetSceneRootObjects();
            foreach (GameObject rootObj in rootObjects)
            {
                // Get all TextMeshProUGUI components in children, including inactive ones
                TextMeshProUGUI[] textComponents = rootObj.GetComponentsInChildren<TextMeshProUGUI>(true);
                foreach (TextMeshProUGUI txt in textComponents)
                {
                    txt.font = fontAsset;
                    txt.fontStyle = FontStyles.Normal;
                    
                    // If the object is part of a prefab, apply the change to the prefab asset
                    GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(txt.gameObject);
                    if (prefab != null)
                    {
                        PrefabUtility.ApplyPrefabInstance(txt.gameObject, InteractionMode.UserAction);
                    }
                }
            }
        }
        private static GameObject[] GetSceneRootObjects()
        {
            Scene currentScene=SceneManager.GetActiveScene();
            return currentScene.GetRootGameObjects();
        }
    }
}