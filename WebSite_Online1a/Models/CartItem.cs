using static WebSite_Online1a.Models.CartItem;

namespace WebSite_Online1a.Models
{
    public class CartItem
    {
        public int MaHh { get; set; }
        public int MaSP { get; set; }  
        public string TenHH { get; set; }
        public int SoLuong { get; set; }
        public string Color { get; set; }
        public string HinhAnh { get; set; }
        public int Gia { get; set; }
        public double ThanhTien => SoLuong * Gia;      
    }
    public class CheckoutViewModel
    {
        public string Email { get; set; }
        public string SDT { get; set; }
        public string HoTen { get; set; }

    }
    public class CheckoutPageViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public CheckoutViewModel CheckoutViewModel { get; set; }
    }
    
}




/*public class Order
    {
        public int OrderId { get; set; }
        public List<CartItem> sanpham{ get; set; }

        public Order()
        {
            sanpham = new List<CartItem>();
        }
    }*/
/*public int MaHh { get; set; }
public string TenHH { get; set; }
public string Hinh { get; set; }
public int DonGia { get; set; }
public int SoLuong { get; set; }
public double ThanhTien => SoLuong * DonGia;

public string Color { get; set; }
public string HinhAnh { get; set; }
public int Gia { get; set; }
public double ThanhTien1 => SoLuong * Gia;*/

/* public int ProductId { get; set; }
 public string NameProduct{ get; set; }
 public string ProductImage { get; set; }
 public int Price{ get; set; }
 public int SoLuong { get; set; }
 public double ThanhTien  => SoLuong * Price;*/

