﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Reporting Services                           Component : Web Api                               *
*  Assembly : Empiria.FinancialAccounting.WebApi.dll       Pattern   : Controller                            *
*  Type     : ReportGenerationController                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Query web API used to generate financial accounting reports: financial, operational, fiscal.   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.WebApi;

using Empiria.FinancialAccounting.Reporting;
using Empiria.FinancialAccounting.Reporting.Adapters;

namespace Empiria.FinancialAccounting.WebApi.Reporting {

  /// <summary>Query web API used to generate financial accounting reports:
  /// financial, operational, fiscal.</summary>
  public class ReportGenerationController : WebApiController {

    #region Web Apis


    [HttpPost]
    [Route("v2/financial-accounting/reporting/{reportType:string}/export")]
    public SingleObjectModel ExportReportData([FromUri] string reportType,
                                              [FromBody] GenerateReportCommand command) {
      base.RequireBody(command);

      command.ReportType = reportType;

      using (var service = ReportingService.ServiceInteractor()) {
        FileReportDto fileReportDto = service.ExportReport(command);

        return new SingleObjectModel(this.Request, fileReportDto);
      }
    }


    [HttpPost]
    [Route("v2/financial-accounting/reporting/{reportType:string}/data")]
    public SingleObjectModel GenerateReport([FromUri] string reportType,
                                            [FromBody] GenerateReportCommand command) {
      base.RequireBody(command);

      command.ReportType = reportType;

      using (var service = ReportingService.ServiceInteractor()) {
        ReportDataDto reportData = service.GenerateReport(command);

        return new SingleObjectModel(this.Request, reportData);
      }
    }


    [HttpGet]
    [Route("v2/financial-accounting/reporting/report-types")]
    public CollectionModel GetReportTypes() {

      using (var service = ReportingService.ServiceInteractor()) {
        FixedList<ReportTypeDto> reportTypes = service.GetReportTypes();

        return new CollectionModel(this.Request, reportTypes);
      }
    }

    #endregion Web Apis

  } // class ReportGenerationController

} // namespace Empiria.FinancialAccounting.WebApi.Reporting