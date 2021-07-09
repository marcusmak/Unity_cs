using Unity.WebRTC;

namespace Local.WebRTC{
    internal static class WebRTCSettings
    {
        private static bool s_enableHWCodec = false;
        private static bool s_limitTextureSize = true;

        public static bool EnableHWCodec
        {
            get { return s_enableHWCodec; }
            set { s_enableHWCodec = value; }
        }

        public static bool LimitTextureSize
        {
            get { return s_limitTextureSize; }
            set { s_limitTextureSize = value; }
        }

        public static EncoderType EncoderType
        {
            get { return s_enableHWCodec ? EncoderType.Hardware : EncoderType.Software; }
        }
    }

}