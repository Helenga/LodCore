﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using Common;
using FrontendServices.App_Data.Mappers;
using FrontendServices.Models;
using Journalist;
using UserManagement.Application;
using UserManagement.Domain;

namespace FrontendServices.Controllers
{
    public class DevelopersController : ApiController
    {
        public DevelopersController(
            IUserManager userManager, 
            DevelopersMapper mapper, 
            IConfirmationService confirmationService)
        {
            Require.NotNull(userManager, nameof(userManager));
            Require.NotNull(mapper, nameof(mapper));
            Require.NotNull(confirmationService, nameof(confirmationService));

            _userManager = userManager;
            _mapper = mapper;
            _confirmationService = confirmationService;
        }

        [Route("developers/random/{count}")]
        public IEnumerable<IndexPageDeveloper> GetRandomIndexPageDevelopers(int count)
        {
            Require.ZeroOrGreater(count, nameof(count));

            var users = _userManager.GetUserList().GetRandom(count);
            var indexPageDevelopers = users.Select(_mapper.ToIndexPageDeveloper);
            return indexPageDevelopers;
        }
        
        [HttpGet]
        [Route("developers")]
        public IEnumerable<DeveloperPageDeveloper> GetAllDevelopers()
        {
            var users = _userManager.GetUserList(account => account.ConfirmationStatus != ConfirmationStatus.Unconfirmed);
            var developerPageDevelopers = users.Select(_mapper.ToDeveloperPageDeveloper);
            return developerPageDevelopers;
        }

        [HttpGet]
        [Route("developers/{id}")]
        public Developer GetDeveloper(int id)
        {
            Require.Positive(id, nameof(id));
            try
            {
                var user = _userManager.GetUser(id);
                return _mapper.ToDeveloper(user);
            }
            catch (AccountNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [Route("developers")]
        public IHttpActionResult RegisterNewDeveloper([FromBody] RegisterDeveloperRequest request)
        {
            var createAccountRequest = new CreateAccountRequest(
                new MailAddress(request.Email),
                request.LastName,
                request.FirstName,
                request.Password,
                new Profile
                {
                    InstituteName = request.InstituteName,
                    PhoneNumber = request.PhoneNumber,
                    Specialization = request.StudyingProfile,
                    StudentAccessionYear = request.AccessionYear,
                    StudyingDirection = request.Department,
                    VkProfileUri = request.VkProfileUri == null ? null : new Uri(request.VkProfileUri)
                });
            try
            {
                _userManager.CreateUser(createAccountRequest);
            }
            catch (AccountAlreadyExistsException)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Conflict));
            }

            return Ok();
        }

        [HttpPut]
        [Route("developers/{id}")]
        public IHttpActionResult UpdateProfile(int id, [FromBody] UpdateProfileRequest updateProfileRequest)
        {
            Require.Positive(id, nameof(id));
            Require.NotNull(updateProfileRequest, nameof(updateProfileRequest));

            Account userToChange;
            try
            {
                userToChange = _userManager.GetUser(id);
            }
            catch (AccountNotFoundException)
            {
                return NotFound();
            }

            if (updateProfileRequest.NewPassword != null)
            {
                userToChange.Password = new Password(updateProfileRequest.NewPassword);
            }

            if (updateProfileRequest.Profile != null)
            {
                userToChange.Profile = updateProfileRequest.Profile;
            }

            _userManager.UpdateUser(userToChange);

            return Ok();
        }

        [HttpPost]
        [Route("developers/confirmation/{confirmationToken}")]
        public IHttpActionResult ConfirmEmail(string confirmationToken)
        {
            Require.NotEmpty(confirmationToken, nameof(confirmationToken));

            try
            {
                _confirmationService.ConfirmEmail(confirmationToken);
            }
            catch (TokenNotFoundException)
            {
                return BadRequest("Token not found");
            }

            return Ok();
        }

        private readonly IUserManager _userManager;
        private readonly IConfirmationService _confirmationService;
        private readonly DevelopersMapper _mapper;
    }
}
