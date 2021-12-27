﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Reporting Services                         Component : Domain Layer                            *
*  Assembly : FinancialAccounting.Reporting.dll          Pattern   : Helper methods                          *
*  Type     : VouchersByAccountHelper                       License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Helper methods to build vouchers by account information.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Linq;
using Empiria.Collections;
using Empiria.FinancialAccounting.BalanceEngine;
using Empiria.FinancialAccounting.Reporting.Adapters;
using Empiria.FinancialAccounting.Reporting.Data;
using Empiria.FinancialAccounting.Reporting.Domain;

namespace Empiria.FinancialAccounting.Reporting {

  /// <summary>Helper methods to build vouchers by account information.</summary>
  internal class AccountStatementHelper {

    private readonly AccountStatementCommand AccountStatementCommand;

    internal AccountStatementHelper(AccountStatementCommand accountStatementCommand) {
      Assertion.AssertObject(accountStatementCommand, "accountStatementCommand");

      AccountStatementCommand = accountStatementCommand;
    }


    #region Public methods


    internal FixedList<AccountStatementEntry> CombineInitialAccountBalanceWithVouchers(
                                                FixedList<AccountStatementEntry> orderingVouchers,
                                                AccountStatementEntry initialAccountBalance) {

      var totalBalanceAndVouchers = new List<AccountStatementEntry>();
      if (initialAccountBalance != null) {
        totalBalanceAndVouchers.Add(initialAccountBalance);
      }
      totalBalanceAndVouchers.AddRange(orderingVouchers);

      return totalBalanceAndVouchers.ToFixedList();
    }


    internal FixedList<AccountStatementEntry> GetOrderingVouchers(
                                              FixedList<AccountStatementEntry> voucherEntries) {

      List<AccountStatementEntry> returnedVouchers = voucherEntries
                                                      .OrderBy(a => a.Ledger.Number)
                                                      .ThenBy(a => a.Currency.Code)
                                                      .ThenBy(a => a.Account.Number)
                                                      .ThenBy(a => a.Sector.Code)
                                                      .ThenBy(a => a.SubledgerAccountNumber)
                                                      .ThenBy(a => a.VoucherNumber)
                                                      .ThenBy(a => a.VoucherEntryId).ToList();
      return returnedVouchers.ToFixedList();
    }


    internal AccountStatementEntry GetInitialOrCurrentAccountBalance(
                                      decimal balance, bool isCurrentBalance = false) {

      var initialBalanceEntry = new AccountStatementEntry();

      initialBalanceEntry.Ledger = Ledger.Empty;
      initialBalanceEntry.Currency = Currency.Empty;
      initialBalanceEntry.Account = StandardAccount.Empty;
      initialBalanceEntry.Sector = Sector.Empty;
      initialBalanceEntry.SubledgerAccountNumber = "";
      initialBalanceEntry.VoucherNumber = "";
      initialBalanceEntry.Concept = "";
      initialBalanceEntry.CurrentBalance = balance;
      initialBalanceEntry.ItemType = TrialBalanceItemType.Total;
      initialBalanceEntry.IsCurrentBalance = isCurrentBalance;

      return initialBalanceEntry;
    }


    internal string GetTitle() {
      var accountNumber = AccountStatementCommand.Entry.AccountNumberForBalances;
      var accountName = AccountStatementCommand.Entry.AccountName;
      var subledgerAccountNumber = AccountStatementCommand.Entry.SubledgerAccountNumber;

      if (accountNumber != "" &&
          subledgerAccountNumber.Length > 1) {

        accountName = accountName.Length > 0 ? ": " + accountName : "";

        return $"{accountNumber} {accountName} ({subledgerAccountNumber})";

      } else if (accountNumber != "") {

        return $"{accountNumber}" +
               $": {accountName}";

      } else if (accountNumber.Length == 0 && subledgerAccountNumber.Length > 1) {

        return $"{subledgerAccountNumber}";

      } else {
        return ".";
      }

    }


    internal FixedList<AccountStatementEntry> GetVouchersListWithCurrentBalance(
                                                FixedList<AccountStatementEntry> orderingVouchers) {
      
      List<AccountStatementEntry> returnedVouchersWithCurrentBalance =
                                    new List<AccountStatementEntry>(orderingVouchers).ToList();

      decimal initialBalance = AccountStatementCommand.Entry.InitialBalance;
      decimal currentBalance = initialBalance;

      foreach (var voucher in returnedVouchersWithCurrentBalance) {
        if (voucher.DebtorCreditor == "D") {
          voucher.CurrentBalance = currentBalance + (voucher.Debit - voucher.Credit);
          currentBalance = currentBalance + (voucher.Debit - voucher.Credit);
        } else {
          voucher.CurrentBalance = currentBalance + (voucher.Credit - voucher.Debit);
          currentBalance = currentBalance + (voucher.Credit - voucher.Debit);
        }
      }

      AccountStatementEntry voucherWithCurrentBalance = GetInitialOrCurrentAccountBalance(
                                                          currentBalance, true);

      if (voucherWithCurrentBalance != null) {
        returnedVouchersWithCurrentBalance.Add(voucherWithCurrentBalance);
      }

      return returnedVouchersWithCurrentBalance.ToFixedList();
    }


    internal FixedList<AccountStatementEntry> GetVoucherEntries() {

      AccountStatementCommandData commandData = VouchersByAccountCommandDataMapped();

      return AccountStatementDataService.GetVouchersByAccountEntries(commandData);
    }


    private AccountStatementCommandData VouchersByAccountCommandDataMapped() {

      var commandExtensions = new AccountStatementCommandExtensions(AccountStatementCommand);
      AccountStatementCommandData commandData = commandExtensions.MapToVouchersByAccountCommandData();

      return commandData;
    }

    #endregion Public methods


    #region Private methods



    #endregion Private methods

  } // class VouchersByAccountHelper

} // namespace Empiria.FinancialAccounting.Reporting.Domain