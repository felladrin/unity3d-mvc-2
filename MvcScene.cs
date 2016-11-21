using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MvcScene : MonoBehaviour
{
    private Dictionary<string, UnityEvent> eventDictionary = new Dictionary<string, UnityEvent>();
    private Dictionary<string, object> singleInstanceMvcBehaviors = new Dictionary<string, object>();

    /// <summary>
    /// Finds a specific Model on scene.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>An MvcModel-derived object.</returns>
    public T Model<T>() where T : MvcModel
    {
        return FindMvcBehaviorOfType<T>();
    }

    /// <summary>
    /// Finds a specific View on scene.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>A MvcView-derived object.</returns>
    public T View<T>() where T : MvcView
    {
        return FindMvcBehaviorOfType<T>();
    }

    /// <summary>
    /// Finds a specific Controller on scene.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>A MvcController-derived object.</returns>
    public T Controller<T>() where T : MvcController
    {
        return FindMvcBehaviorOfType<T>();
    }

    /// <summary>
    /// Finds all Models of an specific type on scene.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>An array of MvcModel-derived objects.</returns>
    public T[] Models<T>() where T : MvcModel
    {
        return FindMvcBehaviorsOfType<T>();
    }

    /// <summary>
    /// Finds all Views of an specific type on scene.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>An array of MvcView-derived objects.</returns>
    public T[] Views<T>() where T : MvcView
    {
        return FindMvcBehaviorsOfType<T>();
    }

    /// <summary>
    /// Finds all Controllers of an specific type on scene.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>An array of MvcController-derived objects.</returns>
    public T[] Controllers<T>() where T : MvcController
    {
        return FindMvcBehaviorsOfType<T>();
    }

    /// <summary>
    /// Finds a specific Model on scene by the Name of its GameObject.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>An MvcModel-derived object.</returns>
    public T ModelByName<T>(string gameObjectName) where T : MvcModel
    {
        return FindMvcBehaviorOfTypeByName<T>(gameObjectName);
    }

    /// <summary>
    /// Finds a specific View on scene by the Name of its GameObject.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>A MvcView-derived object.</returns>
    public T ViewByName<T>(string gameObjectName) where T : MvcView
    {
        return FindMvcBehaviorOfTypeByName<T>(gameObjectName);
    }

    /// <summary>
    /// Finds a specific Controller on scene by the Name of its GameObject.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>A MvcController-derived object.</returns>
    public T ControllerByName<T>(string gameObjectName) where T : MvcController
    {
        return FindMvcBehaviorOfTypeByName<T>(gameObjectName);
    }

    private T FindMvcBehaviorOfType<T>() where T : MvcBehaviour
    {
        object mvcBehavior;
        var typeName = typeof(T).Name;

        if (singleInstanceMvcBehaviors.TryGetValue(typeName, out mvcBehavior))
        {
            return (T) mvcBehavior;
        }

        mvcBehavior = GetComponentInChildren<T>();

        if (mvcBehavior == null)
        {
            Debug.LogWarning("[MVC Framework] A script is trying to access " + typeName + " but no GameObjet in " +
                             "scene has this script attached. We're creating a new GameObject for it, to avoid " +
                             "errors, but you should fix this. Please read the MVC Freamework Usage Instructions.");

            var newGameObject = new GameObject {name = typeName};
            newGameObject.transform.SetParent(transform);
            mvcBehavior = newGameObject.AddComponent<T>();
        }

        singleInstanceMvcBehaviors.Add(typeName, mvcBehavior);

        return (T) mvcBehavior;
    }

    private T[] FindMvcBehaviorsOfType<T>() where T : MvcBehaviour
    {
        var typeName = typeof(T).Name;

        var mvcBehaviorsFound = GetComponentsInChildren<T>();

        if (mvcBehaviorsFound.Length != 0)
        {
            return mvcBehaviorsFound;
        }

        Debug.LogWarning("[MVC Framework] A script is trying to access " + typeName + " but no GameObjet in " +
                         "scene has this script attached. We're creating a new GameObject for it, to avoid " +
                         "errors, but you should fix this. Please read the MVC Freamework Usage Instructions.");

        var newGameObject = new GameObject {name = typeName};
        newGameObject.transform.SetParent(transform);
        mvcBehaviorsFound[0] = newGameObject.AddComponent<T>();

        return mvcBehaviorsFound;
    }

    private T FindMvcBehaviorOfTypeByName<T>(string gameObjectName) where T : MvcBehaviour
    {
        var typeName = typeof(T).Name;

        var mvcBehaviorsFound = GetComponentsInChildren<T>();

        if (mvcBehaviorsFound.Length != 0)
        {
            foreach (var mvcBehavior in mvcBehaviorsFound)
            {
                if (mvcBehavior.gameObject.name == gameObjectName)
                {
                    return mvcBehavior;
                }
            }
        }

        Debug.LogWarning("[MVC Framework] A script is trying to access " + typeName + " but no GameObjet in " +
                         "scene has this script attached. We're creating a new GameObject for it, to avoid " +
                         "errors, but you should fix this. Please read the MVC Freamework Usage Instructions.");

        var newGameObject = new GameObject {name = gameObjectName};
        newGameObject.transform.SetParent(transform);
        var newMvcBehavior = newGameObject.AddComponent<T>();

        return newMvcBehavior;
    }

    /// <summary>
    /// Add listener to a given event.
    /// Usage: Call it inside OnEnable() method of MonoBehaviours.
    /// </summary>
    /// <param name="eventName">Name of the event.</param>
    /// <param name="listener">Callback function.</param>
    public void AddEventListener(string eventName, UnityAction listener)
    {
        UnityEvent unityEvent;
        if (eventDictionary.TryGetValue(eventName, out unityEvent))
        {
            unityEvent.AddListener(listener);
        }
        else
        {
            unityEvent = new UnityEvent();
            unityEvent.AddListener(listener);
            eventDictionary.Add(eventName, unityEvent);
        }
    }

    /// <summary>
    /// Remove listener from a given event.
    /// Usage: Call it inside OnDisable() method of MonoBehaviours.
    /// </summary>
    /// <param name="eventName">Name of the event.</param>
    /// <param name="listener">Callback function.</param>
    public void RemoveEventListener(string eventName, UnityAction listener)
    {
        UnityEvent unityEvent;
        if (eventDictionary.TryGetValue(eventName, out unityEvent))
        {
            unityEvent.RemoveListener(listener);
        }
    }

    /// <summary>
    /// Triggers all registered callbacks of a given event.
    /// </summary>
    public void TriggerEvent(string eventName)
    {
        UnityEvent unityEvent;
        if (eventDictionary.TryGetValue(eventName, out unityEvent))
        {
            unityEvent.Invoke();
        }
    }
}