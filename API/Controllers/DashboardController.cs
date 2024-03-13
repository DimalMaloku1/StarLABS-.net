﻿using Application.DTOs.AccountDTOs;
using Application.Services.BillService;
using Application.Services.DashboardService;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace API.Controllers;
[Authorize(Roles = "Admin")]
public class DashboardController : Controller
 
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }
    
    public IActionResult Index()
    {
        return View();
    }
    
    public async Task<IActionResult> Bookings()
    {
        var bookings = await _dashboardService.GetBookings();
        return View(bookings);
    }
    public async Task <IActionResult> Bills()
    {
        var bills = await _dashboardService.GetBills();
        return View(bills);
    }
    public async Task<IActionResult> Users()
    {
        var users = await _dashboardService.GetUsers();
        return View(users);
    }
    public async Task<IActionResult> Rooms()
    {
        var rooms = await _dashboardService.GetRooms();
        return View(rooms);
    }
    public async Task<IActionResult> RoomTypes()
    {
        var roomTypes = await _dashboardService.GetRoomTypes();
        return View(roomTypes);
    }

    public async Task<IActionResult> Staff()
    {
        var staff = await _dashboardService.GetStaff();
        return View(staff);
    }

    public async Task<IActionResult> Payments()
    {
        var payments = await _dashboardService.GetPayments();
        return View(payments);
    }

    public async Task<IActionResult> Positions()
    {
        var positions = await _dashboardService.GetPositions();
        return View(positions);
    }

    public async Task<IActionResult> UserDetails(Guid id)
    {
        var user = await _dashboardService.GetUserById(id);
        return View(user);
    }

    public async Task<IActionResult> DeleteUser(Guid id)
    {
        await _dashboardService.DeleteUser(id);
        return RedirectToAction(nameof(Users));
    }

    [HttpGet]
    public async Task<IActionResult> UpdateUser(Guid id)
    {
        var user = await _dashboardService.GetUserById(id);
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUser(UserDto user)
    {
        await _dashboardService.UpdateUser(user);
        return RedirectToAction("UserDetails", new { id = user.Id });
    }
    
}