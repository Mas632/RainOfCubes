using System.Collections;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour
{
    [Header("Настройки самоликвидации куба при выходе его за границы области")]
    [SerializeField] private bool _isDistanceChecking;
    [SerializeField] private float _maxDistance;

    [Header("Настройки продолжительности жизни куба после столкновения")]
    [SerializeField] private float _minLifeTimeAfterCollision;
    [SerializeField] private float _maxLifeTimeAfterCollision;

    private bool _isSelfDestroyActive;

    public event UnityAction<Cube> Disappeared;
    
    private void OnEnable()
    {
        _isSelfDestroyActive = false;
    }

    private void Update()
    {
        if (_isDistanceChecking)
        {
            Vector3 worldCenter = Vector3.zero;

            if (Vector3.Distance(worldCenter, transform.position) > _maxDistance)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isSelfDestroyActive == false && collision.gameObject.TryGetComponent(out Platform platform))
        {
            ChangeColorToRandom();
            StartCoroutine(ActiveSelfDestroyMechanism());
        }
    }

    private IEnumerator ActiveSelfDestroyMechanism()
    {
        _isSelfDestroyActive = true;
        yield return new WaitForSeconds(Random.Range(_minLifeTimeAfterCollision, _maxLifeTimeAfterCollision));
        Disappeared?.Invoke(this);
    }

    private void ChangeColorToRandom()
    {
        Renderer renderer = GetComponent<Renderer>();

        renderer.material.color = Randomizer.GetColor();
    }
}
