﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Vouchers Management                        Component : Interface adapters                      *
*  Assembly : FinancialAccounting.Vouchers.dll           Pattern   : Type Extension methods                  *
*  Type     : VoucherEntryFieldsExtensions               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Extension methods for VoucherFields interface adapter.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.FinancialAccounting.Vouchers.Adapters {

  /// <summary>Extension methods for SearchVoucherCommand interface adapter.</summary>
  static internal class VoucherEntryFieldsExtensions {

    #region Extension methods

    static internal void EnsureValidFor(this VoucherEntryFields fields,
                                        Ledger ledger,
                                        DateTime accountingDate) {
      EnsureValidData(fields);

      var validator = new VoucherEntryValidator(ledger, accountingDate);

      validator.EnsureValid(fields);
    }


    static internal void EnsureValidFor(this VoucherEntryFields fields, Voucher voucher) {
      fields.EnsureValidData();
      fields.EnsureVoucherIsAssigned(voucher);

      Ledger ledger = voucher.Ledger;
      DateTime accountingDate = voucher.AccountingDate;

      EnsureValidFor(fields, ledger, accountingDate);
    }


    static private void EnsureValidData(this VoucherEntryFields fields) {
      Assertion.Assert(fields.LedgerAccountId > 0, "fields.LedgerAccountId");

      Assertion.AssertObject(fields.CurrencyUID, "fields.CurrencyUID");

      Assertion.Assert(fields.VoucherEntryType == VoucherEntryType.Credit ||
                       fields.VoucherEntryType == VoucherEntryType.Debit,
                       "fields.VoucherEntryType");

      Assertion.Assert(fields.Amount > 0, "fields.Amount");

      if (fields.UsesBaseCurrency()) {
        Assertion.Assert(
            fields.BaseCurrencyAmount == 0 || fields.BaseCurrencyAmount == fields.Amount,
            "Invalid value for fields.BaseCurrencyAmount. Must be zero or equals to fields.Amount.");

      } else {
        Assertion.Assert(fields.BaseCurrencyAmount > 0,
                         "fields.BaseCurrencyAmount must be greater than zero");
      }

      Assertion.Assert(fields.Amount > 0, "fields.Amount");
    }


    static internal EventType GetEventType(this VoucherEntryFields fields) {
      return EventType.Parse(fields.EventTypeId);
    }


    static internal LedgerAccount GetLedgerAccount(this VoucherEntryFields fields) {
      return LedgerAccount.Parse(fields.LedgerAccountId);
    }


    static internal FunctionalArea GetResponsibilityArea(this VoucherEntryFields fields) {
      return FunctionalArea.Parse(fields.ResponsibilityAreaId);
    }


    static internal SubledgerAccount GetSubledgerAccount(this VoucherEntryFields fields) {
      return SubledgerAccount.Parse(fields.SubledgerAccountId);
    }


    static internal Voucher GetVoucher(this VoucherEntryFields fields) {
      return Voucher.Parse(fields.VoucherId);
    }


    static internal bool UsesBaseCurrency(this VoucherEntryFields fields) {
      return fields.Currency.Equals(fields.GetLedgerAccount().Ledger.BaseCurrency);
    }

    #endregion Extension methods

    static private void EnsureVoucherIsAssigned(this VoucherEntryFields fields,
                                                Voucher voucher) {
      Assertion.Assert(fields.VoucherId > 0, "fields.VoucherId");

      Assertion.Assert(fields.GetVoucher().Id == voucher.Id,
                       "fields.VoucherId does not match the given voucher.");

   }

  }  // class VoucherEntryFieldsExtensions

} // namespace Empiria.FinancialAccounting.Vouchers.Adapters
