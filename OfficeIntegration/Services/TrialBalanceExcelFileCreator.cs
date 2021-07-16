﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Office Integration                           Component : Excel Exporter                        *
*  Assembly : FinancialAccounting.OficeIntegration.dll     Pattern   : Service                               *
*  Type     : TrialBalanceExcelFileCreator                 License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Creates a Microsoft Excel file with trial balance information.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.FinancialAccounting.BalanceEngine;
using Empiria.FinancialAccounting.BalanceEngine.Adapters;

namespace Empiria.FinancialAccounting.OfficeIntegration {

  /// <summary>Creates a Microsoft Excel file with trial balance information.</summary>
  internal class TrialBalanceExcelFileCreator {

    private readonly ExcelTemplateConfig _templateConfig;
    private ExcelFile _excelFile;

    public TrialBalanceExcelFileCreator(ExcelTemplateConfig templateConfig) {
      Assertion.AssertObject(templateConfig, "templateConfig");

      _templateConfig = templateConfig;
    }


    internal ExcelFile CreateExcelFile(TrialBalanceDto trialBalance) {
      Assertion.AssertObject(trialBalance, "trialBalance");

      _excelFile = new ExcelFile(_templateConfig);

      _excelFile.Open();

      SetHeader(trialBalance.Command);

      SetTable(trialBalance);

      _excelFile.Save();

      _excelFile.Close();

      return _excelFile;
    }



    #region Private methods

    private void SetHeader(TrialBalanceCommand command) {
      _excelFile.SetCell($"A2", _templateConfig.Title);

      var subTitle = $"Del {command.FromDate.ToString("dd/MMM/yyyy")} " +
                     $"al {command.ToDate.ToString("dd/MMM/yyyy")}";

      if (command.ValuateBalances) {
        subTitle += $". Saldos valorizados al {command.ExchangeRateDate.ToString("dd/MMM/yyyy")}.";
      }

      _excelFile.SetCell($"A3", subTitle);
    }


    private void SetTable(TrialBalanceDto trialBalance) {
      switch (trialBalance.Command.TrialBalanceType) {
        case TrialBalanceType.AnaliticoDeCuentas:
          FillOutAnaliticoDeCuentas(trialBalance.Entries.Select(x => (TwoColumnsTrialBalanceEntryDto) x));
          return;
        case TrialBalanceType.SaldosPorCuentaYMayor:
          FillOutSaldosPorCuentayMayor(trialBalance.Entries.Select(x => (TrialBalanceEntryDto) x));
          return;
        case TrialBalanceType.Balanza:
        case TrialBalanceType.BalanzaConAuxiliares:
        case TrialBalanceType.Saldos:
        case TrialBalanceType.SaldosPorAuxiliar:
        case TrialBalanceType.SaldosPorCuenta:
          FillOutBalanza(trialBalance.Entries.Select(x => (TrialBalanceEntryDto) x));
          return;
        default:
          throw Assertion.AssertNoReachThisCode();
      }
    }


    private void FillOutAnaliticoDeCuentas(IEnumerable<TwoColumnsTrialBalanceEntryDto> entries) {
      int i = 5;

      foreach (var entry in entries) {
        _excelFile.SetCell($"A{i}", entry.LedgerNumber);
        if (entry.ItemType == TrialBalanceItemType.BalanceEntry) {
          _excelFile.SetCell($"B{i}", "*");
        }
        _excelFile.SetCell($"C{i}", entry.AccountNumber);
        _excelFile.SetCell($"D{i}", entry.AccountName);
        _excelFile.SetCell($"E{i}", entry.SectorCode);
        _excelFile.SetCell($"F{i}", entry.DomesticBalance);
        _excelFile.SetCell($"G{i}", entry.ForeignBalance);

        if (entry.ItemType != TrialBalanceItemType.BalanceEntry &&
            entry.ItemType != TrialBalanceItemType.BalanceSummary) {
          _excelFile.SetRowStyleBold(i);
        }
        i++;
      }
    }


    private void FillOutSaldosPorCuentayMayor(IEnumerable<TrialBalanceEntryDto> entries) {
      int i = 5;

      foreach (var entry in entries) {
        _excelFile.SetCell($"A{i}", entry.CurrencyCode);
        _excelFile.SetCell($"B{i}", entry.AccountNumber);
        if (entry.LedgerNumber.Length == 0) {
          _excelFile.SetCell($"C{i}", entry.AccountName);
          _excelFile.SetCell($"D{i}", "00");
          _excelFile.SetCell($"E{i}", "Todas");
        } else {
          _excelFile.SetCell($"D{i}", entry.LedgerNumber);
          _excelFile.SetCell($"E{i}", entry.AccountName);
        }
        _excelFile.SetCell($"F{i}", entry.InitialBalance);
        _excelFile.SetCell($"G{i}", entry.Debit);
        _excelFile.SetCell($"H{i}", entry.Credit);
        _excelFile.SetCell($"I{i}", entry.CurrentBalance);

        if (entry.LedgerNumber.Length == 0) {
          _excelFile.SetRowStyleBold(i);
        }
        i++;
      }
    }


    private void FillOutBalanza(IEnumerable<TrialBalanceEntryDto> entries) {
      int i = 5;

      foreach (var entry in entries) {
        _excelFile.SetCell($"A{i}", entry.LedgerNumber);
        _excelFile.SetCell($"B{i}", entry.CurrencyCode);
        if (entry.ItemType == TrialBalanceItemType.BalanceEntry) {
          _excelFile.SetCell($"C{i}", "*");
        }
        _excelFile.SetCell($"D{i}", entry.AccountNumber);
        _excelFile.SetCell($"E{i}", entry.AccountName);
        _excelFile.SetCell($"F{i}", entry.SectorCode);
        _excelFile.SetCell($"G{i}", entry.InitialBalance);
        _excelFile.SetCell($"H{i}", entry.Debit);
        _excelFile.SetCell($"I{i}", entry.Credit);
        _excelFile.SetCell($"J{i}", entry.CurrentBalance);

        if (entry.ItemType != TrialBalanceItemType.BalanceEntry &&
            entry.ItemType != TrialBalanceItemType.BalanceSummary) {
          _excelFile.SetRowStyleBold(i);
        }
        i++;
      }
    }

    #endregion Private methods

  }  // class TrialBalanceExcelFileCreator

}  // namespace Empiria.FinancialAccounting.OfficeIntegration
