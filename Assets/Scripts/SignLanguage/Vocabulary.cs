namespace Assets.Scripts.SignLanguage
{
    using System.Collections.Generic;
    using UnityEngine;
    
    [CreateAssetMenu(fileName = "Vocabulary", menuName = "")]
    public class Vocabulary : ScriptableObject
    {
        public string Name;
        public string Hint;
        public List<Handshape> RightHandshapes=new();
        public List<Handshape> LeftHandshapes=new();
    }
}