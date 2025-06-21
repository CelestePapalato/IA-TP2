using UnityEngine;

public class Pez : MonoBehaviour
{
    [Header("Configuraci�n")]
    public float rapidez = 4;
    public float radioDePercepci�n = 2.5f;
    public float radioDeSeparaci�n = 1;
    public float fuerzaM�ximaDeGiro = 3;

    public float alineaci�nPeso = 1;
    public float cohesi�nPeso = 1;
    public float separaci�nPeso = 1;
    public float objetivoPeso = 1;

    private Vector3 fuerzaDeSeparacion = Vector3.zero;
    private Vector3 fuerzaDeAlineacion = Vector3.zero;
    private Vector3 fuerzaDeCohesion = Vector3.zero;
    private Vector3 fuerzaObjetivo = Vector3.zero;

    private Vector3 velocidad = Vector3.zero;

    Transform transformObjetivo;

    Cardumen cardumen;

    [Header("A actualizar por el controlador")]
    public int cantidadDePecesVecinos;

    public void Inicializar(Transform objetivo)
    {
        Cardumen cardumen = Cardumen.Instancia;
        transformObjetivo = objetivo;
    }

    public void Avanzar()
    {
        Vector3 fuerza = fuerzaDeSeparacion * separaci�nPeso +
                         fuerzaDeAlineacion * alineaci�nPeso +
                         fuerzaDeCohesion * cohesi�nPeso +
                         fuerzaObjetivo * objetivoPeso;
        velocidad = transform.forward * rapidez;
        velocidad += fuerza * Time.deltaTime;
        velocidad = velocidad.normalized * rapidez;
        transform.position += velocidad * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(velocidad);
    }

    public void CalcularFuerzas()
    {
        // Tal vez calcule los peces vecinos y actualice la velocidad con Jobs 
    }

}
