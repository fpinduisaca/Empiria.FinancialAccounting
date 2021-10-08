﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras Integration Services                 Component : Vouchers Importer                    *
*  Assembly : FinancialAccounting.BanobrasIntegration.dll   Pattern   : Structurer                           *
*  Type     : DbVouchersStructurer                          License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Holds a voucher's structure coming from database tables.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

namespace Empiria.FinancialAccounting.BanobrasIntegration.VouchersImporter {

  /// <summary>Holds a voucher's structure coming from database tables.</summary>
  internal class DbVouchersStructurer {

    private readonly FixedList<Encabezado> _encabezados;
    private readonly FixedList<Movimiento> _movimientos;

    internal DbVouchersStructurer(FixedList<Encabezado> encabezados, FixedList<Movimiento> movimientos) {
      Assertion.AssertObject(encabezados, "encabezados");
      Assertion.AssertObject(movimientos, "movimientos");

      _encabezados = encabezados;
      _movimientos = movimientos;
    }


    internal ToImportVouchersList GetToImportVouchersList() {
      var vouchersListToImport = new ToImportVouchersList();

      foreach (Encabezado encabezado in _encabezados) {
        var voucherToImport = BuildVoucherToImport(encabezado);

        vouchersListToImport.AddVoucher(voucherToImport);
      }

      return vouchersListToImport;
    }


    private ToImportVoucher BuildVoucherToImport(Encabezado encabezado) {
      ToImportVoucherHeader header = MapEncabezadoToImportVoucherHeader(encabezado);
      FixedList<ToImportVoucherEntry> entries = MapMovimientosToToImportVoucherEntries(header);

      return new ToImportVoucher(header, entries);
    }


    private FixedList<ToImportVoucherEntry> MapMovimientosToToImportVoucherEntries(ToImportVoucherHeader header) {
      var entries = _movimientos.FindAll(x => x.GetVoucherUniqueID() == header.UniqueID);

      var mapped = entries.Select(x => MapMovimientoToStandardVoucherEntry(header, x));

      return new List<ToImportVoucherEntry>(mapped).ToFixedList();
    }


    private ToImportVoucherHeader MapEncabezadoToImportVoucherHeader(Encabezado encabezado) {
      var header = new ToImportVoucherHeader();

      header.ImportationSet = encabezado.GetImportationSet();
      header.UniqueID = encabezado.GetUniqueID();
      header.Ledger = encabezado.GetLedger();
      header.Concept = encabezado.GetConcept();
      header.AccountingDate = encabezado.GetAccountingDate();
      header.VoucherType = encabezado.GetVoucherType();
      header.TransactionType = encabezado.GetTransactionType();
      header.FunctionalArea = encabezado.GetFunctionalArea();
      header.RecordingDate = encabezado.GetRecordingDate();
      header.ElaboratedBy = encabezado.GetElaboratedBy();

      header.Issues = encabezado.GetIssues();

      return header;
    }


    private ToImportVoucherEntry MapMovimientoToStandardVoucherEntry(ToImportVoucherHeader header,
                                                                     Movimiento movimiento) {
      var entry = new ToImportVoucherEntry(header);

      entry.LedgerAccount = movimiento.GetLedgerAccount();
      entry.Sector = movimiento.GetSector();
      entry.SubledgerAccount = movimiento.GetSubledgerAccount();
      entry.ResponsibilityArea = movimiento.GetResponsibilityArea();
      entry.BudgetConcept = movimiento.GetBudgetConcept();
      entry.EventType = movimiento.GetEventType();
      entry.VerificationNumber = movimiento.GetVerificationNumber();
      entry.VoucherEntryType = movimiento.GetVoucherEntryType();
      entry.Date = movimiento.GetDate();
      entry.Concept = movimiento.GetConcept();
      entry.Currency = movimiento.GetCurrency();
      entry.Amount = movimiento.GetAmount();
      entry.ExchangeRate = movimiento.GetExchangeRate();
      entry.BaseCurrencyAmount = movimiento.GetBaseCurrencyAmount();
      entry.Protected = movimiento.GetProtected();

      entry.Issues = movimiento.GetIssues();

      return entry;
    }

  }  // class DbVouchersStructurer

}  // namespace Empiria.FinancialAccounting.BanobrasIntegration.VouchersImporter