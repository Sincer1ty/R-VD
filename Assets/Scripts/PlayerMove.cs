using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private Actions actions;

    private Vector3 moveDirection;
    public float moveSpeed = 4f;

    private void OnEnable()
    {
        actions = InputManager.actions;

        actions.PlayerActions.Move.started += DoMove;
        actions.PlayerActions.Move.canceled += DoMove;
        actions.PlayerActions.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        bool hasControl = (moveDirection != Vector3.zero);
        //Debug.Log("moveDirection : " + moveDirection);
        if (hasControl)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    void DoMove(InputAction.CallbackContext obj)
    {
        Vector2 input = obj.ReadValue<Vector2>();
        if (input != null)
        {
            moveDirection = new Vector3(input.x, 0f, input.y);
            Debug.Log($"SEND_MESSAGE : {input.magnitude}");
        }
    }

    //private void FixedUpdate()
    //{
    //    Debug.Log("Move Values " + actions.PlayerActions.Move.ReadValue<Vector2>());
    //}
}
