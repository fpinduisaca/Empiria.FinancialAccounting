﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Accounts Chart                             Component : Domain Layer                            *
*  Assembly : FinancialAccounting.Core.dll               Pattern   : Empiria Data Object                     *
*  Type     : StandardAccount                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Contains information about an standard account.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.FinancialAccounting {

  /// <summary>Contains information about an standard account.</summary>
  public class StandardAccount : BaseObject {


    #region Constructors and parsers

    private StandardAccount() {
      // Required by Empiria Framework.
    }


    static public StandardAccount Parse(int id) {
      return BaseObject.ParseId<StandardAccount>(id);
    }


    static public StandardAccount Parse(string uid) {
      return BaseObject.ParseKey<StandardAccount>(uid);
    }

    static public void Preload() {
      BaseObject.GetList<StandardAccount>();
    }


    static public StandardAccount Empty => BaseObject.ParseEmpty<StandardAccount>();


    #endregion Constructors and parsers

    #region Public properties


    [DataField("ID_TIPO_CUENTAS_STD", ConvertFrom = typeof(long))]
    public AccountsChart AccountsChart {
      get; private set;
    }


    [DataField("NUMERO_CUENTA_ESTANDAR")]
    public string Number {
      get; private set;
    } = string.Empty;


    [DataField("NOMBRE_CUENTA_ESTANDAR")]
    public string Name {
      get; private set;
    } = string.Empty;


    [DataField("DESCRIPCION")]
    public string Description {
      get; private set;
    } = string.Empty;


    [DataField("ROL_CUENTA", Default = AccountRole.Sumaria)]
    public AccountRole Role {
      get; private set;
    } = AccountRole.Sumaria;


    [DataField("ID_TIPO_CUENTA")]
    private AccountType _accountType = FinancialAccounting.AccountType.Empty;

    public string AccountType {
      get {
        return _accountType.Name;
      }
    }


    [DataField("NATURALEZA", Default = DebtorCreditorType.Deudora)]
    public DebtorCreditorType DebtorCreditor {
      get; private set;
    } = DebtorCreditorType.Deudora;


    public bool HasParent {
      get {
        return (this.Level > 1);
      }
    }


    public bool NotHasParent {
      get {
        return !this.HasParent;
      }
    }


    public int Level {
      get {
        var accountNumberSeparator = this.AccountsChart.MasterData.AccountNumberSeparator;

        return EmpiriaString.CountOccurences(Number, accountNumberSeparator) + 1;
      }
    }


    #endregion Public properties


    #region Public methods

    internal FixedList<Account> GetHistory() {
      return this.AccountsChart.GetAccountHistory(this.Number);
    }


    public StandardAccount GetParent() {
      if (this.Level == 1) {
        return StandardAccount.Empty;
      }

      var accountNumberSeparator = this.AccountsChart.MasterData.AccountNumberSeparator;

      var parentAccountNumber = this.Number.Substring(0, this.Number.LastIndexOf(accountNumberSeparator));

      var parent = AccountsChart.GetAccount(parentAccountNumber);

      return StandardAccount.Parse(parent.StandardAccountId);

    }


    #endregion Public methods

  }  // class StandardAccount

}  // namespace Empiria.FinancialAccounting
