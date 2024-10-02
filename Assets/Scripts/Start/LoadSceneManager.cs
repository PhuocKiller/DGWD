using Fusion;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : NetworkSceneManagerBase
{
    protected override IEnumerator SwitchScene(SceneRef prevScene, SceneRef newScene, FinishedLoadingDelegate finished)
    {
        FindObjectOfType<GameManager>().SwitchState(GameState.Transition);
        Singleton<Loading>.Instance.ShowLoading();
        yield return SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);
        yield return null;
         Scene currentScene = SceneManager.GetSceneByBuildIndex(newScene);
        List<NetworkObject> networkObjects = FindNetworkObjects(currentScene,false);
        IEnumerable<NetworkObject> IenumrableNO = networkObjects;
        yield return null;
        finished(IenumrableNO);
        yield return new WaitForSeconds(1);
        FindObjectOfType<GameManager>().SwitchState(GameState.Ingame);
        Singleton<Loading>.Instance.HideLoading();

    }
}
