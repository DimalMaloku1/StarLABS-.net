using System.Collections;
using Application.DTOs;
using Application.DTOs.AccountDTOs;
using Application.Services;
using Application.Services.AccountServices;
using Application.Services.BillService;
using Application.Services.BookingServices;
using Application.Services.PaymentServices;
using Application.Services.PositionServices;
using Application.Services.RoomServices;
using Application.Services.RoomTypeServices;
using Application.Services.StaffServices;
using AutoMapper;
using Domain.Models;

namespace Application.Services.DashboardService;

public class DashboardService : IDashboardService
{
    private readonly IBillService _billService;
    private readonly IRoomServices _roomService;
    private readonly IMapper _mapper;
    private readonly IBookingService _bookingService;
    private readonly IRoomTypeServices _roomTypeService;
    private readonly IAccountService _userService;
    private readonly IStaffService _staffService;
    private readonly IPaymentService _paymentService;
    private readonly IPositionServices _positionService;



    public DashboardService(IBillService billService, IRoomServices roomService,
        IBookingService bookingService,IStaffService staffService, IPaymentService paymentService,
        IRoomTypeServices roomTypeService, IAccountService userService,IPositionServices positionService,
        IMapper mapper )
    {
        _billService = billService;
        _roomService = roomService;
        _mapper = mapper;
        _bookingService = bookingService;
        _roomTypeService = roomTypeService;
        _userService = userService;
        _staffService = staffService;
        _paymentService = paymentService;
        _positionService = positionService;
       

    }


    public async Task<IEnumerable<RoomDto>> GetRooms()
    {
        var rooms = await _roomService.GetAllRoomsAsync();
        return _mapper.Map<IEnumerable<RoomDto>>(rooms);
    }


    public async Task<IEnumerable<BillDto>> GetBills()
    {
        var bills = await _billService.GetAllBills();
        return bills;
    }

    public async Task<IEnumerable<BookingDto>> GetBookings()
    {
        var bookings = await _bookingService.GetAllBookingsAsync();
        return bookings;
    }


    public async Task<IEnumerable<RoomTypeDto>> GetRoomTypes()
    {
        var roomTypes = await _roomTypeService.GetAllRoomTypesAsync();
        return roomTypes;
    }

    public async Task<IEnumerable<UserDto>> GetUsers()
    {
        var users = await _userService.GetAllUsers();
        return users;
    }

    public async Task<IEnumerable<StaffDTO>> GetStaff()
    {
        var staff = await _staffService.GetAllStaffAsync();
        return staff;
    }

    public async Task<IEnumerable<PaymentDto>> GetPayments()
    {
        var payments = await _paymentService.GetAllPaymentsAsync();
        return payments;
    }

    public async Task<IEnumerable<PositionDTO>> GetPositions()
    {
        var positions = await _positionService.GetAllPositionsAsync();
        return positions;
    }

    public async Task<UserDto> UpdateUser(UserDto user)
    {
        await _userService.UpdateUser(user);
        return user;
    }

    public async Task<UserDto> GetUserById(Guid id)
    {
        var user = await _userService.GetUserById(id);
        return user;
    }

    public async Task DeleteUser(Guid id)
    {
        await _userService.DeleteUser(id);
    }

    
}