﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Vouchers Management                          Component : Web Api                               *
*  Assembly : Empiria.FinancialAccounting.WebApi.dll       Pattern   : Query Controller                      *
*  Type     : VouchersDataController                       License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Query web API used to retrive accounting vouchers related data.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.WebApi;

using Empiria.FinancialAccounting.Adapters;
using Empiria.FinancialAccounting.Vouchers.UseCases;

namespace Empiria.FinancialAccounting.WebApi.Vouchers {

  /// <summary>Query web API used to retrive accounting vouchers related data.</summary>
  public class VoucherDataController : WebApiController {

    #region Web Apis


    [HttpGet]
    [Route("v2/financial-accounting/vouchers/editors/")]
    public CollectionModel GetVoucherEditors([FromUri] string keywords) {

      using (var usecases = VoucherDataUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> editors = usecases.VoucherEditors(keywords);

        return new CollectionModel(base.Request, editors);
      }
    }


    [HttpGet]
    [Route("v2/financial-accounting/vouchers/event-types")]
    public CollectionModel GetEventTypes() {

      using (var usecases = VoucherDataUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> eventTypes = usecases.EventTypes();

        return new CollectionModel(base.Request, eventTypes);
      }
    }


    [HttpGet]
    [Route("v2/financial-accounting/vouchers/functional-areas")]
    public CollectionModel GetFunctionalAreas() {

      using (var usecases = VoucherDataUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> functionalAreas = usecases.FunctionalAreas();

        return new CollectionModel(base.Request, functionalAreas);
      }
    }



    [HttpGet]
    [Route("v2/financial-accounting/vouchers/transactional-systems")]
    public CollectionModel GetTransactionalSystems() {

      using (var usecases = VoucherDataUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> systems = usecases.TransactionalSystems();

        return new CollectionModel(base.Request, systems);
      }
    }


    [HttpGet]
    [Route("v2/financial-accounting/vouchers/opened-accounting-dates/{accountsChartUID:guid}")]
    public CollectionModel GetOpenedAccountingDates([FromUri] string accountsChartUID) {

      using (var usecases = VoucherDataUseCases.UseCaseInteractor()) {
        FixedList<DateTime> openedAccountingDates = usecases.OpenedAccountingDates(accountsChartUID);

        return new CollectionModel(base.Request, openedAccountingDates);
      }
    }


    [HttpGet]
    [Route("v2/financial-accounting/vouchers/transaction-types")]
    public CollectionModel GetTransactionTypes() {

      using (var usecases = VoucherDataUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> transactionTypes = usecases.TransactionTypes();

        return new CollectionModel(base.Request, transactionTypes);
      }
    }


    [HttpGet]
    [Route("v2/financial-accounting/vouchers/voucher-types")]
    public CollectionModel GetVoucherTypes() {

      using (var usecases = VoucherDataUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> vouchersTypes = usecases.VoucherTypes();

        return new CollectionModel(base.Request, vouchersTypes);
      }
    }


    [HttpGet]
    [Route("v2/financial-accounting/vouchers/{voucherId:int}/search-accounts-for-edition")]
    public CollectionModel SearchAccountsForVoucher([FromUri] long voucherId,
                                                    [FromUri] string keywords) {

      using (var usecases = VoucherDataUseCases.UseCaseInteractor()) {
        FixedList<LedgerAccountDto> accounts = usecases.SearchAccountsForVoucherEdition(voucherId, keywords);

        return new CollectionModel(base.Request, accounts);
      }
    }


    [HttpGet]
    [Route("v2/financial-accounting/vouchers/{voucherId:int}/search-subledger-accounts-for-edition/{accountId:int}")]
    public CollectionModel SearchSubledgerAccountsForVoucher([FromUri] long voucherId,
                                                             [FromUri] int accountId,
                                                             [FromUri] string keywords) {

      using (var usecases = VoucherDataUseCases.UseCaseInteractor()) {
        FixedList<SubledgerAccountDescriptorDto> accounts =
                              usecases.SearchSubledgerAccountsForVoucherEdition(voucherId, accountId, keywords);

        return new CollectionModel(base.Request, accounts);
      }
    }

    #endregion Web Apis

  }  // class VouchersDataController

}  // namespace Empiria.FinancialAccounting.WebApi.Vouchers
