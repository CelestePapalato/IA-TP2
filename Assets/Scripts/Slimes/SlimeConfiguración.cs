using UnityEngine;

[CreateAssetMenu(fileName = "SlimeConfiguración", menuName = "Scriptable Objects/Slime Configuración")]
public class SlimeConfiguración : ScriptableObject
{
    public float tiempoDeVida = 10f;

    public float rapidez = 4;
    public float sqrDistanciaSeparación = 7f;

    public float alineaciónPeso = 1;
    public float cohesiónPeso = 1;
    public float separaciónPeso = 1;
    public float objetivoPeso = 1;
    public float evasiónPeso = 1;

    public float distanciaEvasiónDeColisión = 5f;
    public float radioDeColisión = 5f;
    public LayerMask maskObstáculos;
}
