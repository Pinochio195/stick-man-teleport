using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(HeaderTextColorAttribute))]
public class HeaderTextColorDecorator : DecoratorDrawer
{
    private GUIStyle headerStyle;
    private bool initialized;
    private void InitGUIStyle()
    {
        headerStyle = new GUIStyle(GUI.skin.label);
        headerStyle.fontStyle = FontStyle.Bold;
        headerStyle.normal.textColor = ((HeaderTextColorAttribute)attribute).color;
        initialized = true;
    }

    public override float GetHeight()
    {
        if (!initialized)
        {
            InitGUIStyle();
        }

        return EditorGUIUtility.singleLineHeight * 2;
    }

    public override void OnGUI(Rect position)
    {
        if (!initialized)
        {
            InitGUIStyle();
        }

        HeaderTextColorAttribute attribute = (HeaderTextColorAttribute)this.attribute;
        string headerText = attribute.headerText;

        Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth + 200, 50);
        EditorGUI.LabelField(labelRect, headerText, headerStyle);
    }
}
#endif

public class HeaderTextColorAttribute : PropertyAttribute
{
    public Color color;
    public string headerText;

    public HeaderTextColorAttribute(float r, float g, float b, float a = 1.0f, string headerText = "")
    {
        color = new Color(r, g, b, a);
        this.headerText = headerText;
    }
}

