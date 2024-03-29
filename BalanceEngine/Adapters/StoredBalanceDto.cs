﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Balance Engine                               Component : Domain Layer                          *
*  Assembly : FinancialAccounting.BalanceEngine.dll        Pattern   : Information Holder                    *
*  Type     : StoredBalanceDto                             License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Holds a stored account balance.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.FinancialAccounting.BalanceEngine {

  public class StoredBalanceDto {

    internal StoredBalanceDto() {
      // no-op
    }

    public int StandardAccountId {
      get; internal set;
    }


    public NamedEntityDto Ledger {
      get; internal set;
    }

    public NamedEntityDto Currency {
      get; internal set;
    }

    public int SubledgerAccountId {
      get; internal set;
    }

    public string SectorCode {
      get; internal set;
    }

    public string AccountName {
      get; internal set;
    }

    public string AccountNumber {
      get; internal set;
    }

    public string SubledgerAccountNumber {
      get; internal set;
    }

    public string SubledgerAccountName {
      get; internal set;
    }

    public decimal Balance {
      get; internal set;
    }

    //public DateTime LastChangeDate {
    //  get; internal set;
    //}
    


  }  // class StoredBalance

}  // namespace Empiria.FinancialAccounting.BalanceEngine.Adapters
