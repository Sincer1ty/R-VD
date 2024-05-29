using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Actions actions;

    private void Awake()
    {
        if (actions == null)
            actions = new Actions();
    }

    public static void ChangeInputEventOption(string actionName, int bindingIndex, string statusText)
    {
        Debug.Log("ChangeInputEventOption : " + actionName);
        InputAction inputAction = actions.asset.FindAction(actionName);
        if (inputAction == null || inputAction.bindings.Count <= bindingIndex)
        {
            Debug.Log("Couldn't find action or binding");
            return;
        }

        if (inputAction.bindings[bindingIndex].isComposite)
        {
            Debug.Log("isComposite");
            var firstPartIndex = bindingIndex + 1;
            if (firstPartIndex < inputAction.bindings.Count && inputAction.bindings[firstPartIndex].isComposite)
            {
                Debug.Log("DoChange for Composite");
                DoChange(inputAction, bindingIndex, statusText, true);
            }
        }
        else
        {
            Debug.Log("DoChange");
            DoChange(inputAction, bindingIndex, statusText, false);
        }
            
    }

    public static void DoChange(InputAction actionToChange, int bindingIndex, string statusText, bool allCompositeParts)
    {
        if (actionToChange == null)
            return;

        statusText = $"Press a {actionToChange.expectedControlType} for {actionToChange}";
        Debug.Log(statusText);

        actionToChange.Disable();

        var change = actionToChange.PerformInteractiveRebinding(bindingIndex);

        change.OnComplete(operation =>
        {
            actionToChange.Enable();
            operation.Dispose();

            if(allCompositeParts)
            {
                var nextBindingIndex = bindingIndex + 1;
                if (nextBindingIndex < actionToChange.bindings.Count && actionToChange.bindings[nextBindingIndex].isComposite)
                    DoChange(actionToChange, nextBindingIndex, statusText, allCompositeParts);
            }

            SaveBindingOverride(actionToChange);
        });

        change.OnCancel(operation =>
        {
            actionToChange.Enable();
            operation.Dispose();
        });

        change.Start();
    }

    private static void SaveBindingOverride(InputAction action)
    {
        for(int i=0; i<action.bindings.Count;i++)
        {
            PlayerPrefs.SetString(action.actionMap + action.name + i, action.bindings[i].overridePath);
        }
    }

    public static void LoadBindingOverride(string actionName)
    {
        if (actions == null)
            actions = new Actions();

        InputAction inputAction = actions.asset.FindAction(actionName);

        for(int i =0; i< inputAction.bindings.Count;i++)
        {
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString(inputAction.actionMap + inputAction.name + i)))
                inputAction.ApplyBindingOverride(i, PlayerPrefs.GetString(inputAction.actionMap + inputAction.name + i));
        }
    }
}
