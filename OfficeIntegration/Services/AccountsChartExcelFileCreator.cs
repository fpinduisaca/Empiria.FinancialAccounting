﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Office Integration                           Component : Excel Exporter                        *
*  Assembly : FinancialAccounting.OficeIntegration.dll     Pattern   : Service                               *
*  Type     : ExcelExporter                                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Main service to export accounting information to Microsoft Excel.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/


using System;
using Empiria.FinancialAccounting.Adapters;

namespace Empiria.FinancialAccounting.OfficeIntegration {

  internal class AccountsChartExcelFileCreator {

    private readonly ExcelTemplateConfig _templateConfig;

    public AccountsChartExcelFileCreator(ExcelTemplateConfig templateConfig) {
      Assertion.AssertObject(templateConfig, "templateConfig");

      _templateConfig = templateConfig;
    }


    internal ExcelFile CreateExcelFile(AccountsChartDto accountsChart) {
      Assertion.AssertObject(accountsChart, "accountsChart");

      var excelFile = new ExcelFile(_templateConfig);

      excelFile.Open();

      excelFile.SetCell($"A2", "Catálogo de cuentas");

      FillOut(accountsChart, excelFile);

      excelFile.Save();

      excelFile.Close();

      return excelFile;
    }

    #region Private methods


    private void FillOut(AccountsChartDto accountsChart, ExcelFile excelFile) {
      int i = 5;

      foreach (var account in accountsChart.Accounts) {
        excelFile.SetCell($"C{i}", account.Number);
        excelFile.SetCell($"D{i}", account.Name);

        if (account.Role != AccountRole.Sumaria &&
           (account.Role == AccountRole.Sectorizada && accountsChart.WithSectors)) {
          excelFile.SetCell($"E{i}", "*");
        }
        excelFile.SetCell($"F{i}", account.Sector);
        excelFile.SetCell($"G{i}", account.Role.ToString());
        excelFile.SetCell($"H{i}", account.Type);
        excelFile.SetCell($"I{i}", account.DebtorCreditor.ToString());
        excelFile.SetCell($"J{i}", account.StartDate);
        i++;
      }

      if (!accountsChart.WithSectors) {
        excelFile.RemoveColumn("F");
      }
    }

    #endregion Private methods

  }  // class AccountsChartExcelFileCreator

}  // namespace Empiria.FinancialAccounting.OfficeIntegration