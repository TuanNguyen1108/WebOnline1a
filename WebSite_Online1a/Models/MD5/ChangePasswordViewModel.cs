using System.ComponentModel.DataAnnotations;

namespace WebSite_Online1a.Models.MD5
{
    public class ChangePasswordViewModel
    {
        [Key]
        public int AccountId { get; set; }


        [Display(Name ="Mật Khẩu hiện tại")]
        public string PasswordNow { get; set; }


        [Display(Name = "Mật Khẩu Mới")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu" )]
        [MinLength(5, ErrorMessage = "Bạn cần đặt tối thiểu 5 kí tự")]
        public string Password { get; set;}


        [Display(Name = "Nhập Lại Mật Khẩu Mới")]
        [MinLength(5, ErrorMessage = "Bạn cần đặt tối thiểu 5 kí tự")]
        [Compare("Password", ErrorMessage = "Mật Khẩu không giống nhau")]
        public string ConfirmPassword { get; set;}


    }
}
