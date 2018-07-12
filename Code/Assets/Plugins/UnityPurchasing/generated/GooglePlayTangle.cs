#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("kz5DA8eU+l8qRhWPkpVQrFhyijWyAIOgso+Ei6gEygR1j4ODg4eCgTbUnhkRnD/L15CrlUDerVw5OJCZNikc6Xefsrp2z2LXVnBXtv3YG3TK56Z4cf6pbIVyFqPF+m8RJmo7+bm/EzePETNjAz8KugPIbEEp44TIvi4lhDMSE5CAbDFeBE0LgnIWeDn8b+D85JbKCqhmIbA2zj3Y4n84iACDjYKyAIOIgACDg4IfNTTTTGgaDlCMc3ZyPvOxlUnNltGLi97MSAfueQT/7objEkfSoV4YM+L9qFHb5zCt0zfLz8CpNgBSosUGki9/LDscWNax4r+JVzx0fe1G8TRdf4wbC0OZeW4EVIcjjdehjPrx+EgtGBmL9L7jlH+kLZq8X4CBg4KD");
        private static int[] order = new int[] { 13,13,7,7,6,9,12,10,13,12,12,11,12,13,14 };
        private static int key = 130;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
