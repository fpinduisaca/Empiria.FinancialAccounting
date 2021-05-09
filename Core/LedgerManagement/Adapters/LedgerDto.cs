﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Ledger Management                          Component : Interface adapters                      *
*  Assembly : FinancialAccounting.Core.dll               Pattern   : Data Transfer Object                    *
*  Type     : LedgerDto                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO with data related to an accounting ledger book.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.FinancialAccounting.Adapters {

  /// <summary>Output DTO with data related to an accounting ledger book.</summary>
  public class LedgerDto {

    internal LedgerDto() {
      // no-op
    }

    public string UID {
      get; internal set;
    }


    public string Name {
      get; internal set;
    }


    public string Number {
      get; internal set;
    }


    public string Subnumber {
      get; internal set;
    }


    public string SubsidiaryAccountsPrefix {
      get;
      internal set;
    }


    public NamedEntityDto AccountsChart {
      get; internal set;
    }


    public NamedEntityDto BaseCurrency {
      get; internal set;
    }

  }  // public class LedgerDto

}  // namespace Empiria.FinancialAccounting.Adapters
