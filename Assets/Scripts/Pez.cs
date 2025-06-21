using UnityEngine;

public class Pez : MonoBehaviour
{
    [Header("Configuración")]
    public float rapidez = 4;
    public float radioDePercepción = 2.5f;
    public float radioDeSeparación = 1;
    public float fuerzaMáximaDeGiro = 3;

    public float alineaciónPeso = 1;
    public float cohesiónPeso = 1;
    public float separaciónPeso = 1;
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
        Vector3 fuerza = fuerzaDeSeparacion * separaciónPeso +
                         fuerzaDeAlineacion * alineaciónPeso +
                         fuerzaDeCohesion * cohesiónPeso +
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
