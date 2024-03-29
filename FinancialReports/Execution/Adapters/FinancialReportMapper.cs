﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Reports                          Component : Interface adapters                      *
*  Assembly : FinancialAccounting.FinancialReports.dll   Pattern   : Mapper class                            *
*  Type     : FinancialReportMapper                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Methods used to map financial reports data.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.FinancialAccounting.FinancialReports.Adapters {

  /// <summary>Methods used to map financial reports data.</summary>
  static internal class FinancialReportMapper {

    #region Public mappers

    internal static FinancialReportDto Map(FinancialReport financialReport) {
      return new FinancialReportDto {
        Command = financialReport.Command,
        Columns = financialReport.DataColumns(),
        Entries = MapEntries(financialReport)
      };
    }

    static internal FinancialReportDto MapBreakdown(FinancialReport breakdownReport) {
      return new FinancialReportDto {
        Command = breakdownReport.Command,
        Columns = breakdownReport.BreakdownDataColumns(),
        Entries = MapBreakdownEntries(breakdownReport.Entries)
      };
    }

    #endregion Public mappers

    #region Private mappers

    static private FixedList<DynamicFinancialReportEntryDto> MapBreakdownEntries(FixedList<FinancialReportEntry> list) {
      var mappedItems = list.Select((x) => MapBreakdownEntry((FinancialReportBreakdownEntry) x));

      return new FixedList<DynamicFinancialReportEntryDto>(mappedItems);
    }


    static private DynamicFinancialReportEntryDto MapBreakdownEntry(FinancialReportBreakdownEntry entry) {
      dynamic o = new FinancialReportBreakdownEntryDto {
        UID = entry.GroupingRuleItem.UID,
        ItemCode = entry.GroupingRuleItem.Code,
        ItemName = entry.GroupingRuleItem.Name,
        SubledgerAccount = entry.GroupingRuleItem.SubledgerAccountNumber,
        SectorCode = entry.GroupingRuleItem.SectorCode,
        Operator = Convert.ToString((char) entry.GroupingRuleItem.Operator),
        GroupingRuleUID = entry.GroupingRuleItem.GroupingRule.UID,
      };

      SetTotalsFields(o, entry);
      return o;
    }


    static private FixedList<DynamicFinancialReportEntryDto> MapEntries(FinancialReport financialReport) {
      FinancialReportType reportType = financialReport.Command.GetFinancialReportType();

      switch (reportType.DesignType) {

        case FinancialReportDesignType.FixedRows:
          return MapToFixedRowsReport(financialReport.Entries);

        case FinancialReportDesignType.ConceptsIntegration:
          return MapToFixedRowsReportConceptsIntegration(financialReport.Entries);

        default:
          throw Assertion.AssertNoReachThisCode(
                $"Unhandled financial report type {financialReport.Command.FinancialReportType}.");
      }
    }


    static private FixedList<DynamicFinancialReportEntryDto> MapToFixedRowsReport(FixedList<FinancialReportEntry> list) {
      var mappedItems = list.Select((x) => MapToFixedRowsReport((FixedRowFinancialReportEntry) x));

      return new FixedList<DynamicFinancialReportEntryDto>(mappedItems);
    }


    static private FinancialReportEntryDto MapToFixedRowsReport(FixedRowFinancialReportEntry entry) {
      dynamic o = new FinancialReportEntryDto {
         UID = entry.Row.UID,
         ConceptCode = entry.GroupingRule.Code,
         Concept = entry.GroupingRule.Concept,
         GroupingRuleUID = entry.GroupingRule.UID,
         AccountsChartName = entry.GroupingRule.RulesSet.AccountsChart.Name,
         RulesSetName = entry.GroupingRule.RulesSet.Name,
      };

      SetTotalsFields(o, entry);

      return o;
    }

    static private FixedList<DynamicFinancialReportEntryDto> MapToFixedRowsReportConceptsIntegration(FixedList<FinancialReportEntry> list) {
      var mappedItems = list.Select((x) => MapToFixedRowsReportConceptsIntegration((FixedRowFinancialReportEntry) x));

      return new FixedList<DynamicFinancialReportEntryDto>(mappedItems);
    }

    static private FinancialReportEntryDto MapToFixedRowsReportConceptsIntegration(FixedRowFinancialReportEntry entry) {
      dynamic o = new FinancialReportEntryDto {
        UID = entry.Row.UID,
        ConceptCode = entry.GroupingRule.Code,
        Concept = entry.GroupingRule.Concept,
        GroupingRuleUID = entry.GroupingRule.UID,
        AccountsChartName = entry.GroupingRule.RulesSet.AccountsChart.Name,
        RulesSetName = entry.GroupingRule.RulesSet.Name
      };

      SetTotalsFields(o, entry);

      return o;
    }

    #endregion Private mappers

    #region Helpers

    static private void SetTotalsFields(DynamicFinancialReportEntryDto o, FinancialReportEntry entry) {
      o.SetTotalField(FinancialReportTotalField.DomesticCurrencyTotal,
                      entry.GetTotalField(FinancialReportTotalField.DomesticCurrencyTotal));
      o.SetTotalField(FinancialReportTotalField.ForeignCurrencyTotal,
                      entry.GetTotalField(FinancialReportTotalField.ForeignCurrencyTotal));
      o.SetTotalField(FinancialReportTotalField.Total,
                      entry.GetTotalField(FinancialReportTotalField.Total));
    }

    #endregion Helpers

  } // class FinancialReportMapper

} // namespace Empiria.FinancialAccounting.FinancialReports.Adapters
