using UnityEngine;

[CreateAssetMenu(fileName = "GallinaConfiguración", menuName = "Scriptable Objects/Gallina Configuración")]
public class GallinaConfiguración : ScriptableObject
{
    [Header("Movimiento")]
    public float rapidez = 4;
    public float sqrDistanciaSeparación = 7f;

    [Header("Detección de colisiones")]
    public float distanciaEvasiónDeColisión = 5f;
    public float radioDeColisión = 5f;
    public LayerMask maskObstáculos;

    [Header("Pesos")]
    public float alineaciónPeso = 1;
    public float cohesiónPeso = 1;
    public float separaciónPeso = 1;
    public float objetivoPeso = 1;
    public float evasiónPeso = 1;
}
