using UnityEngine;
using UnityEngine.SceneManagement;

public static class MvcFramework
{
	/// <summary>
	/// Finds a specific Model on scene.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns>An MvcModel-derived object.</returns>
	public static T FindModel<T> () where T : ModelBehaviour
	{
		return FindMvcBehaviorOfType<T> ();
	}

	/// <summary>
	/// Finds a specific View on scene.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns>A MvcView-derived object.</returns>
	public static T FindView<T> () where T : ViewBehaviour
	{
		return FindMvcBehaviorOfType<T> ();
	}

	/// <summary>
	/// Finds a specific Controller on scene.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns>A MvcController-derived object.</returns>
	public static T FindController<T> () where T : ControllerBehaviour
	{
		return FindMvcBehaviorOfType<T> ();
	}

	/// <summary>
	/// Finds all Models of an specific type on scene.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns>An array of MvcModel-derived objects.</returns>
	public static T[] FindModels<T> () where T : ModelBehaviour
	{
		return FindMvcBehaviorsOfType<T> ();
	}

	/// <summary>
	/// Finds all Views of an specific type on scene.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns>An array of MvcView-derived objects.</returns>
	public static T[] FindViews<T> () where T : ViewBehaviour
	{
		return FindMvcBehaviorsOfType<T> ();
	}

	/// <summary>
	/// Finds all Controllers of an specific type on scene.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns>An array of MvcController-derived objects.</returns>
	public static T[] FindControllers<T> () where T : ControllerBehaviour
	{
		return FindMvcBehaviorsOfType<T> ();
	}

	/// <summary>
	/// Finds a specific Model on scene by the Name of its GameObject.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns>An MvcModel-derived object.</returns>
	public static T FindModelByName<T> (string gameObjectName) where T : ModelBehaviour
	{
		return FindMvcBehaviorOfTypeByName<T> (gameObjectName);
	}

	/// <summary>
	/// Finds a specific View on scene by the Name of its GameObject.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns>A MvcView-derived object.</returns>
	public static T FindViewByName<T> (string gameObjectName) where T : ViewBehaviour
	{
		return FindMvcBehaviorOfTypeByName<T> (gameObjectName);
	}

	/// <summary>
	/// Finds a specific Controller on scene by the Name of its GameObject.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns>A MvcController-derived object.</returns>
	public static T FindControllerByName<T> (string gameObjectName) where T : ControllerBehaviour
	{
		return FindMvcBehaviorOfTypeByName<T> (gameObjectName);
	}

	static T FindMvcBehaviorOfType<T> () where T : MonoBehaviour
	{
		var typeName = typeof(T).Name;

		var mvcBehavior = Object.FindObjectOfType<T> ();

		if (mvcBehavior == null) {
			Debug.LogWarning ("[MVC Framework] A script is trying to access " + typeName + " but no GameObjet in " +
			"scene has this script attached. We're creating a new GameObject for it, to avoid " +
			"errors, but you should fix this. Please read the MVC Freamework Usage Instructions.");

			var newGameObject = new GameObject { name = typeName };
			SceneManager.MoveGameObjectToScene (newGameObject, SceneManager.GetActiveScene ());
			mvcBehavior = newGameObject.AddComponent<T> ();
		}

		return mvcBehavior;
	}

	static T[] FindMvcBehaviorsOfType<T> () where T : MonoBehaviour
	{
		var typeName = typeof(T).Name;

		var mvcBehaviorsFound = Object.FindObjectsOfType<T> ();

		if (mvcBehaviorsFound.Length > 0) {
			return mvcBehaviorsFound;
		}

		Debug.LogWarning ("[MVC Framework] A script is trying to access " + typeName + " but no GameObjet in " +
		"scene has this script attached. We're creating a new GameObject for it, to avoid " +
		"errors, but you should fix this. Please read the MVC Freamework Usage Instructions.");

		var newGameObject = new GameObject { name = typeName };
		SceneManager.MoveGameObjectToScene (newGameObject, SceneManager.GetActiveScene ());
		mvcBehaviorsFound [0] = newGameObject.AddComponent<T> ();

		return mvcBehaviorsFound;
	}

	static T FindMvcBehaviorOfTypeByName<T> (string gameObjectName) where T : MonoBehaviour
	{
		var typeName = typeof(T).Name;

		var mvcBehaviorsFound = Object.FindObjectsOfType<T> ();

		if (mvcBehaviorsFound.Length > 0) {
			foreach (var mvcBehavior in mvcBehaviorsFound) {
				if (mvcBehavior.gameObject.name == gameObjectName) {
					return mvcBehavior;
				}
			}
		}

		Debug.LogWarning ("[MVC Framework] A script is trying to access " + typeName + " but no GameObjet in " +
		"scene has this script attached. We're creating a new GameObject for it, to avoid " +
		"errors, but you should fix this. Please read the MVC Freamework Usage Instructions.");

		var newGameObject = new GameObject { name = gameObjectName };
		SceneManager.MoveGameObjectToScene (newGameObject, SceneManager.GetActiveScene ());
		var newMvcBehavior = newGameObject.AddComponent<T> ();

		return newMvcBehavior;
	}

	public abstract class ModelBehaviour : MonoBehaviour
	{
	}

	public abstract class ViewBehaviour : MonoBehaviour
	{
	}

	public abstract class ControllerBehaviour : MonoBehaviour
	{
	}
}