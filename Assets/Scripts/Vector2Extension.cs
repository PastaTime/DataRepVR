using UnityEngine;
// Code provided by DDP on Unity3D forums, http://answers.unity3d.com/answers/734946/view.html
// Accessed 2017-08-17
public static class Vector2Extension {
     
    public static Vector2 Rotate(this Vector2 v, float degrees) {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
         
        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
}