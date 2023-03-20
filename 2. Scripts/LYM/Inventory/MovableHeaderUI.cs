using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 마우스포인터 관련 인터페이스를 상속받기위해 네임스페이스 사용
using UnityEngine.EventSystems;

// 유니티에는 친절하게도 드래그앤드롭을 할수 있게 인벤토리 API가 존재한다. (ex 클릭유지, 클릭을 뗄 경우...)
public class MovableHeaderUI : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField]
    private Transform _targetTr; // 이동될 UI

    private Vector2 _beginPoint;
    private Vector2 _moveBegin;

    private void Awake()
    {
        // 이동 대상 UI를 지정하지 않은 경우, 자동으로 부모로 초기화
        if (_targetTr == null)
            _targetTr = transform.parent;
    }

    // 드래그 시작 위치로 마우스버튼을 누르면 이벤트가 발생해 처음 마우스 위치를 기억한다
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        _beginPoint = _targetTr.position;
        _moveBegin = eventData.position;
    }

    // 드래그 : 마우스 커서 위치로 이동
    void IDragHandler.OnDrag(PointerEventData eventData)
    {                                // 일종의 오프셋을 시작위치에서 더해줌
        _targetTr.position = _beginPoint + (eventData.position - _moveBegin);
    }
}
