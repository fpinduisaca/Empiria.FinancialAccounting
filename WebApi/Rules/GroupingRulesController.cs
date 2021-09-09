﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounting Rules                   Component : Web Api                               *
*  Assembly : Empiria.FinancialAccounting.WebApi.dll       Pattern   : Query Controller                      *
*  Type     : GroupingRulesController                      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Query web API used to retrive financial accounting grouping rules.                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.WebApi;

using Empiria.FinancialAccounting.Rules.UseCases;
using Empiria.FinancialAccounting.Rules.Adapters;

namespace Empiria.FinancialAccounting.WebApi.Rules {

  /// <summary>Query web API used to retrive financial accounting grouping rules.</summary>
  public class GroupingRulesController : WebApiController {

    #region Web Apis

    [HttpGet]
    [Route("v2/financial-accounting/rules/grouping-rules/{rulesSetUID:guid}")]
    public CollectionModel GetGroupingRules([FromUri] string rulesSetUID) {

      using (var usecases = GroupingRulesUseCases.UseCaseInteractor()) {
        FixedList<GroupingRuleDto> rules = usecases.GroupingRules(rulesSetUID);

        return new CollectionModel(base.Request, rules);
      }
    }

    [HttpGet]
    [Route("v2/financial-accounting/rules/rules-sets-for/{accountsChartUID:guid}/grouping-rules")]
    public CollectionModel GetRulesSetsFor([FromUri] string accountsChartUID) {

      using (var usecases = GroupingRulesUseCases.UseCaseInteractor()) {
        FixedList<NamedEntityDto> rules = usecases.GroupingRulesSetsFor(accountsChartUID);

        return new CollectionModel(base.Request, rules);
      }
    }

    #endregion Web Apis

  }  // class GroupingRulesController

}  // namespace Empiria.FinancialAccounting.WebApi.Rules