namespace Assets.Scripts.CharacterData
{
    using UnityEngine;
    [CreateAssetMenu(fileName = "Character", menuName = "")]
    public class Character : ScriptableObject
    {
        public Sprite CharacterSprite;
        public string Name;
    }
}