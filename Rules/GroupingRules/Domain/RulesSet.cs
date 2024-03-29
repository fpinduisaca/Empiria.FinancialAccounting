﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounting Rules                 Component : Domain Layer                            *
*  Assembly : FinancialAccounting.Rules.dll              Pattern   : Empiria Data Object                     *
*  Type     : RulesSet                                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds a set of financial accounting rules.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.FinancialAccounting.Rules.Data;

namespace Empiria.FinancialAccounting.Rules {

  /// <summary>Holds a set of financial accounting rules.</summary>
  public class RulesSet : GeneralObject {

    #region Fields

    private Lazy<FixedList<GroupingRule>> _groupingRules;

    private Lazy<FixedList<GroupingRuleItem>> _groupingRulesItems;

    #endregion Fields

    #region Constructors and parsers

    protected RulesSet() {
      // Required by Empiria Framework.
    }

    static public RulesSet Parse(int id) {
      return BaseObject.ParseId<RulesSet>(id);
    }


    static public RulesSet Parse(string uid) {
      return BaseObject.ParseKey<RulesSet>(uid);
    }


    static public FixedList<RulesSet> GetList() {
      return BaseObject.GetList<RulesSet>()
                       .ToFixedList();
    }

    static public FixedList<RulesSet> GetList(AccountsChart accountsChart) {
      var list = GetList();

      return list.FindAll(x => x.AccountsChart.Equals(accountsChart));
    }

    static public RulesSet Empty {
      get {
        return RulesSet.ParseEmpty<RulesSet>();
      }
    }

    protected override void OnLoad() {
      base.OnLoad();

      if (this.IsEmptyInstance) {
        return;
      }

      _groupingRules = new Lazy<FixedList<GroupingRule>>(() => GroupingRulesData.GetGroupingRules(this));
      _groupingRulesItems = new Lazy<FixedList<GroupingRuleItem>>(() => GroupingRulesData.GetGroupingRulesItems(this));
    }


    #endregion Constructors and parsers

    #region Properties

    public AccountsChart AccountsChart {
      get {
        return base.ExtendedDataField.Get<AccountsChart>("accountsChartId");
      }
    }


    public string Code {
      get {
        return base.ExtendedDataField.Get<string>("code");
      }
    }

    #endregion Properties

    #region Methods

    public FixedList<GroupingRule> GetGroupingRules() {
      return _groupingRules.Value;
    }


    internal FixedList<GroupingRuleItem> GetGroupingRuleItems(GroupingRule groupingRule) {
      return _groupingRulesItems.Value.FindAll(x => x.GroupingRule.Equals(groupingRule));
    }


    internal FixedList<GroupingRuleItem> GetGroupingRulesRoots() {
      return _groupingRulesItems.Value.FindAll(x => x.GroupingRule.IsEmptyInstance);
    }


    internal GroupingRulesTree GetGroupingRulesTree() {
      return new GroupingRulesTree(this);
    }

    #endregion Methods

  } // class RulesSet

}  // namespace Empiria.FinancialAccounting.Rules
