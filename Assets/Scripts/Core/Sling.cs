using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Sling : MonoBehaviour
{
    [FormerlySerializedAs("AllHumans")] [SerializeField] private List<Food> AllFoods;
    [FormerlySerializedAs("_humanPos")] [SerializeField] private Transform _foodPos;
    [SerializeField] private Trajectory _trajectory;
    [SerializeField] private float _pushForce;
    [SerializeField] private float _maxForce;
    [SerializeField, Range(0f, 10f)] protected float _zMultiplier;

    private Vector2 _startPos, _endPos;
    private Vector3 _forcevector;
    private Vector3 _slingBandFirstPos;
    private Food _food;
    private bool isLevelStart;
    private Camera _camera;
    private GameManager _gameManager;

    private void OnEnable() => EventManager.OnLevelStart.AddListener(() => isLevelStart = true);
    
    private void OnDisable() => EventManager.OnLevelStart.RemoveListener(() => isLevelStart = true);
    

    private void Start()
    {
        _camera = Camera.main;
        _gameManager = GameManager.Instance;
        _slingBandFirstPos = transform.localPosition;
        _food = _foodPos.GetComponentInChildren<Food>();
        NewFoodGettingPosition();
    }

    private void Update()
    {
        if (!isLevelStart) return;
        if(!_gameManager.canSling) return;
        ControlSwipe();
    }

    private void ControlSwipe()
    {
        if (!_food) return;
        if (Input.GetMouseButtonDown(0))
        {
            _trajectory.Show();
            _startPos = _camera.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            _endPos = _camera.ScreenToViewportPoint(Input.mousePosition);

            Vector3 direction = (_startPos - _endPos).normalized;
            var distance = Vector2.Distance(_startPos, _endPos) / 30;
            var transform1 = transform;
            transform1.localPosition = new Vector3(_slingBandFirstPos.x + (_startPos.y - _endPos.y) / 10,
                transform1.localPosition.y, -(_startPos.x - _endPos.x) / 10);
            _forcevector = direction * (distance * _pushForce);
            _forcevector.z = _forcevector.y * _zMultiplier + 1f;
            _forcevector = Vector3.ClampMagnitude(_forcevector, _maxForce);

            _trajectory.UpdateDots(transform.position, _forcevector);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _food.transform.SetParent(null);
            _food.Push(_forcevector);
            _food = null;
            transform.localPosition = _slingBandFirstPos;
            Invoke(nameof(NewFoodGettingPosition), 0.5f);
            EventManager.OnFoodThrowed.Invoke();

            _trajectory.Hide();
        }
    }

    public void NewFoodGettingPosition()
    {
        if (_foodPos.childCount > 0 || AllFoods.Count == 0) return;

        _food = AllFoods[^1];
        AllFoods.Remove(_food);

        _food.transform.DOJump(_foodPos.position, 0.1f, 1, 0.5f)
            .OnComplete(() => { _food.transform.SetParent(_foodPos); });
    }
}