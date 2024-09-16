using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerScene : NetworkBehaviour
{
  public void PlayGameScene()
    {
        SceneManager.LoadScene("Play");
    }
}
