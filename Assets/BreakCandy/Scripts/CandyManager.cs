using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Touch;
using pooling;
using UnityEngine.EventSystems;

namespace BreakCandy.Scripts
{
    public class CandyManager : MonoBehaviour
    {
        [SerializeField] private GameObject dotObjectPrefab;
        [SerializeField] private Vector3 oldDot;
        
        [SerializeField] private Transform stick;
        [SerializeField] private Transform dotCheck;
        private Vector3 lastPosition;
        
        private bool isDown;
        private bool onComplete = false;
        private int _amountFailDot;
        public float doLech = 0.5f;

        private void OnEnable()
        {
            LeanTouch.OnFingerDown += HandleFingerDown;
            LeanTouch.OnFingerUpdate+= HandleFingerSet;
            LeanTouch.OnFingerUpdate += HandleFingerUpdate;
            LeanTouch.OnFingerUp += HandleFingerUp;
        }

        private void OnDisable()
        {
            LeanTouch.OnFingerDown -= HandleFingerDown;
            LeanTouch.OnFingerUpdate-= HandleFingerSet;
            LeanTouch.OnFingerUpdate -= HandleFingerUpdate;
            LeanTouch.OnFingerUp -= HandleFingerUp;
        }
        private void HandleFingerUpdate(LeanFinger finger)
        {
            if(onComplete) return;
            if (IsPointerOverUIObject()) return;
            if (isDown)
            {
                Vector3 position = dotCheck.position;
                RaycastHit hit;
                float radius = 0.02f;
                Vector3 direction = Vector3.forward;
                Debug.DrawRay(position, direction * 1f, Color.yellow);
              
                if (Physics.SphereCast(position, radius, direction, out hit, 1f))
                {
                    if (hit.collider != null)
                    {
                        if (hit.collider.name.Contains("Main"))
                        {
                            SpawnDot(hit.transform.position);
                            Debug.Log("Losse");
                        }
                        else
                        {
                            SpawnDot(hit.transform.position);
                        }
                        Destroy(hit.collider.gameObject);
                    }   
                }
            }
        }
        private bool IsPointerOverUIObject()
        {
            var results = new List<RaycastResult>();
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
        private void HandleFingerDown(LeanFinger finger)
        {
            if (!onComplete)
            {
                Vector3 position = GetWorldPosition(finger.ScreenPosition);
                stick.position = new Vector3(position.x, position.y, stick.position.z);
                if (position.x > 1f)
                {
                    stick.localRotation = Quaternion.Euler(0, 0, 25f);
                }else if (position.x < -1f)
                {
                    stick.localRotation = Quaternion.Euler(0, 0, 45f);
                }
                else
                {
                    stick.localRotation = Quaternion.Euler(0, 0, 35f);
                }
                stick.DOKill();
                transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 0.5f).SetEase(Ease.Linear);
                stick.DOMoveY(position.y - 0.5f, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    isDown = true;  
                });
                lastPosition = GetWorldPosition(finger.ScreenPosition);
                Debug.Log((position));
                Debug.Log((stick.position));
            }
        }
        private void HandleFingerSet(LeanFinger finger)
        {
            if (isDown)
            {
                Vector3 currentPosition = GetWorldPosition(finger.ScreenPosition);
                
                Vector3 delta = currentPosition - lastPosition;
                
                stick.position += delta;
                
                lastPosition = currentPosition;
            }
        }
        private void HandleFingerUp(LeanFinger finger)
        {
            isDown = false;     
            Vector3 position = GetWorldPosition(finger.ScreenPosition);
            stick.DOKill();
            transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.Linear);
            stick.DOMoveY(position.y + 0.5f, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                isDown = false;     
            });
        }
        private Vector3 GetWorldPosition(Vector2 screenPosition)
        {
            Vector3 worldPosition = GameManager.instance.Cam.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, GameManager.instance.Cam.nearClipPlane));
            worldPosition.z = 0; 
            return worldPosition;
        }

        private void SpawnDot(Vector3 position)
        {
            PoolingManager.Spawn(GameManager.instance.effectBreak, position, Quaternion.identity, transform);
        }
    }

}