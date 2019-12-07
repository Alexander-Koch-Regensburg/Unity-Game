using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Notice: Consider MonoBehaviour.OnMouseDrag() for future features
public class MouseButton : MonoBehaviour
{
    private int mouseButtonId;
    private MouseButtonState state = MouseButtonState.NONE;
    private float mouseButtonDownTimeStamp;
    private float minPressedTime = 0.2f;
    private bool active = false;

    public void Setup(int mouseButtonId, float minPressedTime)
    {
        this.mouseButtonId = mouseButtonId;
        this.minPressedTime = minPressedTime;
    }

    private void Update()
    {
        //DetermineMouseButtonStateIntern();
    }

    public void DetermineMouseButtonState()
    {
        DetermineMouseButtonStateIntern();
    }

    private void DetermineMouseButtonStateIntern()
    {
        float deltaTime;

        if (state == MouseButtonState.CLICKED)
            state = MouseButtonState.UP;

        // A click must be exactly one time frame, so after setting this state it has to be set to NONE at the next time frame
        else if (state == MouseButtonState.DOWN)
            state = MouseButtonState.NONE;

        else if (Input.GetMouseButtonDown(mouseButtonId))
        {
            state = MouseButtonState.DOWN;
            mouseButtonDownTimeStamp = Time.time;
            active = true;
        }

        else if (Input.GetMouseButtonUp(mouseButtonId))
        {
            deltaTime = Time.time - mouseButtonDownTimeStamp;
            if (deltaTime < minPressedTime)
                state = MouseButtonState.CLICKED;
            else
                state = MouseButtonState.UP;

            active = false;
        }
        else if (active)
        {
            deltaTime = Time.time - mouseButtonDownTimeStamp;
            if (deltaTime >= minPressedTime)
                state = MouseButtonState.PRESSED;
        }
        else if (!active)
        {
            state = MouseButtonState.NONE;
        }
    }

    public MouseButtonState GetState()
    {
        return state;
    }

    public void SetState(MouseButtonState state)
    {
        this.state = state; ;
    }

    public bool IsActive()
    {
        return active;
    }
}
