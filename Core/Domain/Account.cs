﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Accounts Chart                             Component : Domain Layer                            *
*  Assembly : FinancialAccounting.Core.dll               Pattern   : Empiria Data Object                     *
*  Type     : Account                                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Contains information about an account.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.FinancialAccounting {

  /// <summary>Contains information about an account.</summary>
  public class Account : BaseObject {

    #region Constructors and parsers

    private Account() {
      // Required by Empiria Framework.
    }


    static public Account Parse(int id) {
      return BaseObject.ParseId<Account>(id);
    }

    static public Account Empty => BaseObject.ParseEmpty<Account>();


    #endregion Constructors and parsers

    #region Public properties

    [DataField("ID_TIPO_CUENTAS_STD", ConvertFrom=typeof(long))]
    internal int AccountsChartId {
      get; private set;
    } = -1;


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


    [DataField("ROL_CUENTA", Default = AccountRole.Summary)]
    public AccountRole Role {
      get; private set;
    } = AccountRole.Summary;


    [DataField("ID_TIPO_CUENTA")]
    private AccountType _accountType = FinancialAccounting.AccountType.Empty;

    public string AccountType {
      get {
        return _accountType.Name;
      }
    }


    [DataField("NATURALEZA", Default = DebtorCreditorType.Debtor)]
    public DebtorCreditorType DebtorCreditor {
      get; private set;
    } = DebtorCreditorType.Debtor;


    #endregion Public properties

  }  // class Account

}  // namespace Empiria.FinancialAccounting
