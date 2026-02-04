using UnityEngine;

namespace FG_GP2_T3
{
    /// <summary>
    /// Utlity class to detect towers under the mouse cursor.
    /// Returns the TowerBase if a tower is detected, otherwise returns null.
    /// </summary>
    public static class FusionDetector { 
        public static Camera Cam;
        public static TowerBase GetTowerUnderMouse()
        {
            if(Cam==null)
                Cam=Camera.main;

            Ray ray = Cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                    if(hitInfo.collider.CompareTag("Tower"))
                    {
                        return hitInfo.collider.GetComponent<TowerBase>();
                    }
            }
             return null;
                
                
                
        }
    }
}
