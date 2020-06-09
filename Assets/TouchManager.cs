using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    [SerializeField] private List<RectTransform> _touchBlockers = NullObject.RectTransformList;
    
    private static readonly Camera _emptyCamera = new Camera();
    [SerializeField] private Camera _mainCamera = _emptyCamera;

    public delegate void DragLeftHandler();
    public delegate void DragRightHandler();
    public delegate void DragUpHandler();
    public delegate void DragDownHandler();

    public event DragLeftHandler OnDraggedLeft;
    public event DragRightHandler OnDraggedRight;
    public event DragUpHandler OnDraggedUp;
    public event DragDownHandler OnDraggedDown;

    private Vector2 _touchStartPos = Vector2.zero;
    private Vector2 _touchEndPos = Vector2.zero;
    private bool _touchIsRegistered = false;

    private void DetectTouch()
    {
        var touchPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            _touchStartPos = touchPosition;
        }

        else if (Input.GetMouseButtonUp(0))
        {
            _touchIsRegistered = true;
            _touchEndPos = touchPosition;
        }

        else if (Input.GetMouseButton(0))
        {
            // Do Nothing.
        }

        else
        {
            _touchIsRegistered = false;
        }
    }

    private void ProcessTouch()
    {
        if (!_touchIsRegistered)
            return;

        var direction = (_touchEndPos - _touchStartPos).normalized;

        if (direction == Vector2.zero) // This is just a click or touch.
            return;

        if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
                OnDraggedRight?.Invoke(); // Debug.Log("Direction is right."); 
            else
                OnDraggedLeft?.Invoke(); // Debug.Log("Direction is left.");
        }

        else 
        {
            if (direction.y > 0)
                OnDraggedUp?.Invoke(); // Debug.Log("Direction is up.");
            else
                OnDraggedDown?.Invoke(); // Debug.Log("Direction is down.");
        }

    }

    private void Update()
    {
        DetectTouch();
        ProcessTouch();
    }
}
