using UnityEngine;

namespace FG_GP2_T3
{
    public class UIAudioManager : MonoBehaviour
    {
        public void OnDefaultButton()
        {
            GlobalSoundManager.Instance.OnPlaySound(GlobalSoundType.ButtonDefault);
        }
        
        public void OnExpand()
        {
            GlobalSoundManager.Instance.OnPlaySound(GlobalSoundType.TowerInfoExpand);
        }
        
        public void OnCollapse()
        {
            GlobalSoundManager.Instance.OnPlaySound(GlobalSoundType.TowerInfoCollapse);
        }
        
        public void OnSpeedUpButton()
        {
            GlobalSoundManager.Instance.OnPlaySound(GlobalSoundType.ChangeSpeed);
        }
    }
}
