using System;
using System.Security.Cryptography;
using System.Text;

namespace WebSite_Online1a.Models.MD5
{
    public class MD5
    {
        public static string MD5Hash(string input)
        {
            // Chuyển đổi input thành mảng bytes
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);

            // Tạo đối tượng MD5
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                // Mã hóa mảng bytes thành mảng bytes mã hóa
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Chuyển đổi mảng bytes mã hóa thành chuỗi hexa
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
                /////////////////////////////////////////////// Khi sử dụng chỉ cần thêm a = MD5.MD5Hash(password) để lưu
            }
        }
    }
}
