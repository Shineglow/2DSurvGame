using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Gameplay
{
    public class AssetsLoader
    {
        public static async UniTask<IList<T>> LoadAll<T>(string key)
        {
            var handle = Addressables.LoadAssetsAsync<T>(key, null);
            await handle.ToUniTask();
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }
            return null;
        }
    }
}