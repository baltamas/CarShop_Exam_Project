using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CarShop.Data;
using CarShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarShop.Services.BidService;
using CarShop.ViewModels.Bids;

namespace CarShop.Controllers
{
    [Authorize(AuthenticationSchemes = "Identity.Application, Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class BidController : ControllerBase
    {
        private readonly IBidManagementService _bidManagementService;
        private readonly UserManager<ApplicationUser> _userManager;

        public BidController(IBidManagementService bidManagementService, UserManager<ApplicationUser> userManager)
        {
            _bidManagementService = bidManagementService;
            _userManager = userManager;
        }

        /// <summary>
        /// Adds a new reservation
        /// </summary>
        /// <response code="201">Adds a new reservation</response>
        /// <response code="400">Unable to add the reservation</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "Identity.Application, Bearer")]
        [HttpPost]
        public async Task<ActionResult> PlaceReservation(NewBidRequest newBidRequest)
        {
            var user = new ApplicationUser();
            try
            {
                user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("Please login!");
            }

            var bidServiceResult = await _bidManagementService.PlaceBid(newBidRequest, user);
            if (bidServiceResult.ResponseError != null)
            {
                return BadRequest(bidServiceResult.ResponseError);
            }

            var bid = bidServiceResult.ResponseOk;

            return CreatedAtAction("GetBids", new { id = bid.Id }, "New bid successfully placed");
        }


        /// <summary>
        /// Get all reservations
        /// </summary>
        /// <response code="200">Get All Reservations</response>
        /// <summary>
        /// Get all reservations
        /// </summary>
        /// <response code="200">Get All Reservations</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = "Identity.Application, Bearer")]
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var user = new ApplicationUser();
            try
            {
                user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("You have to login!");
            }

            var bidServiceResult = await _bidManagementService.GetAll(user);

            return Ok(bidServiceResult.ResponseOk);
        }

        /// <summary>
        /// Edit a reservation
        /// </summary>
        /// <response code="204">Update a reservation</response>
        /// <response code="400">Unable to update the reservation</response>
        /// <response code="404">Reservation not found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = "Identity.Application, Bearer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBid(int id, NewBidRequest updateBidRequest)
        {
            var user = new ApplicationUser();
            try
            {
                user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("You have to login!");
            }

            var bidServiceResult = await _bidManagementService.UpdateBid(id, updateBidRequest, user);
            if (bidServiceResult.ResponseError != null)
            {
                return BadRequest(bidServiceResult.ResponseError);
            }

            return NoContent();
        }

        /// <summary>
        /// Delete a reservation by the given id
        /// </summary>
        /// <response code="204">Deletes a reservation</response>
        /// <response code="404">Reservation not found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = "Identity.Application, Bearer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBid(int id)
        {
            try
            {
                var user = await _userManager?.FindByNameAsync(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized("You have to login!");
            }

            if (!_bidManagementService.BidExists(id))
            {
                return NotFound();
            }

            var bidServiceResult = await _bidManagementService.DeleteBid(id);
            if (bidServiceResult.ResponseError != null)
            {
                return BadRequest(bidServiceResult.ResponseError);
            }

            return NoContent();
        }
    }

}


