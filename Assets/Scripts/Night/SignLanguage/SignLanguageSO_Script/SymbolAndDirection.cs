using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandByHand.NightSystem.SignLanguageSystem
{
    public enum Symbol 
    {

    }

    public enum Direction
    {

    }

    [System.Serializable]
    public class SymbolAndDirection
    {
        public Sprite sprite;

        public Symbol symbol;

        public Direction direction;
    }
}
