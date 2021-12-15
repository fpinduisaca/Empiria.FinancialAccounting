﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Balance Engine                             Component : Interface adapters                      *
*  Assembly : FinancialAccounting.Reporting.dll      Pattern   : Type Extension methods                  *
*  Type     : VouchersByAccountCommandExtensions         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Type extension methods for VouchersByAccountCommand.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Linq;
using Empiria.FinancialAccounting.Reporting.Data;

namespace Empiria.FinancialAccounting.Reporting.Adapters {

  /// <summary>Type extension methods for VouchersByAccountCommand.</summary>
  internal class VouchersByAccountCommandExtensions {

    private AccountStatementCommand _accountStatementCommand;
    internal VouchersByAccountCommandExtensions(AccountStatementCommand accountStatementCommand) {
      _accountStatementCommand = accountStatementCommand;
    }

    #region Public methods

    internal VouchersByAccountCommandData MapToVouchersByAccountCommandData() {
      var commandData = new VouchersByAccountCommandData();
      var accountsChart = AccountsChart.Parse(_accountStatementCommand.Command.AccountsChartUID);


      //commandData.Fields = GetFields();
      commandData.AccountsChartId = accountsChart.Id;
      commandData.FromDate = _accountStatementCommand.Command.InitialPeriod.FromDate;
      commandData.ToDate = _accountStatementCommand.Command.InitialPeriod.ToDate;
      commandData.Filters = GetFilters();
      //commandData.Grouping = GetGroupingClause();
      return commandData;
    }

    private string GetGroupingClause() {
      return string.Empty;
    }

    #endregion


    #region Private methods

    private string GetFilters() {
      string accountRangeFilter = GetAccountFilter();
      string ledgerFilter = GetLedgerFilter();
      string currencyFilter = GetCurrencyFilter();
      string subledgerAccountFilter = GetSubledgerAccountFilter();

      var filter = new Filter(accountRangeFilter);

      filter.AppendAnd(ledgerFilter);
      filter.AppendAnd(currencyFilter);
      filter.AppendAnd(subledgerAccountFilter);

      return filter.ToString().Length > 0 ? $"AND {filter}" : "";
    }

    private string GetSubledgerAccountFilter() {
      if (_accountStatementCommand.Entry.SubledgerAccountNumber.Length == 0 || 
          _accountStatementCommand.Entry.SubledgerAccountNumber == "0") {
        return string.Empty;
      }
      return $"NUMERO_CUENTA_AUXILIAR = '{_accountStatementCommand.Entry.SubledgerAccountNumber}'";
    }

    private string GetAccountFilter() {
      if (_accountStatementCommand.Entry.AccountNumber.Length != 0) {
        return $"NUMERO_CUENTA_ESTANDAR = '{_accountStatementCommand.Entry.AccountNumber}'";
      } else {
        return string.Empty;
      }
    }

    private string GetCurrencyFilter() {
      if (_accountStatementCommand.Entry.CurrencyCode.Length == 0) {
        return string.Empty;
      }
      //int[] currencyIds = _accountStatementCommand.Entry.CurrencyCode
      //                    .Select(uid => Currency.Parse(uid).Id).ToArray();
      int currencyId = Currency.Parse(_accountStatementCommand.Entry.CurrencyCode).Id;
      return $"ID_MONEDA IN ({String.Join(", ", currencyId)})";
    }

    private string GetLedgerFilter() {
      if (_accountStatementCommand.Command.Ledgers.Length == 0) {
        return string.Empty;
      }

      int[] ledgerIds = _accountStatementCommand.Command.Ledgers
                        .Select(uid => Ledger.Parse(uid).Id).ToArray();

      return $"ID_MAYOR IN ({String.Join(", ", ledgerIds)})";
    }

    static private string GetFields() {
      return "ID_MAYOR, ID_MONEDA, ID_CUENTA_ESTANDAR, ID_SECTOR, ID_CUENTA_AUXILIAR, " +
             "ID_TRANSACCION, SUM(DEBE) AS DEBE, SUM(HABER) AS HABER";
    }

    #endregion



  } // class VouchersByAccountCommandExtensions

} // namespace Empiria.FinancialAccounting.Reporting.Adapters