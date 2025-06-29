using UnityEngine;

public class SeguirMouse: MonoBehaviour
{
    [SerializeField]
    private bool seguirMouse = true;

    [SerializeField]
    private float velocidadMovimientoAleatorio = 5f;
    private Vector3 targetPosition;

    private Camera mainCamera;

    private Vector3 boundsMin;
    private Vector3 boundsMax;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    void Start()
    {
        if (!seguirMouse)
        {
            SetNewRandomTargetPosition();
        }
    }

    void Update()
    {
        if (seguirMouse)
        {
            SeguirPunteroDelMouse();
        }
        else
        {
            MoverAleatoriamente();
        }
    }

    private void SeguirPunteroDelMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane suelo = new Plane(Vector3.up, Vector3.zero);

        if (suelo.Raycast(ray, out float distancia))
        {
            Vector3 point = ray.GetPoint(distancia);
            transform.position = new Vector3(point.x, 0f, point.z);
        }
    }

    private void MoverAleatoriamente()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, velocidadMovimientoAleatorio * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewRandomTargetPosition();
        }
    }

    private void SetNewRandomTargetPosition()
    {
        Plane suelo = new Plane(Vector3.up, Vector3.zero);

        Ray testRay = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        float zDistance = 0f;
        if (suelo.Raycast(testRay, out float hitDistance))
        {
            zDistance = hitDistance;
        }
        else
        {
            zDistance = 10f;
        }

        Vector3 lowerLeftWorld = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, zDistance));
        Vector3 upperRightWorld = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, zDistance));

        float minX = lowerLeftWorld.x;
        float maxX = upperRightWorld.x;
        float minZ = lowerLeftWorld.z;
        float maxZ = upperRightWorld.z;

        targetPosition = new Vector3(
            Random.Range(minX, maxX),
            0f,
            Random.Range(minZ, maxZ)
        );

        boundsMin = new Vector3(minX, 0f, minZ);
        boundsMax = new Vector3(maxX, 0f, maxZ);
    }

    private void OnDrawGizmosSelected()
    {
        if (!seguirMouse && mainCamera != null)
        {
            Gizmos.color = Color.blue;
            Vector3 center = (boundsMin + boundsMax) * 0.5f;
            Vector3 size = boundsMax - boundsMin;
            Gizmos.DrawWireCube(new Vector3(center.x, 0f, center.z), new Vector3(size.x, 0.1f, size.z));
        }
    }
}