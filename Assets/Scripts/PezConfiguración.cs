using UnityEngine;

[CreateAssetMenu(fileName = "PezConfiguración", menuName = "Scriptable Objects/PezConfiguración")]
public class PezConfiguración : ScriptableObject
{
    public float rapidez = 4;
    public float sqrDistanciaSeparación = 7f;

    public float alineaciónPeso = 1;
    public float cohesiónPeso = 1;
    public float separaciónPeso = 1;
    public float objetivoPeso = 1;
}
