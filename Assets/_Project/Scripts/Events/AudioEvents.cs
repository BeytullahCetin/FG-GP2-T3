namespace FG_GP2_T3
{
    public enum VolumeType
    {
        VolumeMain,
        VolumeMusic,
        VolumeAmbience,
        VolumeSfxAuto,
        VolumeSfxInteraction,
        VolumeNarration,
        VolumeUi,
    }

    public class OnVolumeEvent : GameEventArgs
    {
        public readonly float Volume01;
        public readonly VolumeType VolumeType;

        public OnVolumeEvent(float volume01, VolumeType volumeType)
        {
            Volume01 = volume01;
            VolumeType = volumeType;
        }
    }
}