﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras Integration Services                Component : Excel Reports                         *
*  Assembly : FinancialAccounting.BanobrasIntegration.dll  Pattern   : Service                               *
*  Type     : ExcelExporter                                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Main service to export accounting information to Microsoft Excel.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using Empiria.FinancialAccounting.Adapters;
using Empiria.FinancialAccounting.BalanceEngine.Adapters;

using Empiria.FinancialAccounting.BanobrasIntegration.ExcelReports.Adapters;

namespace Empiria.FinancialAccounting.BanobrasIntegration.ExcelReports {

  /// <summary>Main service to export accounting information to Microsoft Excel.</summary>
  public class ExcelExporter {

    public ExcelFileDto Export(TrialBalanceDto trialBalance, TrialBalanceCommand command) {
      Assertion.AssertObject(trialBalance, "trialBalance");
      Assertion.AssertObject(command, "command");

      var templateUID = $"TrialBalanceTemplate.{trialBalance.Command.TrialBalanceType}";

      var templateConfig = ExcelTemplateConfig.Parse(templateUID);

      var creator = new TrialBalanceExcelFileCreator(templateConfig);

      ExcelFile excelFile = creator.CreateExcelFile(trialBalance);

      return ExcelFileMapper.Map(excelFile);
    }


    public ExcelFileDto Export(AccountsChartDto accountsChart,
                               AccountsSearchCommand searchCommand) {
      Assertion.AssertObject(accountsChart, "accountsChart");
      Assertion.AssertObject(searchCommand, "searchCommand");

      var templateUID = "AccountsChartTemplate";

      var templateConfig = ExcelTemplateConfig.Parse(templateUID);

      var creator = new AccountsChartExcelFileCreator(templateConfig);

      ExcelFile excelFile = creator.CreateExcelFile(accountsChart);

      return ExcelFileMapper.Map(excelFile);
    }


    public ExcelFileDto Export(StoredBalanceSetDto balanceSet) {
      Assertion.AssertObject(balanceSet, "balanceSet");

      var templateUID = "BalanceSetTemplate";

      var templateConfig = ExcelTemplateConfig.Parse(templateUID);

      var creator = new StoredBalanceSetExcelFileCreator(templateConfig);

      ExcelFile excelFile = creator.CreateExcelFile(balanceSet);

      return ExcelFileMapper.Map(excelFile);
    }

  }  // class ExcelExporter

} // namespace Empiria.FinancialAccounting.BanobrasIntegration.ExcelReports