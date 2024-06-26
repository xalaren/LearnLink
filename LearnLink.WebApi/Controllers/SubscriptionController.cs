﻿using LearnLink.Application.Interactors;
using LearnLink.Core.Entities;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.WebApi.Controllers
{
    [ApiController]
    [Route("api/subscriptions")]
    public class SubscriptionController : Controller
    {
        private readonly SubscriptionInteractor subscriptionInteractor;
        private readonly UserVerifierService userVerifierService;

        public SubscriptionController(SubscriptionInteractor subscriptionInteractor, UserVerifierService userVerifierService)
        {
            this.subscriptionInteractor = subscriptionInteractor;
            this.userVerifierService = userVerifierService;
        }

        [Authorize]
        [HttpPost("subscribe")]
        public async Task<Response> SubscribeAsync(int userId, int courseId)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, userId);


            if (!verifyResponse.Success) return new()
            {
                Success = verifyResponse.Success,
                Message = verifyResponse.Message,
            };

            return await subscriptionInteractor.SubscribeAsync(new SubscriptionDto(userId, courseId, DateTime.Now));
        }

        [Authorize]
        [HttpDelete("unsubscribe")]
        public async Task<Response> UnsubscribeAsync(int userId, int courseId)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, userId);

            if (!verifyResponse.Success) return new()
            {
                Success = verifyResponse.Success,
                Message = verifyResponse.Message,
            };

            return await subscriptionInteractor.Unsubscribe(userId, courseId);
        }

        [Authorize]
        [HttpPost("invite")]
        public async Task<Response> InviteAsync(int userId, int courseId, int localRoleId, [FromBody] int[] userIdentifiers)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, userId);

            if (!verifyResponse.Success) return new()
            {
                Success = verifyResponse.Success,
                Message = verifyResponse.Message,
            };

            return await subscriptionInteractor.InviteAsync(userId, courseId, localRoleId, userIdentifiers);
        }

        [Authorize]
        [HttpDelete("kick")]
        public async Task<Response> KickAsync(int requesterUserId, int targetUserId, int courseId)
        {
            var verifyResponse = await userVerifierService.VerifyUserAsync(User.Identity?.Name, requesterUserId);

            if (!verifyResponse.Success) return new()
            {
                Success = verifyResponse.Success,
                Message = verifyResponse.Message,
            };

            return await subscriptionInteractor.KickUserAsync(requesterUserId, targetUserId, courseId);
        }
    }
}
