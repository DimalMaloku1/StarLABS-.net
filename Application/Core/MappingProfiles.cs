using Application.DTOs;
using Application.DTOs.AccountDTOs;
using AutoMapper;
using Domain.Models;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Booking, BookingDto>().ReverseMap();
            CreateMap<Feedback, FeedbackDto>().ReverseMap();
            CreateMap<Room, RoomDto>().ReverseMap();
            CreateMap<RoomType, RoomTypeDto>().ReverseMap();
            CreateMap<Staff, StaffDTO>().ReverseMap();
            CreateMap<Payment, PaymentDto>().ReverseMap();
            CreateMap<Position, PositionDTO>().ReverseMap();
            CreateMap<Bill, BillDto>().ReverseMap();
            CreateMap<AppUser, UserDto>().ReverseMap();
            CreateMap<RoomTypePhoto,RoomTypePhotoDTO>().ReverseMap();
            CreateMap<AppUser, VerifyAccountDto>().ReverseMap();





        }
    }
}