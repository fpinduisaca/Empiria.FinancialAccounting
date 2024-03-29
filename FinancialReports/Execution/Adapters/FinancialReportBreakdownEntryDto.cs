﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Reports                          Component : Interface adapters                      *
*  Assembly : FinancialAccounting.FinancialReports.dll   Pattern   : Data Transfer Object                    *
*  Type     : FinancialReportBreakdownEntryDto           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return the entries of a financial report breakdown.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.FinancialAccounting.Rules;

namespace Empiria.FinancialAccounting.FinancialReports.Adapters {

  /// <summary>Output DTO used to return the entries of a financial report breakdown.</summary>
  public class FinancialReportBreakdownEntryDto : DynamicFinancialReportEntryDto {

    public string UID {
      get; internal set;
    }

    public GroupingRuleItemType Type {
      get;
      internal set;
    }

    public string GroupingRuleUID {
      get; internal set;
    }

    public string ItemName {
      get;
      internal set;
    }

    public string ItemCode {
      get;
      internal set;
    }

    public string SubledgerAccount {
      get;
      internal set;
    }

    public string SectorCode {
      get;
      internal set;
    }

    public string Operator {
      get;
      internal set;
    }

    public override IEnumerable<string> GetDynamicMemberNames() {
      List<string> members = new List<string>();

      members.Add("UID");
      members.Add("Type");
      members.Add("GroupingRuleUID");
      members.Add("ItemName");
      members.Add("ItemCode");
      members.Add("SubledgerAccount");
      members.Add("SectorCode");
      members.Add("Operator");

      members.AddRange(base.GetDynamicMemberNames());

      return members;
    }

  } // class FinancialReportBreakdownEntryDto

} // namespace Empiria.FinancialAccounting.FinancialReports.Adapters
