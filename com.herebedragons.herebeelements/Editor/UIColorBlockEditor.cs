using HereBeElements.Internal;
using UnityEditor;
using UnityEditor.UI;

namespace HereBeElements.Editor
{
    [CustomPropertyDrawer(typeof(UIColorBlock), true)]
    public class UIColorBlockEditor: ColorBlockDrawer
    {
    }
}