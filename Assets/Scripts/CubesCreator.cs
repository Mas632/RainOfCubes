using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class CubesCreator : MonoBehaviour
{
    [Header("��������� �������� �����")]
    [Tooltip("������ ��� ����������� �����")]
    [SerializeField] private Cube _prototype;
    [Tooltip("������� ������������, � ������� ����� ����������� ����")]
    [SerializeField] private Transform _spawnArea;
    [Tooltip("������������� �������� ����� (���������� ����������� ����� � �������)")]
    [SerializeField, Min(1)] private float _cubesPerSecond;
    
    [Space(20)]
    [Header("��������� ���� ��������")]
    [Tooltip("����� ���� �� ���������")]
    [SerializeField, Min(1)] private int _poolDefaultCapacity;
    [Tooltip("������������ ������ ����")]
    [SerializeField, Min(1)] private int _poolMaxSize;

    private Color _cubeDefaultColor = Color.black;
    private ObjectPool<Cube> _pool;

    private void Start()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_prototype),
            actionOnGet: CustomizeCube,
            actionOnRelease: cube => cube.gameObject.SetActive(false),
            actionOnDestroy: cube => Destroy(cube.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolDefaultCapacity,
            maxSize: _poolMaxSize);
        
        StartCoroutine(MakeRainOfCubes());
    }

    private IEnumerator MakeRainOfCubes()
    {
        int oneSecond = 1;
        float timeBetweenCubesCreation = oneSecond / _cubesPerSecond;
        var wait = new WaitForSecondsRealtime(timeBetweenCubesCreation);

        while (true)
        {
            _pool.Get();
            yield return wait;
        }
    }

    private void CustomizeCube(Cube cube)
    {
        cube.transform.SetPositionAndRotation(Randomizer.GetPoint(_spawnArea), Quaternion.identity);

        if (cube.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.velocity = Vector3.zero;
        }

        if (cube.TryGetComponent(out MeshRenderer meshRenderer))
        {
            meshRenderer.material.color = _cubeDefaultColor;
        }

        cube.Disappeared += ReturnCubeToPool;
        cube.gameObject.SetActive(true);
    }

    private void ReturnCubeToPool(Cube cube)
    {
        cube.Disappeared -= ReturnCubeToPool;
        _pool.Release(cube);
    }
}
