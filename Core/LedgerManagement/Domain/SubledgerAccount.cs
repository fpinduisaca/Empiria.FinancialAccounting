﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Ledger Management                          Component : Domain Layer                            *
*  Assembly : FinancialAccounting.Core.dll               Pattern   : Empiria Data Object                     *
*  Type     : SubledgerAccount                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds information about a subledger account (cuenta auxiliar).                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.FinancialAccounting.Adapters;
using Empiria.FinancialAccounting.Data;

namespace Empiria.FinancialAccounting {

  /// <summary>Holds information about a subledger account (cuenta auxiliar).</summary>
  public class SubledgerAccount {

    #region Constructors and parsers

    protected SubledgerAccount() {
      // Required by Empiria Framework.
    }


    internal SubledgerAccount(Subledger subledger, string number, string name) {
      Assertion.AssertObject(subledger, "subledger");
      Assertion.AssertObject(number, "number");
      Assertion.AssertObject(name, "name");

      this.Subledger = subledger;
      this.Number = number;
      this.Name = name;
    }


    static public SubledgerAccount Parse(int id) {
      if (id == 0) {
        id = -1;
      }
      return SubledgerData.GetSubledgerAccount(id);
    }


    static public SubledgerAccount TryParse(string subledgerAccountNumber) {
      return SubledgerData.TryGetSubledgerAccount(subledgerAccountNumber);
    }


    static internal FixedList<SubledgerAccount> Search(AccountsChart accountsChart, string filter) {
      Assertion.AssertObject(accountsChart, "accountsChart");
      Assertion.AssertObject(filter, "filter");

      return SubledgerData.GetSubledgerAccountsList(accountsChart, filter);
    }


    static public void Preload() {
      SubledgerData.GetSubledgerAccountsList();
    }


    static public SubledgerAccount Empty => Parse(-1);

    #endregion Constructors and parsers

    #region Public properties

    [DataField("ID_CUENTA_AUXILIAR", ConvertFrom = typeof(long))]
    public int Id {
      get;
      private set;
    }


    public Ledger Ledger {
      get {
        return this.Subledger.BaseLedger;
      }
    }


    public Ledger AdditionalLedger {
      get {
        return this.Subledger.AdditionalLedger;
      }
    }


    [DataField("ID_MAYOR_AUXILIAR", ConvertFrom = typeof(long))]
    public Subledger Subledger {
      get; private set;
    }


    [DataField("NUMERO_CUENTA_AUXILIAR")]
    public string Number {
      get; private set;
    }


    [DataField("NOMBRE_CUENTA_AUXILIAR")]
    public string Name {
      get; private set;
    }


    [DataField("DESCRIPCION")]
    public string Description {
      get; private set;
    }


    [DataField("ELIMINADA", ConvertFrom = typeof(int))]
    public bool Suspended {
      get; private set;
    }


    public bool IsEmptyInstance {
      get {
        return (this.Id == -1 || this.Id == 0);
      }
    }

    #endregion Public properties

    #region Methods

    internal void Activate() {
      this.Suspended = false;
    }


    public void Save() {
      if (this.Id == 0) {
        this.Id = SubledgerData.NextSubledgerAccountId();
      }
      SubledgerData.WriteSubledgerAccount(this);
    }


    internal void Suspend() {
      this.Suspended = true;
    }


    internal void Update(SubledgerAccountFields fields) {
      Assertion.AssertObject(fields, "fields");

      fields.EnsureValid();

      this.Name = FieldPatcher.PatchField(fields.Name, this.Name);
      this.Description = FieldPatcher.PatchField(fields.Description, this.Description);

      UpdateSubledger(fields.SubledgerType());
    }


    private void UpdateSubledger(SubledgerType newSubledgerType) {
      Assertion.AssertObject(newSubledgerType, "newSubledgerType");

      if (newSubledgerType.Equals(this.Subledger.SubledgerType)) {
        return;
      }

      var newSubledger = this.Ledger.Subledgers().Find(x => x.SubledgerType == newSubledgerType);

      Assertion.AssertObject(newSubledger,
            $"No existe un libro auxiliar de tipo {newSubledgerType.Name} para la " +
            $"contabilidad {this.Ledger.FullName}.");

      this.Subledger = newSubledger;
    }


    #endregion Methods

  }  // class SubledgerAccount

}  // namespace Empiria.FinancialAccounting
