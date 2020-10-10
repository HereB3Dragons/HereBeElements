using Internal;
using UnityEditor;
using UnityEditor.UI;

namespace com.herebedragons.herebeelements.Editor
{
    [CustomPropertyDrawer(typeof(UIColorBlock), true)]
    public class UIColorBlockEditor: ColorBlockDrawer
    {
    }
}