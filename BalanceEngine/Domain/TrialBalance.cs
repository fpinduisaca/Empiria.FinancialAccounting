﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Balance Engine                             Component : Domain Layer                            *
*  Assembly : FinancialAccounting.BalanceEngine.dll      Pattern   : Information Holder                      *
*  Type     : TrialBalance                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Contains the header and entries of a trial balance                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.FinancialAccounting.BalanceEngine.Adapters;

namespace Empiria.FinancialAccounting.BalanceEngine {

  /// <summary>Contains the header and entries of a trial balance.</summary>
  public class TrialBalance {

    #region Constructors and parsers

    internal TrialBalance(TrialBalanceCommand command,
                          FixedList<ITrialBalanceEntry> entries) {
      Assertion.AssertObject(command, "command");
      Assertion.AssertObject(entries, "entries");

      this.Command = command;
      this.Entries = entries;
    }


    internal FixedList<DataTableColumn> DataColumns() {
      switch (this.Command.TrialBalanceType) {
        case TrialBalanceType.AnaliticoDeCuentas:
          return TwoCurrenciesDataColumns();

        case TrialBalanceType.Balanza:
        case TrialBalanceType.BalanzaConAuxiliares:
        case TrialBalanceType.GeneracionDeSaldos:
        case TrialBalanceType.Saldos:
        case TrialBalanceType.SaldosPorAuxiliar:
        case TrialBalanceType.SaldosPorCuenta:
        case TrialBalanceType.SaldosPorCuentaYMayor:
          return TrialBalanceDataColumns();

        case TrialBalanceType.BalanzaValorizadaComparativa:
          return TwoBalancesComparativeDataColumns();

        default:
          throw Assertion.AssertNoReachThisCode(
                $"Unhandled trial balance type {this.Command.TrialBalanceType}.");
      }
    }


    private FixedList<DataTableColumn> TrialBalanceDataColumns() {
      List<DataTableColumn> columns = new List<DataTableColumn>();

      if (Command.ReturnLedgerColumn) {
        columns.Add(new DataTableColumn("ledgerNumber", "Cont", "text"));
      }

      columns.Add(new DataTableColumn("currencyCode", "Mon", "text"));

      if (Command.ReturnSubledgerAccounts) {
        columns.Add(new DataTableColumn("accountNumber", "Cuenta / Auxiliar", "text-nowrap"));
      } else {
        columns.Add(new DataTableColumn("accountNumber", "Cuenta", "text-nowrap"));
      }

      columns.Add(new DataTableColumn("sectorCode", "Sct", "text"));
      columns.Add(new DataTableColumn("accountName", "Nombre", "text"));
      columns.Add(new DataTableColumn("initialBalance", "Saldo anterior", "decimal"));
      columns.Add(new DataTableColumn("debit", "Cargos", "decimal"));
      columns.Add(new DataTableColumn("credit", "Abonos", "decimal"));
      columns.Add(new DataTableColumn("currentBalance", "Saldo actual", "decimal"));
      if (Command.ExchangeRateTypeUID != string.Empty) {
        columns.Add(new DataTableColumn("exchangeRate", "TC", "decimal"));
      }

      return columns.ToFixedList();
    }


    private FixedList<DataTableColumn> TwoBalancesComparativeDataColumns() {
      List<DataTableColumn> columns = new List<DataTableColumn>();

      columns.Add(new DataTableColumn("ledgerNumber", "Cont", "text"));
      columns.Add(new DataTableColumn("currencyCode", "Mon", "text"));
      columns.Add(new DataTableColumn("accountNumber", "Cta", "text-nowrap"));
      columns.Add(new DataTableColumn("accountNumber", "Scta", "text-nowrap"));
      columns.Add(new DataTableColumn("accountNumber", "Cuenta", "text-nowrap"));
      columns.Add(new DataTableColumn("sectorCode", "Sct", "text"));
      columns.Add(new DataTableColumn("accountNumber", "Auxiliar", "text-nowrap"));
      columns.Add(new DataTableColumn("subledgerAccountName", "Nombre", "text"));

      columns.Add(new DataTableColumn("totalBalance", $"{Command.FromDate:MMM_yyyy}", "decimal"));
      columns.Add(new DataTableColumn("exchangeRate", "Tc_Ini", "decimal"));
      columns.Add(new DataTableColumn("currentVal", $"{Command.FromDate:MMM}_VAL", "decimal"));
      columns.Add(new DataTableColumn("debit", "Cargos", "decimal"));
      columns.Add(new DataTableColumn("credit", "Abonos", "decimal"));

      columns.Add(new DataTableColumn("totalBalanceComparative", $"{Command.FromDateComparative:MMM_yyyy}", "decimal"));
      columns.Add(new DataTableColumn("exchangeRateComparative", "Tc_Fin", "decimal"));
      columns.Add(new DataTableColumn("totalValComparative", $"{Command.FromDate:MMM}_VAL", "decimal"));

      columns.Add(new DataTableColumn("AccountName", "Nom_Cta", "text"));
      columns.Add(new DataTableColumn("naturaleza", "Nat", "text"));
      columns.Add(new DataTableColumn("variation", "Variación", "decimal"));
      columns.Add(new DataTableColumn("variationByTC", "Variación por TC", "decimal"));
      columns.Add(new DataTableColumn("realVariation", "Variación por TC", "decimal"));

      return columns.ToFixedList();
    }


    private FixedList<DataTableColumn> TwoCurrenciesDataColumns() {
      List<DataTableColumn> columns = new List<DataTableColumn>();

      if (Command.ReturnLedgerColumn) {
        columns.Add(new DataTableColumn("ledgerNumber", "Cont", "text"));
      }

      columns.Add(new DataTableColumn("currencyCode", "Mon", "text"));

      if (Command.ReturnSubledgerAccounts) {
        columns.Add(new DataTableColumn("accountNumber", "Cuenta / Auxiliar", "text-nowrap"));
      } else {
        columns.Add(new DataTableColumn("accountNumber", "Cuenta", "text-nowrap"));
      }

      columns.Add(new DataTableColumn("sectorCode", "Sct", "text"));
      columns.Add(new DataTableColumn("accountName", "Nombre", "text"));
      columns.Add(new DataTableColumn("domesticBalance", "Saldo Mon. Nal.", "decimal"));
      columns.Add(new DataTableColumn("foreignBalance", "Saldo Mon. Ext.", "decimal"));
      columns.Add(new DataTableColumn("totalBalance", "Total", "decimal"));
      if (Command.ExchangeRateTypeUID != string.Empty) {
        //columns.Add(new DataTableColumn("exchangeRate", "TC", "decimal"));
      }

      return columns.ToFixedList();
    }

    #endregion Constructors and parsers

    #region Properties

    public TrialBalanceCommand Command {
      get;
    }


    public FixedList<ITrialBalanceEntry> Entries {
      get;
    }

    #endregion Properties

  } // class TrialBalance

} // namespace Empiria.FinancialAccounting.BalanceEngine
