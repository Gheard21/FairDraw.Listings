using AutoMapper;
using FairDraw.Listings.App.Application.Models.Requests;
using FairDraw.Listings.App.Application.Models.Responses;
using FairDraw.Listings.App.Domain.Entities;
using FairDraw.Listings.App.Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FairDraw.Listings.App.Api.Controllers
{
    [Route("api/admin/listings")]
    [ApiController]
    [Authorize]
    public class AdminListingController(IListingRepository listingRepository, IMapper mapper, IValidator<NewListingRequest> validator) : ControllerBase
    {
        [HttpDelete("{listingId}")]
        public async Task<IActionResult> Delete(Guid listingId)
        {
            var listing = await listingRepository.FindAsync(listingId);
            if (listing is null)
                return NotFound();

            listingRepository.Remove(listing);
            await listingRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{listingId}")]
        public async Task<IActionResult> Get(Guid listingId)
        {
            var listing = await listingRepository.FindAsync(listingId);

            if (listing is null)
                return NotFound();

            var response = mapper.Map<ListingResponse>(listing);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] int pageSize = 20, [FromQuery] Guid? lastId = null, [FromQuery] string? search = null, [FromQuery] string? sortBy = null, [FromQuery] bool isSortAscending = true)
        {
            var listings = await listingRepository.GetPagedFilteredResultAsync(pageSize, lastId, search, sortBy, isSortAscending);

            var response = mapper.Map<IEnumerable<ListingResponse>>(listings);

            return Ok(response);
        }

        [HttpPatch("{listingId}")]
        public async Task<IActionResult> Patch(Guid listingId, [FromBody] UpdateListingRequest request)
        {
            var listing = await listingRepository.FindAsync(listingId);

            if (listing is null)
                return NotFound();

            mapper.Map(request, listing);

            listingRepository.Update(listing);
            await listingRepository.SaveChangesAsync();

            var response = mapper.Map<ListingResponse>(listing);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewListingRequest request)
        {
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var listing = mapper.Map<ListingEntity>(request);

            await listingRepository.AddAsync(listing);
            await listingRepository.SaveChangesAsync();

            var response = mapper.Map<ListingResponse>(listing);

            return CreatedAtAction(nameof(Get), new { listingId = listing.Id }, response);
        }
    }
}
