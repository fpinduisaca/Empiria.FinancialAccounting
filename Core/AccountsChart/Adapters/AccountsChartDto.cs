﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Accounts Chart                             Component : Interface adapters                      *
*  Assembly : FinancialAccounting.Core.dll               Pattern   : Data Transfer Object                    *
*  Type     : AccountsChartDto                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO with data related to an account chart and its contents.                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.FinancialAccounting.Adapters {

  /// <summary>Output DTO with data related to an account chart and its contents.</summary>
  public class AccountsChartDto {

    internal AccountsChartDto() {
      // no-op
    }

    public string UID {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public bool WithSectors {
      get; internal set;
    }

    public FixedList<AccountDescriptorDto> Accounts {
      get; internal set;
    }

  }  // public class AccountsChartDto


  /// <summary>Output DTO with an account data with less information to be used in lists.</summary>
  public class AccountDescriptorDto {

    internal AccountDescriptorDto() {
      // no-op
    }

    public string UID {
      get; internal set;
    }

    public string Number {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public string Type {
      get; internal set;
    }

    public AccountRole Role {
      get; internal set;
    }

    public bool UsesSector {
      get; internal set;
    }

    public bool UsesSubledger {
      get; internal set;
    }

    public DebtorCreditorType DebtorCreditor {
      get; internal set;
    }

    public int Level {
      get; internal set;
    }

    public bool Obsolete {
      get; internal set;
    }

    public string Sector {
      get; internal set;
    }

    public DateTime StartDate {
      get; internal set;
    }

    public DateTime EndDate {
      get; internal set;
    }

    public bool LastLevel {
      get; internal set;
    }

    public string Parent {
      get; internal set;
    }

    public bool SummaryWithNotChildren {
      get; internal set;
    }

  }  // class AccountDescriptorDto


  /// <summary>Output DTO with an account full data.</summary>
  public class AccountDto : AccountDescriptorDto {

    internal AccountDto() {
      // no-op
    }

    public string Description {
      get; internal set;
    }

    public NamedEntityDto AccountsChart {
      get; internal set;
    }

    public FixedList<AreaRule> AreaRules {
      get; internal set;
    }

    public FixedList<CurrencyRule> CurrencyRules {
      get; internal set;
    }

    public FixedList<SectorRuleDto> SectorRules {
      get; internal set;
    }

    public FixedList<LedgerRuleDto> LedgerRules {
      get; internal set;
    }

    public FixedList<AccountHistoryDto> History {
      get; internal set;
    }

  }  // class AccountDto



  /// <summary>Output DTO with the changes history of an account.</summary>
  public class AccountHistoryDto {

    internal AccountHistoryDto() {
      // no-op
    }

    public string UID {
      get; internal set;
    }

    public string Number {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public string Type {
      get; internal set;
    }

    public AccountRole Role {
      get; internal set;
    }

    public DebtorCreditorType DebtorCreditor {
      get; internal set;
    }

    public DateTime StartDate {
      get; internal set;
    }

    public DateTime EndDate {
      get; internal set;
    }

  }  // class AccountHistoryDto

}  // namespace Empiria.FinancialAccounting.Adapters
