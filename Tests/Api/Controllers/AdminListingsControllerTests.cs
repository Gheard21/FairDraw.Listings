using AutoMapper;
using FairDraw.Listings.App.Api.Controllers;
using FairDraw.Listings.App.Application.Models.Requests;
using FairDraw.Listings.App.Application.Models.Responses;
using FairDraw.Listings.App.Domain.Entities;
using FairDraw.Listings.App.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;

namespace FairDraw.Listings.Tests.Api.Controllers
{
    public class AdminListingsControllerTestsTest
    {
        private readonly Mock<IListingRepository> _repo;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IValidator<NewListingRequest>> _validator;

        public AdminListingsControllerTestsTest()
        {
            _repo = new Mock<IListingRepository>(MockBehavior.Strict);
            _mapper = new Mock<IMapper>(MockBehavior.Strict);
            _validator = new Mock<IValidator<NewListingRequest>>(MockBehavior.Strict);
        }

        private AdminListingController CreateController() =>
            new AdminListingController(_repo.Object, _mapper.Object, _validator.Object);

        [Fact]
        public async Task Delete_Returns_NotFound_When_NotExists()
        {
            var id = Guid.NewGuid();
            _repo.Setup(r => r.FindAsync(id)).ReturnsAsync((ListingEntity?)null);

            var controller = CreateController();
            var result = await controller.Delete(id);

            result.ShouldBeOfType<NotFoundResult>();

            _repo.VerifyAll();
            _mapper.VerifyAll();
            _validator.VerifyAll();
        }

        [Fact]
        public async Task Delete_Returns_NoContent_When_Deleted()
        {
            var id = Guid.NewGuid();
            var entity = new ListingEntity("testTenant", "testTitle", 100.0, "testDescription");

            _repo.Setup(r => r.FindAsync(id)).ReturnsAsync(entity);
            _repo.Setup(r => r.Remove(entity));
            _repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var controller = CreateController();
            var result = await controller.Delete(id);

            result.ShouldBeOfType<NoContentResult>();

            _repo.VerifyAll();
            _mapper.VerifyAll();
            _validator.VerifyAll();
        }

        [Fact]
        public async Task Get_Returns_NotFound_When_NotExists()
        {
            var id = Guid.NewGuid();
            _repo.Setup(r => r.FindAsync(id)).ReturnsAsync((ListingEntity?)null);

            var controller = CreateController();
            var result = await controller.Get(id);

            result.ShouldBeOfType<NotFoundResult>();

            _repo.VerifyAll();
            _mapper.VerifyAll();
            _validator.VerifyAll();
        }

        [Fact]
        public async Task Get_Returns_Ok_With_Response_When_Found()
        {
            var id = Guid.NewGuid();
            var entity = new ListingEntity("testTenant", "testTitle", 100.0, "testDescription");
            var response = new ListingResponse();

            _repo.Setup(r => r.FindAsync(id)).ReturnsAsync(entity);
            _mapper.Setup(m => m.Map<ListingResponse>(entity)).Returns(response);

            var controller = CreateController();
            var result = await controller.Get(id);

            var ok = result.ShouldBeOfType<OkObjectResult>();
            ok.Value.ShouldBe(response);

            _repo.VerifyAll();
            _mapper.VerifyAll();
            _validator.VerifyAll();
        }

        [Fact]
        public async Task GetPaged_Returns_Ok_With_Mapped_Results()
        {
            var pageSize = 2;
            var lastId = Guid.NewGuid();
            var search = "test";
            var sortBy = "Title";
            var isAsc = false;

            var entities = new List<ListingEntity>
            {
                new ListingEntity("testTenant", "testTitle1", 100.0, "testDescription1"),
                new ListingEntity("testTenant", "testTitle2", 200.0, "testDescription2")
            } as IEnumerable<ListingEntity>;

            var responses = new List<ListingResponse>
            {
                new ListingResponse(),
                new ListingResponse()
            } as IEnumerable<ListingResponse>;

            _repo.Setup(r => r.GetPagedFilteredResultAsync(pageSize, lastId, search, sortBy, isAsc))
                 .ReturnsAsync(entities);
            _mapper.Setup(m => m.Map<IEnumerable<ListingResponse>>(entities))
                   .Returns(responses);

            var controller = CreateController();
            var result = await controller.GetPaged(pageSize, lastId, search, sortBy, isAsc);

            var ok = result.ShouldBeOfType<OkObjectResult>();
            ok.Value.ShouldBe(responses);

            _repo.VerifyAll();
            _mapper.VerifyAll();
            _validator.VerifyAll();
        }

        [Fact]
        public async Task Patch_Returns_NotFound_When_NotExists()
        {
            var id = Guid.NewGuid();
            var request = new UpdateListingRequest();

            _repo.Setup(r => r.FindAsync(id)).ReturnsAsync((ListingEntity?)null);

            var controller = CreateController();
            var result = await controller.Patch(id, request);

            result.ShouldBeOfType<NotFoundResult>();

            _repo.VerifyAll();
            _mapper.VerifyAll();
            _validator.VerifyAll();
        }

        [Fact]
        public async Task Patch_Updates_And_Returns_Ok()
        {
            var id = Guid.NewGuid();
            var entity = new ListingEntity("testTenant", "testTitle", 100.0, "testDescription");
            var request = new UpdateListingRequest();
            var response = new ListingResponse();

            _repo.Setup(r => r.FindAsync(id)).ReturnsAsync(entity);
            _mapper.Setup(m => m.Map(It.IsAny<UpdateListingRequest>(), It.IsAny<ListingEntity>()))
                   .Callback<UpdateListingRequest, ListingEntity>((_, _) => { })
                   .Returns(entity);
            _repo.Setup(r => r.Update(entity));
            _repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            _mapper.Setup(m => m.Map<ListingResponse>(entity)).Returns(response);

            var controller = CreateController();
            var result = await controller.Patch(id, request);

            var ok = result.ShouldBeOfType<OkObjectResult>();
            ok.Value.ShouldBe(response);

            _repo.VerifyAll();
            _mapper.VerifyAll();
            _validator.VerifyAll();
        }

        [Fact]
        public async Task Post_Returns_BadRequest_When_Invalid()
        {
            var request = new NewListingRequest();
            var failures = new List<ValidationFailure> { new ValidationFailure("Name", "Required") };
            var validationResult = new ValidationResult(failures);

            _validator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(validationResult);

            var controller = CreateController();
            var result = await controller.Post(request);

            var bad = result.ShouldBeOfType<BadRequestObjectResult>();
            bad.Value.ShouldBe(failures);

            _repo.VerifyAll();
            _mapper.VerifyAll();
            _validator.VerifyAll();
        }

        [Fact]
        public async Task Post_Creates_And_Returns_CreatedAtAction_When_Valid()
        {
            var request = new NewListingRequest();
            var entity = new ListingEntity("testTenant", "testTitle", 100.0, "testDescription");
            var response = new ListingResponse();
            var validationResult = new ValidationResult();

            _validator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(validationResult);
            _mapper.Setup(m => m.Map<ListingEntity>(request)).Returns(entity);
            _repo.Setup(r => r.AddAsync(entity)).Returns(Task.CompletedTask);
            _repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            _mapper.Setup(m => m.Map<ListingResponse>(entity)).Returns(response);

            var controller = CreateController();
            var result = await controller.Post(request);

            var created = result.ShouldBeOfType<CreatedAtActionResult>();
            created.ActionName.ShouldBe(nameof(AdminListingController.Get));
            created.RouteValues.ShouldNotBeNull();
            created.RouteValues!["listingId"].ShouldBe(entity.Id);
            created.Value.ShouldBe(response);

            _repo.VerifyAll();
            _mapper.VerifyAll();
            _validator.VerifyAll();
        }
    }
}