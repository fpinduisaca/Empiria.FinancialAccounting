﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Accounts Chart                             Component : Test cases                              *
*  Assembly : Empiria.FinancialAccounting.Tests.dll      Pattern   : Domain tests                            *
*  Type     : AccountsChartTests                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Test cases for retrieving accounts from the accounts chart.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Xunit;

namespace Empiria.FinancialAccounting.Tests {

  /// <summary>Test cases for retrieving accounts from the accounts chart.</summary>
  public class AccountsChartTests {

    #region Facts

    [Fact]
    public void Should_Search_Accounts_In_An_AccountsChart() {
      var chart = AccountsChart.Parse(TestingConstants.ACCOUNTS_CHART_UID);

      Account account = chart.GetAccount(TestingConstants.ACCOUNT_NUMBER);

      Assert.Equal(TestingConstants.ACCOUNT_NUMBER, account.Number);
      Assert.Equal(TestingConstants.ACCOUNT_NAME, account.Name);
    }


    [Fact]
    public void Should_Parse_An_AccountsChart() {
      var chart = AccountsChart.Parse(TestingConstants.ACCOUNTS_CHART_UID);

      Assert.Equal(TestingConstants.ACCOUNTS_CHART_UID, chart.UID);

      Assert.NotEmpty(chart.Accounts);
      Assert.NotNull(chart.AccountsPattern);

      chart = AccountsChart.Parse(TestingConstants.ACCOUNTS_CHART_2021_UID);

      Assert.NotEmpty(chart.Accounts);
    }


    [Fact]
    public void Should_Parse_An_Account_By_Id() {
      var account = Account.Parse(TestingConstants.ACCOUNT_ID);

      Assert.Equal(TestingConstants.ACCOUNT_ID, account.Id);
      Assert.Equal(TestingConstants.ACCOUNT_NUMBER, account.Number);
      Assert.Equal(TestingConstants.ACCOUNT_NAME, account.Name);
    }


    [Fact]
    public void Should_Parse_The_Empty_Account() {
      var account = Account.Empty;

      Assert.Equal(-1, account.Id);
      Assert.Equal("Empty", account.UID);
    }

    #endregion Facts

  }  // class AccountsChartTests

}  // namespace Empiria.FinancialAccounting.Tests
