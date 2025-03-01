﻿using System;
using Smartstore.Core.Checkout.Orders;
using Smartstore.Core.Common;
using Smartstore.Web.Modelling;

namespace Smartstore.Admin.Models.Orders
{
    [LocalizedDisplay("Admin.ReturnRequests.Fields.")]
    public class ReturnRequestModel : EntityModelBase
    {
        [LocalizedDisplay("*ID")]
        public override int Id { get; set; }

        [LocalizedDisplay("Admin.Customers.Customers.Orders.Store")]
        public string StoreName { get; set; }

        [LocalizedDisplay("*Order")]
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }

        [LocalizedDisplay("*Customer")]
        public int CustomerId { get; set; }
        public string CustomerFullName { get; set; }
        public bool CanSendEmailToCustomer { get; set; }

        public int ProductId { get; set; }
        public string ProductSku { get; set; }
        public string AttributeInfo { get; set; }

        [LocalizedDisplay("*Product")]
        public string ProductName { get; set; }
        public string ProductTypeName { get; set; }
        public string ProductTypeLabelHint { get; set; }

        [LocalizedDisplay("Admin.ReturnRequests.MaxRefundAmount")]
        public Money MaxRefundAmount { get; set; }

        [LocalizedDisplay("*Quantity")]
        public int Quantity { get; set; }

        [LocalizedDisplay("*ReasonForReturn")]
        public string ReasonForReturn { get; set; }

        [LocalizedDisplay("*RequestedAction")]
        public string RequestedAction { get; set; }

        [LocalizedDisplay("*RequestedActionUpdatedOnUtc")]
        public DateTime? RequestedActionUpdated { get; set; }

        [LocalizedDisplay("*CustomerComments")]
        public string CustomerComments { get; set; }

        [LocalizedDisplay("*StaffNotes")]
        public string StaffNotes { get; set; }

        [LocalizedDisplay("Admin.Common.AdminComment")]
        public string AdminComment { get; set; }

        [LocalizedDisplay("*Status")]
        public int ReturnRequestStatusId { get; set; }
        [LocalizedDisplay("*Status")]
        public string ReturnRequestStatusString { get; set; }

        [LocalizedDisplay("Common.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [LocalizedDisplay("Common.UpdatedOn")]
        public DateTime UpdatedOn { get; set; }

        public bool CanAccept
            => Id != 0 && (ReturnRequestStatus)ReturnRequestStatusId < ReturnRequestStatus.ReturnAuthorized;

        public string ReturnRequestInfo { get; set; }
        public string EditUrl { get; set; }
        public string OrderEditUrl { get; set; }
        public string CustomerEditUrl { get; set; }
        public string ProductEditUrl { get; set; }

        public UpdateOrderItemModel UpdateOrderItem { get; set; }
    }
}
