namespace Assets.Scripts.Tycoon.RestaurantSystem.MenuData
{
    using UnityEngine;
    using SignLanguage;
    
    [CreateAssetMenu(fileName = "Menu", menuName = "")]
    public class Menu : ScriptableObject
    {
        public MenuType MenuType;
        public string Name;
        public Sprite Sprite;
        public int Price;
        public float EatingDuration;
        public Vocabulary Vocabulary;
    }
    public enum MenuType
    {
        Food,
        Beverage
    }
}