using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Application.Preloader
{ 
    //TODO: rename to BootstrapScene
    public class Preloader : MonoBehaviour
    {
        void Start()
        {
            // SceneManager.LoadSceneAsync("RootScene");

            //TODO: preload any graphics here?
            
            Addressables.LoadSceneAsync("RootScene").Completed += op =>
            {
                Debug.Log("Preloader: loaded RootScene");
            };
        }
    }
}
