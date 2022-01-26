// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("weQrjZQTd0Ktel68/QqOta5drKKi8WLNIC2tsVR+jgU3nnMCr4uiRXrIS2h6R0xDYMwCzL1HS0tLT0pJyanIb6uTKCD6pywGbAo3jaVd6Ka/xpx/auWylE9Wc95hP+sXw61f9G1xTj3zE9UgaZFyP3Pgw8EYQ/jCp9UDNAnWKcS3Y9hF8X7g3HiPeFTIS0VKeshLQEjIS0tKhBxm3XVocxugm/7JXa+6n2h03KcpwccCtvukW9ILZnO2UCL5DfLlXhQTxKlV607rKEbZVakuOyTlFwdiV5FG2BkMkJ54/U+4Q5FE+vSWzHVMgC5hPNmpSKimwiKE0Wn+KqcxVdXsJZ7unfYFaaDvTGx4zcd6VpIEpjRPgn/koLABYCWK0v5PtUhJS0pL");
        private static int[] order = new int[] { 2,9,2,12,11,12,8,9,12,10,10,12,12,13,14 };
        private static int key = 74;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
