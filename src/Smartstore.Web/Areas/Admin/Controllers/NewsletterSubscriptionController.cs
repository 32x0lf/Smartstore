﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smartstore.Admin.Models.Messages;
using Smartstore.ComponentModel;
using Smartstore.Core.Common.Services;
using Smartstore.Core.Data;
using Smartstore.Core.Messaging;
using Smartstore.Core.Security;
using Smartstore.Core.Stores;
using Smartstore.Web.Controllers;
using Smartstore.Web.Models.DataGrid;
using Smartstore.Web.Rendering;

namespace Smartstore.Admin.Controllers
{
    public class NewsletterSubscriptionController : AdminController
    {
        private readonly SmartDbContext _db;
        private readonly IDateTimeHelper _dateTimeHelper;

        public NewsletterSubscriptionController(SmartDbContext db, IDateTimeHelper dateTimeHelper)
        {
            _db = db;
            _dateTimeHelper = dateTimeHelper;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(List));
        }

        [Permission(Permissions.Promotion.Newsletter.Read)]
        public IActionResult List()
        {
            ViewBag.AvailableStores = Services.StoreContext.GetAllStores().ToSelectListItems();
            ViewBag.IsSingleStoreMode = Services.StoreContext.IsSingleStoreMode();

            return View();
        }

        [HttpPost]
        [Permission(Permissions.Promotion.Newsletter.Read)]
        public async Task<IActionResult> List(GridCommand command, NewsletterSubscriptionListModel model)
        {
            var newsletterSubscriptions = await _db.NewsletterSubscriptions
                .AsNoTracking()
                // TODO: (mh) (core) Not sure whether this will work! TEST!! The extension method returns another type (wrapper).
                .ApplyStandardFilter(model.SearchEmail, false, new[] { model.SearchStoreId }, model.SearchCustomerRoleIds)
                .Select(x => x.Subscription)
                .ApplyGridCommand(command)
                .ToPagedList(command)
                .LoadAsync();

            var allStores = Services.StoreContext.GetAllStores().ToDictionary(x => x.Id);
            var newsletterSubscriptionModels = await newsletterSubscriptions
                .SelectAsync(async x =>
                {
                    allStores.TryGetValue(x.StoreId, out var store);
                    var model = await MapperFactory.MapAsync<NewsletterSubscription, NewsletterSubscriptionModel>(x);
                    model.CreatedOn = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc);
                    model.StoreName = store?.Name.NaIfEmpty();

                    return model;
                })
                .AsyncToList();

            var gridModel = new GridModel<NewsletterSubscriptionModel>
            {
                Rows = newsletterSubscriptionModels,
                Total = await newsletterSubscriptions.GetTotalCountAsync()
            };

            return Json(gridModel);
        }

        [HttpPost]
        [Permission(Permissions.Promotion.Newsletter.Update)]
        public async Task<IActionResult> Update(NewsletterSubscriptionModel model)
        {
            var success = false;
            var newsletterSubscription = await _db.NewsletterSubscriptions.FindByIdAsync(model.Id);

            if (newsletterSubscription != null)
            {
                await MapperFactory.MapAsync(model, newsletterSubscription);
                await _db.SaveChangesAsync();
                success = true;
            }

            return Json(new { success });
        }

        [HttpPost]
        [Permission(Permissions.Promotion.Newsletter.Delete)]
        public async Task<IActionResult> Delete(GridSelection selection)
        {
            var success = false;
            var numDeleted = 0;
            var ids = selection.GetEntityIds();

            if (ids.Any())
            {
                var newsletterSubscriptions = await _db.NewsletterSubscriptions.GetManyAsync(ids, true);

                _db.NewsletterSubscriptions.RemoveRange(newsletterSubscriptions);

                numDeleted = await _db.SaveChangesAsync();
                success = true;
            }

            return Json(new { Success = success, Count = numDeleted });
        }
    }
}
