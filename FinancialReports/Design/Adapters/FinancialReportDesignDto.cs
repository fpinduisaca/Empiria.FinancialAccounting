﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Reports                          Component : Interface adapters                      *
*  Assembly : FinancialAccounting.FinancialReports.dll   Pattern   : Data Transfer Object                    *
*  Type     : FinancialReportDesignDto                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return financial reports designs.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.FinancialAccounting.FinancialReports.Adapters {

  /// <summary>Output DTO used to return financial reports designs.</summary>
  public class FinancialReportDesignDto {

    public FixedList<FinancialReportRowDto> Rows {
      get; internal set;
    } = new FixedList<FinancialReportRowDto>();


    public FixedList<DataTableColumn> Columns {
      get; internal set;
    } = new FixedList<DataTableColumn>();


  }  // class FinancialReportDesignDto

} // namespace Empiria.FinancialAccounting.FinancialReports.Adapters
