using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private TouchManager _touchManager = NullObject.TouchManager;
    [SerializeField] private float _movementDuration = 1f; // Duration it takes to move one unit.
    [SerializeField] private PlayerVisualChangeManager _playerVisualChangeManager = NullObject.PlayerVisualChangeManager;

    public delegate void MovementEndHandler();
    public event MovementEndHandler OnMovementEnd;

    public PredefinedColor Color { get; private set; }

    public Vector2 LocalPosition
    {
        get
        {
            return transform.localPosition;
        }

        set
        {
            transform.localPosition = value;
        }
    }


    private bool _moving = false;

    private void Awake()
    {
        _playerVisualChangeManager.InitializeVisuals(PredefinedColor.AquaMarine);
    }

    public void Move(float x = 0, float y = 0)
    {
        if (_moving) return;

        _moving = true;

        StartCoroutine(MoveCoroutine(x, y));
    }

    public void SetColor(PredefinedColor predefinedColor)
    {
        _playerVisualChangeManager.ChangeColor(predefinedColor);
        Color = predefinedColor;
    }

    private IEnumerator MoveCoroutine(float x, float y)
    {
        var step = 0f;
        var initialPosition = LocalPosition;
        var endPosition = LocalPosition + new Vector2(x, y);

        while (step < 1f)
        {
            step += Time.deltaTime / _movementDuration;
            step = (step > 1f) ? 1f : step;

            LocalPosition = Vector2.Lerp(initialPosition, endPosition, step);

            yield return 0;
        }

        LocalPosition = endPosition;

        OnMovementEnd?.Invoke();

        _moving = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SetColor(PredefinedColor.Blue);
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SetColor(PredefinedColor.Tangerine);
        }
    }
}
