using System.Collections.Generic;
using BreakCandy.Scripts;
using DG.Tweening;
using Lean.Touch;
using pooling;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlStick : MonoBehaviour
{
    public Transform stick;
    private bool isDown;
    private Vector3 lastPosition;
    private void OnValidate()
    {
        stick = transform.GetChild(0);
        if (!stick.TryGetComponent<Stick>(out Stick s))
        { 
            stick.gameObject.AddComponent<Stick>();
        }
    }
    public void ChangeStick(Transform newStick)
    {
        Destroy(stick.gameObject);
        stick = PoolingManager.Spawn(newStick, new Vector3(0.2f, 0.3f, -0.5f), Quaternion.identity , transform);
        stick.localEulerAngles = new Vector3(stick.localEulerAngles.x, stick.localEulerAngles.y, -45f);
        if (!stick.TryGetComponent<Stick>(out Stick s))
        { 
            stick.gameObject.AddComponent<Stick>();
        }
    }
    private void OnEnable()
    {
        LeanTouch.OnFingerDown += HandleFingerDown;
        LeanTouch.OnFingerUpdate+= HandleFingerSet;
        LeanTouch.OnFingerUp += HandleFingerUp;
    }
    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= HandleFingerDown;
        LeanTouch.OnFingerUpdate-= HandleFingerSet;
        LeanTouch.OnFingerUp -= HandleFingerUp;
    }
    private bool IsPointerOverUIObject(Vector2 screenPosition)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };
    
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
    
        return results.Count > 0;
    }
     private void HandleFingerDown(LeanFinger finger)
        {
            if (IsPointerOverUIObject(finger.ScreenPosition)) return;
            Vector3 position = GetWorldPosition(finger.ScreenPosition);
            stick.DOKill();
            transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.Linear);
            stick.DOMove(new Vector3(position.x, 0.13f/12f, position.z + 0.025f) * 12f, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                isDown = true;  
            });
            lastPosition = GetWorldPosition(finger.ScreenPosition);
        }
        private void HandleFingerSet(LeanFinger finger)
        {
            if (isDown)
            {
                Vector3 currentPosition = GetWorldPosition(finger.ScreenPosition);
                
                Vector3 delta = currentPosition - lastPosition;
                
                stick.position += delta * 12f;
                
                lastPosition = currentPosition;
            }
        }
        private void HandleFingerUp(LeanFinger finger)
        {
            if (IsPointerOverUIObject(finger.ScreenPosition)) return;
            isDown = false;     
            stick.position = new Vector3(stick.position.x, 0.5f, stick.position.z);
            stick.DOKill();
            transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 0.5f).SetEase(Ease.Linear);
        }
        private Vector3 GetWorldPosition(Vector2 screenPosition)
        {
            Vector3 worldPosition = GameManager.instance.Cam.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, GameManager.instance.Cam.nearClipPlane));
            return worldPosition;
        }
}
