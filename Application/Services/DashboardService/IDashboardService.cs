using System.Collections;
using Application.DTOs;
using Application.DTOs.AccountDTOs;
using Domain.Models;

namespace Application.Services.DashboardService;

public interface IDashboardService
{

     Task <IEnumerable<RoomDto>> GetRooms();
    
     Task <IEnumerable<BillDto>> GetBills();
     Task <IEnumerable<BookingDto>> GetBookings();
    Task <IEnumerable<RoomTypeDto>> GetRoomTypes();
    Task <IEnumerable<UserDto>> GetUsers();
    Task<IEnumerable<StaffDTO>> GetStaff();
    Task<IEnumerable<PaymentDto>> GetPayments();
    Task<IEnumerable<PositionDTO>> GetPositions();
    Task <UserDto> UpdateUser(UserDto user);
    Task<UserDto> GetUserById(Guid id);
    Task DeleteUser(Guid id);
    



}