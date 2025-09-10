using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickAreaDriver : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] Camera cam;
    [SerializeField] PinePie.SimpleJoystick.JoystickController joystick; // 씬에 미리 배치해 둔 조이스틱
    private int activePointerId = int.MinValue;

    public void OnPointerDown(PointerEventData e)
    {
        if (activePointerId != int.MinValue) return;       // 이미 사용 중이면 무시
        activePointerId = e.pointerId;

        joystick.ShowAtScreen(e.position, e.pressEventCamera ?? cam);
    }

    public void OnDrag(PointerEventData e)
    {
        if (e.pointerId != activePointerId) return;
        joystick.DragByScreen(e.position, e.pressEventCamera ?? cam);
    }

    public void OnPointerUp(PointerEventData e)
    {
        if (e.pointerId != activePointerId) return;
        joystick.Hide();
        activePointerId = int.MinValue;
    }
}
