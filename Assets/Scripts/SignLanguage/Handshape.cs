namespace Assets.Scripts.SignLanguage
{
    using UnityEngine;
    
    [System.Serializable]
    public class Handshape
    {
        public Sprite HandshapeImage; // 수형 이미지
        public Vector2 StartPosition; // 시작 위치
        public Vector2 EndPosition;   // 도착 위치
        public Vector3 StartRotation; // 시작 회전
        public Vector3 EndRotation;   // 도착 회전
        public float Duration;
    }
}
