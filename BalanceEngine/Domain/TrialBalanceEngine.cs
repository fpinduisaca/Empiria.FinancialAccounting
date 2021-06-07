﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Balance Engine                             Component : Domain Layer                            *
*  Assembly : FinancialAccounting.BalanceEngine.dll      Pattern   : Service provider                        *
*  Type     : TrialBalanceEngine                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides services to generate a trial balance.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Linq;

using Empiria.Collections;
using Empiria.FinancialAccounting.BalanceEngine.Adapters;
using Empiria.FinancialAccounting.BalanceEngine.Data;

namespace Empiria.FinancialAccounting.BalanceEngine {

  public enum TrialBalanceType {

    BalancesByAccount,

    BalancesBySubledgerAccount,

    Traditional,

    TraditionalWithSubledgerAccounts,

  }


  public enum BalancesType {

    AllAccounts,

    WithCurrentBalance,

    WithCurrentBalanceOrMovements,

    WithMovements

  }


  /// <summary>Provides services to generate a trial balance.</summary>
  internal class TrialBalanceEngine {

    internal TrialBalanceEngine(TrialBalanceCommand command) {
      Assertion.AssertObject(command, "command");

      this.Command = command;
    }


    public TrialBalanceCommand Command {
      get;
    }


    internal TrialBalance BuildTrialBalance() {
      TrialBalanceCommandData commandData = this.Command.MapToTrialBalanceCommandData();

      FixedList<TrialBalanceEntry> postingEntries =
                              TrialBalanceDataService.GetTrialBalanceEntries(commandData);

      if (Command.ValuateBalances) {
        postingEntries = ValuateToExchangeRate(postingEntries);

        if (Command.ConsolidateBalancesToTargetCurrency) {
          postingEntries = ConsolidateToTargetCurrency(postingEntries);
        }
      }

      List <TrialBalanceEntry> summaryEntries = GenerateSummaryEntries(postingEntries);

      FixedList<TrialBalanceEntry> trialBalance = CombineSummaryAndPostingEntries(summaryEntries,
                                                                                  postingEntries);

      trialBalance = RestrictLevels(trialBalance);

      return new TrialBalance(Command, trialBalance);
    }


    #region Private methods

    private FixedList<TrialBalanceEntry> CombineSummaryAndPostingEntries(List<TrialBalanceEntry> summaryEntries,
                                                                         FixedList<TrialBalanceEntry> postingEntries) {
      List<TrialBalanceEntry> returnedEntries = new List<TrialBalanceEntry>(postingEntries);

      returnedEntries.AddRange(summaryEntries);

      returnedEntries = returnedEntries.OrderBy(a => a.Ledger.Number)
                                       .ThenBy(a => a.Currency.Code)
                                       .ThenByDescending(a => a.Account.DebtorCreditor)
                                       .ThenBy(a => a.Account.Number)
                                       .ThenBy(a => a.Sector.Code)
                                       .ThenBy(a => a.SubledgerAccountId)
                                       .ToList();

      return returnedEntries.ToFixedList();
    }


    private FixedList<TrialBalanceEntry> ConsolidateToTargetCurrency(FixedList<TrialBalanceEntry> trialBalance) {
      var targetCurrency = Currency.Parse(Command.ValuateToCurrrencyUID);

      var summaryEntries = new EmpiriaHashTable<TrialBalanceEntry>();

      foreach (var entry in trialBalance) {
        string hash = $"{entry.Account.Number}||{entry.Sector.Code}||{targetCurrency.Id}||{entry.Ledger.Id}";

        if (entry.Currency.Equals(targetCurrency)) {
          summaryEntries.Insert(hash, entry);
        } else if (summaryEntries.ContainsKey(hash)) {
          summaryEntries[hash].Sum(entry);
        } else {
          entry.Currency = targetCurrency;
          summaryEntries.Insert(hash, entry);
        }
      }

      return summaryEntries.Values.ToList()
                                  .ToFixedList();
    }


    private FixedList<TrialBalanceEntry> ValuateToExchangeRate(FixedList<TrialBalanceEntry> entries) {
      var exchangeRateType = ExchangeRateType.Parse(Command.ExchangeRateTypeUID);

      FixedList<ExchangeRate> exchageRates = ExchangeRate.GetList(exchangeRateType, Command.ExchangeRateDate);

      foreach (var entry in entries) {
        var exchangeRate = exchageRates.FirstOrDefault(a => a.FromCurrency.Code == Command.ValuateToCurrrencyUID &&
                                                            a.ToCurrency.Code == entry.Currency.Code);

        Assertion.AssertObject(exchangeRate, $"No hay tipo de cambio para la moneda {entry.Currency.FullName}");

        entry.MultiplyBy(exchangeRate.Value);
      }
      return entries;
    }


    private List<TrialBalanceEntry> GenerateSummaryEntries(FixedList<TrialBalanceEntry> entries) {
      var summaryEntries = new EmpiriaHashTable<TrialBalanceEntry>(entries.Count);

      foreach (var entry in entries) {
        Account currentParent;

        if (this.Command.ReturnSubledgerAccounts) {
          currentParent = entry.Account;

        } else if (this.Command.DoNotReturnSubledgerAccounts && entry.Account.HasParent) {
          currentParent = entry.Account.GetParent();

        } else if (this.Command.DoNotReturnSubledgerAccounts && entry.Account.NotHasParent) {
          continue;
        } else {
          throw Assertion.AssertNoReachThisCode();
        }

        while (true) {

          GenerateOrIncreaseSummaryEntry(summaryEntries, entry, currentParent, entry.Sector);

          if (!currentParent.HasParent && entry.Sector.Code != "00") {
            GenerateOrIncreaseSummaryEntry(summaryEntries, entry, currentParent, Sector.Empty);
            break;

          } else if (!currentParent.HasParent) {
            break;

          } else {
            currentParent = currentParent.GetParent();
          }

        } // while

      } // foreach

      return summaryEntries.Values.ToList();
    }


    private void GenerateOrIncreaseSummaryEntry(EmpiriaHashTable<TrialBalanceEntry> summaryEntries,
                                                TrialBalanceEntry entry,
                                                Account targetAccount, Sector targetSector) {

      string hash = $"{targetAccount.Number}||{targetSector.Code}||{entry.Currency.Id}||{entry.Ledger.Id}";

      TrialBalanceEntry summaryEntry;

      summaryEntries.TryGetValue(hash, out summaryEntry);

      if (summaryEntry == null) {

        summaryEntry = new TrialBalanceEntry() {
          Ledger = entry.Ledger,
          Currency = entry.Currency,
          Sector = targetSector,
          Account = targetAccount,
          ItemType = "BalanceSummary"
        };
        summaryEntry.Sum(entry);

        summaryEntries.Insert(hash, summaryEntry);

      } else {
        summaryEntry.Sum(entry);
      }
    }


    private FixedList<TrialBalanceEntry> RestrictLevels(FixedList<TrialBalanceEntry> entries) {
      if (Command.Level == 0) {
        return entries;
      }

      if (Command.DoNotReturnSubledgerAccounts) {
        return entries.FindAll(x => x.Level <= Command.Level);
      } else if (Command.ReturnSubledgerAccounts) {
        return entries.FindAll(x => x.Level <= Command.Level || x.LedgerAccountId != 0);
      } else {
        throw Assertion.AssertNoReachThisCode();
      }
    }

    #endregion Private methods

  } // class TrialBalanceEngine

} // namespace Empiria.FinancialAccounting.BalanceEngine
