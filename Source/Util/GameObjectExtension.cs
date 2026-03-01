using UnityEngine;

namespace SkillsPlusPlus
{

    /// <summary>
    /// Defines some helpers for unity's <see cref="UnityEngine.GameObject"/>
    /// </summary>
    public static class GameObjectExtension
    {

        /// <summary>
        /// <see href="https://docs.unity3d.com/ScriptReference/GameObject.TryGetComponent.html"/>
        /// </summary>
        public static bool TryGetComponent<T>(this GameObject gameObject, out T component) where T : Component
        {
            component = gameObject.GetComponent<T>();
            return component != null;
        }

        /// <summary>
        /// <see href="https://docs.unity3d.com/ScriptReference/GameObject.TryGetComponent.html"/>
        /// </summary>
        public static bool TryGetComponentInChildren<T>(this GameObject gameObject, out T component) where T : Component
        {
            component = gameObject.GetComponentInChildren<T>();
            return component != null;
        }

        public static T EnsureComponent<T>(this GameObject thisGameObject) where T : Component
        {
            if (thisGameObject.TryGetComponent(out T existingComponent))
            {
                return existingComponent;
            }
            return thisGameObject.AddComponent<T>();
        }

    }

    /// <summary>
    /// Defines some helpers for unity's <see cref="UnityEngine.Component"/>
    /// </summary>
    public static class ComponentExtension
    {

        /// <summary>
        /// <see href="https://docs.unity3d.com/ScriptReference/Component.TryGetComponent.html"/>
        /// </summary>
        public static bool TryGetComponent<T>(this Component thisComponent, out T component) where T : Component
        {
            component = thisComponent.GetComponent<T>();
            return component != null;
        }

        /// <summary>
        /// <see href="https://docs.unity3d.com/ScriptReference/Component.TryGetComponent.html"/>
        /// </summary>
        public static bool TryGetComponentInChildren<T>(this Component thisComponent, out T component) where T : Component
        {
            component = thisComponent.GetComponentInChildren<T>();
            return component != null;
        }

        public static T EnsureComponent<T>(this Component thisComponent) where T : Component
        {
            if (thisComponent.TryGetComponent(out T existingComponent))
            {
                return existingComponent;
            }
            return thisComponent.gameObject.AddComponent<T>();
        }

    }
}
