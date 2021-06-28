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
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
    public class BidsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBidManagementService _bidManagementService;

        public BidsController(
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IBidManagementService bidManagementService
        )
        {
            _mapper = mapper;
            _userManager = userManager;
            _bidManagementService = bidManagementService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BidForUserResponse>>> GetAll()
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (user == null)
            {
                return NotFound();
            }

            var serviceResponse = await _bidManagementService.GetBids(user.Id);
            var bids = serviceResponse.ResponseOk;

            return _mapper.Map<List<Bid>, List<BidForUserResponse>>(bids);
        }

        [HttpPost]
        public async Task<ActionResult> CreateBids(string userId, NewBidRequest newBidRequest)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var serviceResponse = await _bidManagementService.CreateBids(user.Id, newBidRequest);

            if (serviceResponse.ResponseError == null)
            {
                return Ok();
            }

            return StatusCode(500);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateFavourites(UpdateBidForUser updateBidForUser)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var bidsWithIdResponse = await _bidManagementService.GetBid(user.Id, updateBidForUser.Id);

            Bid bids = bidsWithIdResponse.ResponseOk;

            if (bids == null)
            {
                return BadRequest("There is no bids list with this ID.");
            }

            var serviceResponse = await _bidManagementService.UpdateBids(bids, updateBidForUser);

            if (serviceResponse.ResponseError == null)
            {
                return Ok();
            }

            return StatusCode(500);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBids(int id)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var bidsResponse = await _bidManagementService.GetBid(user.Id, id);
            var bids = bidsResponse.ResponseOk;

            if (bids == null)
            {
                return NotFound();
            }

            var result = await _bidManagementService.DeleteBids(id);

            if (result.ResponseError == null)
            {
                return NoContent();
            }


            return StatusCode(500);
        }
    }

}


