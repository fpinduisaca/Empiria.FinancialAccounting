﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Accounts Chart                             Component : Domain Layer                            *
*  Assembly : FinancialAccounting.Core.dll               Pattern   : Empiria Data Object                     *
*  Type     : Currency                                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds data about a currency.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.FinancialAccounting {

  /// <summary>Holds data about a currency.</summary>
  public class Currency : BaseObject {

    #region Constructors and parsers

    private Currency() {
      // Required by Empiria Framework.
    }


    static public Currency Parse(int id) {
      return BaseObject.ParseId<Currency>(id);
    }


    static public Currency Parse(string uid) {
      return BaseObject.ParseKey<Currency>(uid);
    }


    static public FixedList<Currency> GetList() {
      return BaseObject.GetList<Currency>(string.Empty, "O_ID_MONEDA")
                       .ToFixedList();
    }


    #endregion Constructors and parsers

    #region Public properties


    [DataField("CURRENCY_NAME")]
    public string Name {
      get; private set;
    }


    [DataField("ABBREV")]
    public string Abbrev {
      get; private set;
    }


    [DataField("O_ID_MONEDA")]
    public string Code {
      get; private set;
    }


    [DataField("SYMBOL")]
    public string Symbol {
      get; private set;
    }

    #endregion Public properties

  }  // class Currency

}  // namespace Empiria.FinancialAccounting