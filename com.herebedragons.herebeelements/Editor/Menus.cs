using System;
using HereBeElements;
using HereBeElements.Materials;
using HereBeElements.Templates;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace com.herebedragons.herebeelements.Editor
{
    public class Menus
    {

        [MenuItem("GameObject/HereBeElements/UI/Image", false, 1)]
        static void CreateUIImage(MenuCommand menuCommand)
        {
            GameObject parent = ValidateCanvas(menuCommand);
            // Create a custom game object
            GameObject go = new GameObject("UIImage");
            go.AddComponent<UIImage>();
            DefineAsUIElement(go, parent);
        }
        
        [MenuItem("GameObject/HereBeElements/UI/Text", false, 1)]
        static void CreateUIText(MenuCommand menuCommand)
        {
            GameObject parent = ValidateCanvas(menuCommand);
            // Create a custom game object
            GameObject go = new GameObject("UIText");
            go.AddComponent<Text>();
            DefineAsUIElement(go, parent);
        }


        private static void DefineAsUIElement(GameObject go, GameObject parent)
        {
            go.AddComponent<UIElement>();
            go.GetComponent<Graphic>().material = MaterialFactory.UIElementMaterial;
            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(go, parent);
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }


        private static GameObject ValidateCanvas(MenuCommand menuCommand)
        {
            GameObject go = menuCommand.context as GameObject;
            if (go == null)
            {
                Canvas c = Object.FindObjectOfType<Canvas>();
                if (c != null)
                    go = c.gameObject;
            }
            if (go == null || !IsChildOfCanvas(go))
                throw new ArgumentException("UI Elements need to be children of Canvas Objects");
            return go;
        }
        
        private static bool IsChildOfCanvas(GameObject go)
        {
            if (go.GetComponent<Canvas>() != null) return true;
            Canvas[] c = go.GetComponentsInParent<Canvas>();
            return c != null && c.Length > 0;
        }
    }
}