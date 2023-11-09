using Cinemachine;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    #region Variables 

    // Ready to have variables

    #endregion

    #region Methods
    private void Awake()
    {
        #region Checking if all needed object exists
        // Checking if there is a GameObject named CameraTarget
        Debug.Assert(GameObject.Find("CameraTarget"), "There must be a GameObject named 'CameraTarget' in the Scene");

        // Checking if there is GameObject named CameraColliderConfiner
        Debug.Assert(GameObject.Find("CameraColliderConfiner"), "There must be a GameObject named 'CameraColliderConfiner' in the Scene");
        #endregion
    }
    #endregion
}