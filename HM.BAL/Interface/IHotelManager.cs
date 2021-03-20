
using HM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM.BAL.Interface
{
    public interface IHotelManager
    {
        List<Hotel> GetHotels();
        List<Room> GetRooms(string city, decimal? pincode, decimal? price, string category);
        string IsAvailable(int id,DateTime date);
        Hotel AddHotel(Hotel hotel);
        string Book(Booking booking);
        bool UpdateBookDate(int id,Booking booking);
        bool UpdateStatus(int id,Booking booking);
        bool DeleteBook(int id);
    }
}
