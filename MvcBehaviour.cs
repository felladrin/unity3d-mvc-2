using UnityEngine;

public abstract class MvcBehaviour : MonoBehaviour
{
    private static MvcScene currentScene;

    protected static MvcScene CurrentScene
    {
        get
        {
            if (currentScene != null) return currentScene;

            currentScene = FindObjectOfType<MvcScene>();

            if (currentScene != null) return currentScene;

            Debug.LogWarning("[MVC Framework] " + typeof(MvcScene).Name + " script was not found attached to any " +
                             "GameObject in scene. We're creating a new GameObject for it, to avoid errors, " +
                             "but you should fix this. Please read the MVC Freamework Usage Instructions.");

            currentScene = new GameObject {name = typeof(MvcScene).Name}.AddComponent<MvcScene>();

            return currentScene;
        }
    }
}

public abstract class MvcModel : MvcBehaviour
{
}

public abstract class MvcView : MvcBehaviour
{
}

public abstract class MvcController : MvcBehaviour
{
}