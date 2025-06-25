using UnityEngine;

[CreateAssetMenu(fileName = "SlimeConfiguración", menuName = "Scriptable Objects/Slime Configuración")]
public class SlimeConfiguración : ScriptableObject
{
    public float rapidez = 4;
    public float sqrDistanciaSeparación = 7f;

    public float alineaciónPeso = 1;
    public float cohesiónPeso = 1;
    public float separaciónPeso = 1;
    public float objetivoPeso = 1;
}
