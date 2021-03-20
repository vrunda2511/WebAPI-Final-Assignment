﻿using HM.DAL.Database;
using HM.DAL.Interface;
using HM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM.DAL.Repository
{
    public class HotelRepository : IHotelRepository
    {
        private HotelManagementEntities context;
        public HotelRepository()
        {
            context = new HotelManagementEntities();
        }
        // POST hotels
        public Models.Hotel AddHotel(Models.Hotel hotel)
        {
            Database.Hotel model = new Database.Hotel();
            model.Address = hotel.Address;
            model.City = hotel.City;
            model.ContactNo = hotel.ContactNo;
            model.ContactPerson = hotel.ContactPerson;
            model.CreatedBy = hotel.CreatedBy;
            model.CreatedDate = hotel.CreatedDate;
            model.facebook = hotel.facebook;
            model.IsActive = hotel.IsActive;
            model.Name = hotel.Name;
            model.PinCode = hotel.PinCode;
            model.Twitter = hotel.Twitter;
            model.UpdatedBy = hotel.UpdatedBy;
            model.UpdatedDate = hotel.UpdatedDate;
            model.Website = hotel.Website;
            model.Rooms = new List<Database.Room>();
            foreach (var item in hotel.Rooms)
            {
                Database.Room rooms = new Database.Room();
                // rooms.HotelId = item.HotelId;
                rooms.Name = item.Name;
                rooms.Category = item.Category;
                rooms.Price = item.Price;
                rooms.IsActive = item.IsActive;
                rooms.CreatedBy = item.CreatedBy;
                rooms.CreatedDate = DateTime.Today;
                rooms.UpdatedBy = item.UpdatedBy;
                rooms.UpdatedDate = DateTime.Today;
                model.Rooms.Add(rooms);
            }
            var result = context.Hotels.Add(model);
            context.SaveChanges();
            return hotel;

        }
        // POST Booked the room of hotel for particular date 

        public string Book(Models.Booking booking)
        {

            Database.Booking model = new Database.Booking();
            model.BookingDate = booking.BookingDate;
            model.bookingStatus = booking.bookingStatus;
            model.roomID = booking.roomID;
            if (context.Bookings.Any(x => x.roomID == model.roomID))
            {
                if (context.Bookings.Any(x => x.roomID == model.roomID && x.BookingDate == model.BookingDate && x.bookingStatus== "definitive ")) return "Room is alrady booked";
                else
                {
                    var result = context.Bookings.Add(model);
                    context.SaveChanges();
                    return "Your room booked successfully";
                }
            }
            else

                return "none";
           
        }


        // delete booking by booking Id

        public bool DeleteBook(int id)
        {
            var result = context.Bookings.FirstOrDefault(x => x.Id == id);
            if (result != null)
            {
                result.bookingStatus = "Deleted";
                context.SaveChanges();
                return true;
            }
            else return false;
        }

        
        //GET all hotels
        public List<Models.Hotel> GetHotels()
        {
            List<Models.Hotel> hotels = new List<Models.Hotel>();
            var result = context.Hotels.ToList();
            if (result != null)
            {
                foreach (var hotel in result)
                {
                    Models.Hotel model = new Models.Hotel();
                    model.Address = hotel.Address;
                    model.City = hotel.City;
                    model.ContactNo = hotel.ContactNo;
                    model.ContactPerson = hotel.ContactPerson;
                    model.CreatedBy = hotel.CreatedBy;
                    model.CreatedDate = hotel.CreatedDate;
                    model.facebook = hotel.facebook;
                    model.IsActive = hotel.IsActive;
                    model.Name = hotel.Name;
                    model.PinCode = hotel.PinCode;
                    model.Twitter = hotel.Twitter;
                    model.UpdatedBy = hotel.UpdatedBy;
                    model.UpdatedDate = hotel.UpdatedDate;
                    model.Website = hotel.Website;
                    model.Id = hotel.Id;
                    model.Rooms = new List<Models.Room>();
                    foreach (var item in hotel.Rooms)
                    {
                        Models.Room rooms = new Models.Room();
                        rooms.HotelId = item.HotelId;
                        rooms.Name = item.Name;
                        rooms.Category = item.Category;
                        rooms.Price = item.Price;
                        rooms.IsActive = item.IsActive;
                        rooms.CreatedBy = item.CreatedBy;
                        rooms.CreatedDate = DateTime.Today;
                        rooms.UpdatedBy = item.UpdatedBy;
                        rooms.UpdatedDate = DateTime.Today;
                        model.Rooms.Add(rooms);
                    }
                    hotels.Add(model);
                }
            }
            return hotels.OrderBy(x => x.Name).ToList();
        }

        public List<Models.Room> GetRooms(string city, decimal? pincode, decimal? price, string category)
        {
            if (city != null || pincode != null)
            {
                var HotelIds = context.Hotels.Where(x => x.City == city || x.PinCode == pincode).Select(x => x.Id);
                List<Models.Room> rooms = new List<Models.Room>();
                foreach (var HotelId in HotelIds)
                {
                    var Rgrps = context.Rooms.Where(x => x.HotelId == HotelId).ToList();
                    foreach (var rgrp in Rgrps)
                    {
                        Models.Room model = new Models.Room();
                        model.Category = rgrp.Category;
                        model.IsActive = rgrp.IsActive;
                        model.Name = rgrp.Name;
                        model.Price = rgrp.Price;
                        model.CreatedBy = rgrp.CreatedBy;
                        model.CreatedDate = rgrp.CreatedDate;
                        model.HotelId = rgrp.HotelId;
                        model.Id = rgrp.Id;
                        model.UpdatedBy = rgrp.UpdatedBy;
                        model.UpdatedDate = rgrp.UpdatedDate;
                        model.Hotel = new Models.Hotel()
                        {
                            City = rgrp.Hotel.City,
                            Address = rgrp.Hotel.Address,
                            ContactNo = rgrp.Hotel.ContactNo,
                            ContactPerson = rgrp.Hotel.ContactPerson,
                            CreatedBy = rgrp.Hotel.CreatedBy,
                            CreatedDate = rgrp.Hotel.CreatedDate,
                            facebook = rgrp.Hotel.facebook,
                            IsActive = rgrp.Hotel.IsActive,
                            Name = rgrp.Hotel.Name,
                            PinCode = rgrp.Hotel.PinCode,
                            Twitter = rgrp.Hotel.Twitter,
                            UpdatedBy = rgrp.Hotel.UpdatedBy,
                            UpdatedDate = rgrp.Hotel.UpdatedDate,
                            Website = rgrp.Hotel.Website,
                            Id = rgrp.Hotel.Id

                        };

                        rooms.Add(model);
                    }

                }
                return rooms;
            }
            else if (price != null)
            {
                List<Models.Room> rooms = new List<Models.Room>();
                var Rgrps = context.Rooms.Where(x => x.Price == price).ToList();
                foreach (var rgrp in Rgrps)
                {
                    Models.Room model = new Models.Room();
                    model.Category = rgrp.Category;
                    model.IsActive = rgrp.IsActive;
                    model.Name = rgrp.Name;
                    model.Price = rgrp.Price;
                    model.CreatedBy = rgrp.CreatedBy;
                    model.CreatedDate = rgrp.CreatedDate;
                    model.HotelId = rgrp.HotelId;
                    model.Id = rgrp.Id;
                    model.UpdatedBy = rgrp.UpdatedBy;
                    model.UpdatedDate = rgrp.UpdatedDate;
                    model.Hotel = new Models.Hotel()
                    {
                        City = rgrp.Hotel.City,
                        Address = rgrp.Hotel.Address,
                        ContactNo = rgrp.Hotel.ContactNo,
                        ContactPerson = rgrp.Hotel.ContactPerson,
                        CreatedBy = rgrp.Hotel.CreatedBy,
                        CreatedDate = rgrp.Hotel.CreatedDate,
                        facebook = rgrp.Hotel.facebook,
                        IsActive = rgrp.Hotel.IsActive,
                        Name = rgrp.Hotel.Name,
                        PinCode = rgrp.Hotel.PinCode,
                        Twitter = rgrp.Hotel.Twitter,
                        UpdatedBy = rgrp.Hotel.UpdatedBy,
                        UpdatedDate = rgrp.Hotel.UpdatedDate,
                        Website = rgrp.Hotel.Website,
                        Id = rgrp.Hotel.Id

                    };
                    rooms.Add(model);
                }
                return rooms;
            }
            else if (category != null)
            {
                List<Models.Room> rooms = new List<Models.Room>();
                var Rgrps = context.Rooms.Where(x => x.Category == category).ToList();
                foreach (var rgrp in Rgrps)
                {
                    Models.Room model = new Models.Room();
                    model.Category = rgrp.Category;
                    model.IsActive = rgrp.IsActive;
                    model.Name = rgrp.Name;
                    model.Price = rgrp.Price;
                    model.CreatedBy = rgrp.CreatedBy;
                    model.CreatedDate = rgrp.CreatedDate;
                    model.HotelId = rgrp.HotelId;
                    model.Id = rgrp.Id;
                    model.UpdatedBy = rgrp.UpdatedBy;
                    model.UpdatedDate = rgrp.UpdatedDate;
                    model.Hotel = new Models.Hotel()
                    {
                        City = rgrp.Hotel.City,
                        Address = rgrp.Hotel.Address,
                        ContactNo = rgrp.Hotel.ContactNo,
                        ContactPerson = rgrp.Hotel.ContactPerson,
                        CreatedBy = rgrp.Hotel.CreatedBy,
                        CreatedDate = rgrp.Hotel.CreatedDate,
                        facebook = rgrp.Hotel.facebook,
                        IsActive = rgrp.Hotel.IsActive,
                        Name = rgrp.Hotel.Name,
                        PinCode = rgrp.Hotel.PinCode,
                        Twitter = rgrp.Hotel.Twitter,
                        UpdatedBy = rgrp.Hotel.UpdatedBy,
                        UpdatedDate = rgrp.Hotel.UpdatedDate,
                        Website = rgrp.Hotel.Website,
                        Id = rgrp.Hotel.Id

                    };
                    rooms.Add(model);
                }
                return rooms;
            }
            else
            {
                return null;
            }
        }

        public string IsAvailable(int id, DateTime date)
        {
            if (context.Bookings.Any(x => x.roomID == id))
            {
                if (context.Bookings.Any(x => x.roomID == id && x.BookingDate == date)) return "Room is Not avalible";
                else return "Room is avalible";
            }
            else

                return "none";

        }

        public bool UpdateBookDate(int id, Models.Booking booking)
        {
            var result = context.Bookings.FirstOrDefault(x => x.roomID == id);
            if (result != null)
            {
                result.BookingDate = booking.BookingDate;
                context.SaveChanges();
                return true;
            }
            else return false;
        }

        public bool UpdateStatus(int id, Models.Booking booking)
        {
            var result = context.Bookings.FirstOrDefault(x => x.Id == id);
            if (result != null)
            {
                result.bookingStatus = booking.bookingStatus;
                context.SaveChanges();
                return true;
            }
            else return false;
        }
    }

}
