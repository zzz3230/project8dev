using UnityEngine;

public static class StInput
{
    public static bool isTextFieldEditing;

    public static bool GetKeyDown(KeyCode key)
    {
        return !isTextFieldEditing && Input.GetKeyDown(key);
    }
    public static bool GetButtonDown(string name)
    {
        return !isTextFieldEditing && Input.GetButtonDown(name);
    }
    public static float GetAxis(string name)
    {
        return !isTextFieldEditing ? Input.GetAxis(name) : 0f;
    }
}
