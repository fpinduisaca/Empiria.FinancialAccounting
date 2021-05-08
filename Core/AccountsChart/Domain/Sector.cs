﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Accounts Chart                             Component : Domain Layer                            *
*  Assembly : FinancialAccounting.Core.dll               Pattern   : Empiria Data Object                     *
*  Type     : Sector                                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds data about an account's sector.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.FinancialAccounting {

  /// <summary>Holds data about an account's sector.</summary>
  public class Sector : BaseObject {

    #region Constructors and parsers

    private Sector() {
      // Required by Empiria Framework.
    }


    static public Sector Parse(int id) {
      return BaseObject.ParseId<Sector>(id);
    }


    static public Sector Parse(string uid) {
      return BaseObject.ParseKey<Sector>(uid);
    }


    static public FixedList<Sector> GetList() {
      return BaseObject.GetList<Sector>(string.Empty, "CLAVE_SECTOR")
                       .ToFixedList();
    }


    #endregion Constructors and parsers

    #region Public properties


    [DataField("NOMBRE_SECTOR")]
    public string Name {
      get; private set;
    }


    [DataField("CLAVE_SECTOR")]
    public string Code {
      get; private set;
    }


    #endregion Public properties

  }

}  // namespace Empiria.FinancialAccounting