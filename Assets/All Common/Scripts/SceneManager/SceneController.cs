using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace TBGame
{
    public class SceneController : MonoBehaviour
    {


        public void LoadGameScene()
        {
            SceneManager.LoadScene(1);
        }

       
    }
}
