﻿using System;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Services.FeedbackServices;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService _feedbackService;
        private readonly UserManager<AppUser> _userManager;

        public FeedbackController(UserManager<AppUser> userManager, IFeedbackService feedbackService)
        {
            _userManager = userManager;
            _feedbackService = feedbackService;
        }
    
       public async Task<IActionResult> Index()
        {
            var feedbacks = await _feedbackService.GetAllFeedbacksAsync();
            var averageRating = await _feedbackService.CalculateAverageRatingAsync();
            ViewBag.AverageRating = averageRating; 
            return View("Index", feedbacks);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var feedback = await _feedbackService.GetFeedbackByIdAsync(id);
            return View("Details", feedback);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FeedbackDto feedbackDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    feedbackDto.UserId = user.Id;

                    // Pass the feedback object to the service for creation
                    await _feedbackService.CreateAsync(feedbackDto);

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "User not found");
                }
            }

            // If ModelState is not valid or user is not found, return the view with errors
            return View(feedbackDto);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var feedback = await _feedbackService.GetFeedbackByIdAsync(id);
            return View("Edit", feedback);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, FeedbackDto feedbackDto)
        {
            await _feedbackService.UpdateAsync(id, feedbackDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            await _feedbackService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
