﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Vouchers Management                          Component : Web Api                               *
*  Assembly : Empiria.FinancialAccounting.WebApi.dll       Pattern   : Command Controller                    *
*  Type     : VoucherEditionController                     License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Command web API used to retrive accounting vouchers related data.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.WebApi;

using Empiria.FinancialAccounting.Vouchers.UseCases;
using Empiria.FinancialAccounting.Vouchers.Adapters;

namespace Empiria.FinancialAccounting.Vouchers.WebApi {

  /// <summary>Command web API used to retrive accounting vouchers related data.</summary>
  public class VoucherEditionController : WebApiController {

    #region Web Apis


    [HttpGet]
    [Route("v2/financial-accounting/vouchers/{voucherId:int}")]
    public SingleObjectModel GetVoucher([FromUri] int voucherId) {

      using (var usecases = VoucherUseCases.UseCaseInteractor()) {
        VoucherDto voucher = usecases.GetVoucher(voucherId);

        return new SingleObjectModel(base.Request, voucher);
      }
    }


    [HttpGet]
    [Route("v2/financial-accounting/vouchers/{voucherId:int}/entries/{voucherEntryId:int}")]
    public SingleObjectModel GetVoucherEntry([FromUri] int voucherId,
                                             [FromUri] int voucherEntryId) {

      using (var usecases = VoucherUseCases.UseCaseInteractor()) {
        VoucherEntryDto voucher = usecases.GetVoucherEntry(voucherId, voucherEntryId);

        return new SingleObjectModel(base.Request, voucher);
      }
    }


    [HttpPost]
    [Route("v2/financial-accounting/vouchers/{voucherId:int}/entries")]
    public SingleObjectModel AppendVoucherEntry([FromUri] int voucherId,
                                                [FromBody] VoucherEntryFields fields) {
      base.RequireBody(fields);

      using (var usecases = VoucherEditionUseCases.UseCaseInteractor()) {
        VoucherDto voucher = usecases.AppendEntry(voucherId, fields);

        return new SingleObjectModel(base.Request, voucher);
      }
    }


    [HttpPost]
    [Route("v2/financial-accounting/vouchers/create-voucher")]
    public SingleObjectModel CreateVoucher([FromBody] VoucherFields fields) {
      base.RequireBody(fields);

      using (var usecases = VoucherEditionUseCases.UseCaseInteractor()) {
        VoucherDto voucher = usecases.CreateVoucher(fields);

        return new SingleObjectModel(base.Request, voucher);
      }
    }


    [HttpDelete]
    [Route("v2/financial-accounting/vouchers/{voucherId:int}")]
    public NoDataModel DeleteVoucher([FromUri] int voucherId) {

      using (var usecases = VoucherEditionUseCases.UseCaseInteractor()) {
        usecases.DeleteVoucher(voucherId);

        return new NoDataModel(base.Request);
      }
    }


    [HttpDelete]
    [Route("v2/financial-accounting/vouchers/{voucherId:int}/entries/{voucherEntryId:int}")]
    public SingleObjectModel DeleteVoucherEntry([FromUri] int voucherId,
                                                [FromUri] int voucherEntryId) {

      using (var usecases = VoucherEditionUseCases.UseCaseInteractor()) {
        VoucherDto voucher = usecases.DeleteEntry(voucherId, voucherEntryId);

        return new SingleObjectModel(base.Request, voucher);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v2/financial-accounting/vouchers/{voucherId:int}")]
    public SingleObjectModel UpdateVoucher([FromUri] int voucherId,
                                           [FromBody] VoucherFields fields) {
      base.RequireBody(fields);

      using (var usecases = VoucherEditionUseCases.UseCaseInteractor()) {
        VoucherDto voucher = usecases.UpdateVoucher(voucherId, fields);

        return new SingleObjectModel(base.Request, voucher);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v2/financial-accounting/vouchers/{voucherId:int}/entries/{voucherEntryId:int}")]
    public SingleObjectModel UpdateVoucherEntry([FromUri] int voucherId,
                                                [FromUri] int voucherEntryId,
                                                [FromBody] VoucherEntryFields fields) {
      base.RequireBody(fields);

      using (var usecases = VoucherEditionUseCases.UseCaseInteractor()) {
        VoucherDto voucher = usecases.UpdateEntry(voucherId, voucherEntryId, fields);

        return new SingleObjectModel(base.Request, voucher);
      }
    }

    #endregion Web Apis

  }  // class VoucherEditionController

}  // namespace Empiria.FinancialAccounting.Vouchers.WebApi
