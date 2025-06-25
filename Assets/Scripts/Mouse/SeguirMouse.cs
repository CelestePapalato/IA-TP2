using UnityEngine;

public class SeguirMouse : MonoBehaviour
{
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane suelo = new Plane(Vector3.up, Vector3.zero);

        if (suelo.Raycast(ray, out float distancia))
        {
            Vector3 point = ray.GetPoint(distancia);
            transform.position = point;
        }
    }
}
