using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandByHand.NightSystem.SignLanguageSystem
{
    public enum Symbol 
    {
        None
    }

    public enum Direction
    {
        None
    }

    [System.Serializable]
    public class SymbolAndDirection
    {
        public Sprite Sprite;

        public Symbol Symbol;

        public Direction Direction;
    }
}
