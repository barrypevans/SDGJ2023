using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
   public static T Instance;

   protected virtual void Awake()
   {
      if (Instance != null)
      {
         Debug.LogWarningFormat("MonoSingleton - preventing creation of duplicate instance: {0}, {1}", typeof(T), gameObject.name);
         Destroy(gameObject);
         return;
      }

      Instance = (T)this;
   }

   protected virtual void OnDestroy()
   {
      if (Instance == this)
      {
         Instance = null;
      }
   }
}