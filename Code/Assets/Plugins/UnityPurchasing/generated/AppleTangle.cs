#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class AppleTangle
    {
        private static byte[] data = System.Convert.FromBase64String("+MYvH35WTeEbo+yWVyQ8Y5ytRJj36+Kn1ejo86fExreZkIq3sbeztenjp+To6ePu8+7o6fSn6OGn8vTij9m3BYaWgYTSmqeDBYaPtwWGg7fC+ZjL7NcRxg5D8+WMlwTGALQNBrqh4KcNtO1wigVIWWwkqH7U7dzjMJw6FMWjla1AiJoxyhvZ5E/MB5D36+KnxOL18+7h7uTm8+7o6afG8rSx3bfltoy3joGE0oOBlIXS1LaUgGv6vgQM1KdUv0M2OB3Ijex4rHuRt5OBhNKDhJSKxvf36+Kn1ejo8zlz9BxpVeOITP7Is18luX7/eOxP4AiPM6dwTCurp+j3MbiGtwswxEgSGf2LI8AM3FORsLRMQ4jKSZPuVpgCBAKcHrrAsHUuHMcJq1M2F5VfrQHPAXCKhoaCgoe35baMt46BhNKht6OBhNKDjJSaxvf36+KnxOL184+sgYaCgoCFhpGZ7/Pz9/S9qKjwCPQG50Gc3I6oFTV/w893578ZknKKgY6tAc8BcIqGhoKCh4QFhoaH2y9b+aWyTaJSXohR7FMlo6SWcCYrq6fk4vXz7uHu5Obz4qf36Ovu5P4yvSpziImHFYw2ppGp81K7ilzlkfPu4e7k5vPip+X+p+bp/qf35vXzzl/xGLST4ibwE06qhYSGh4YkBYZesfhGANJeIB4+tcV8X1L2Gfkm1QWGh4GOrQHPAXDk44KGtwZ1t62BiBq6dKzOr51PeUkyPole2ZtRTLqxHsuq/zBqCxxbdPAcdfFV8LfIRqfo4afz7+Kn8+/i6afm9/fr7uTmNrffa92DtQvvNAiaWeL0eODZ4jvr4qfO6eSptqG3o4GE0oOMlJrG99Xi6+7m6eTip+jpp/Pv7vSn5OL15evip/Tz5unj5vXjp/Pi9er0p+aDgZSF0tS2lLeWgYTSg42Ujcb396NlbFYw91iIwmagTXbq/2pgMpCQLCT2FcDU0kYoqMY0f3xk90phJMsMng5ZfszrcoAspbeFb5+5f9eOVOOypJLMkt6aNBNwcRsZSNc9Rt/XR+S08HC9gKvRbF2IpoldPfSeyDKpxyFwwMr4j9m3mIGE0pqkg5+3kai3BkSBj6yBhoKCgIWFtwYxnQY0mBZcmcDXbIJq2f4DqmyxJdDL0muCh4QFhoiHtwWGjYUFhoaHYxYujoG3iIGE0pqUhoZ4g4K3hIaGeLea7uHu5Obz7ujpp8by8+/o9e7z/ra3BYM8twWEJCeEhYaFhYaFt4qBjk6e9XLaiVL42Bx1ooQ90gjK2op29ebk8+7k4qf08+bz4uri6fP0qbcHk6xX7sAT8Y55c+wKqcchcMDK+P6n5vT08uri9Kfm5OTi9/Pm6eTip+bp46fk4vXz7uHu5Obz7ujpp/eBhNKaiYORg5OsV+7AE/GOeXPsCv23BYbxt4mBhNKaiIaGeIODhIWG8+/o9e7z/raRt5OBhNKDhJSKxvenxMa3BYalt4qBjq0BzwFwioaGhreWgYTSg42Ujcb39+vip87p5Km2srW2s7e0sd2QirSyt7W3vrW2s7fw8Knm9/fr4qnk6Oqo5vf36+Lk5t4ggo77kMfRlpnzVDAMpLzAJFLo1y0NUl1je1eOgLA38vKm");
        private static int[] order = new int[] { 17,6,43,33,25,56,59,25,8,51,12,50,21,18,49,59,21,35,55,47,54,44,23,39,27,58,34,39,48,41,34,33,38,42,43,53,39,44,54,57,42,44,59,49,44,50,53,47,53,54,59,59,54,53,56,55,59,58,58,59,60 };
        private static int key = 135;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
