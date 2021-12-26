﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras Integration Services                 Component : Vouchers Importer                    *
*  Assembly : FinancialAccounting.BanobrasIntegration.dll   Pattern   : Singleton Controller                 *
*  Type     : DbVouchersImporter                            License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Performs voucher importation tasks from a central database.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Empiria.FinancialAccounting.BanobrasIntegration.VouchersImporter.Adapters;

namespace Empiria.FinancialAccounting.BanobrasIntegration.VouchersImporter {

  /// <summary>Performs voucher importation tasks from a central database.</summary>
  internal class DbVouchersImporter {

    static private readonly DbVouchersImporter _singleton = new DbVouchersImporter();

    private ImportVouchersResult _importVouchersResult;

    private DbVouchersImporter() {
      // Singleton private constructor
    }

    static internal DbVouchersImporter Instance {
      get {
        return _singleton;
      }
    }


    #region Public members


    internal bool IsRunning {
      get;
      private set;
    }


    internal ImportVouchersResult GetImportVouchersResult() {
      if (!this.IsRunning) {
        UpdateImportVouchersResult();
      }
      return _importVouchersResult;
    }


    internal async Task Start(ImportVouchersCommand command) {
      if (this.IsRunning) {
        Assertion.AssertFail("DBVouchersImporter is running. Please stop it before call Start() method");
      }

      command.TryToCloseVouchers = true;     // ToDo: OOJJOO

      command.CanEditVoucherEntries = false;

      SetIsRunningFlag(true);

      await Task.Run(() => {
        try {
          ImportVouchers(command);

          SetIsRunningFlag(false);

        } catch (Exception e) {
          EmpiriaLog.Error(e);
          SetIsRunningFlag(false);
          throw;
        }
      });
    }


    private void ImportVouchers(ImportVouchersCommand command) {
      const int BATCH_SIZE = 50;

      Assertion.AssertObject(command.ProcessOnly[0], "ImportationSet");

      List<Encabezado> encabezados = DbVouchersImporterDataService.GetEncabezados(command.ProcessOnly[0]);
      List<Movimiento> movimientos = DbVouchersImporterDataService.GetMovimientos(command.ProcessOnly[0]);

      while (true) {
        if (encabezados.Count == 0 || !this.IsRunning) {
          EmpiriaLog.Info($"Voucher processing ends at {DateTime.Now}.");
          return;
        }

        EmpiriaLog.Info($"To be processed {encabezados.Count} at {DateTime.Now}.");

        var toProcess = encabezados.GetRange(0, encabezados.Count >= BATCH_SIZE ? BATCH_SIZE : encabezados.Count)
                                   .ToFixedList();

        var structurer = new DbVouchersStructurer(toProcess, movimientos.ToFixedList());

        FixedList<ToImportVoucher> toImport = structurer.GetToImportVouchersList();

        var voucherImporter = new VoucherImporter(command, toImport);

        voucherImporter.Import();

        foreach (var item in toProcess) {
          encabezados.Remove(item);
        }

        UpdateImportVouchersResult();
      }
    }


    internal void Stop() {
      SetIsRunningFlag(false);
    }

    #endregion Public members


    private void SetIsRunningFlag(bool isRunning) {
      this.IsRunning = isRunning;

      UpdateImportVouchersResult();
    }


    private void UpdateImportVouchersResult() {
      if (!this.IsRunning) {
        _importVouchersResult = DbVouchersImporterDataService.GetEncabezadosTotals();
      }

      _importVouchersResult.IsRunning = this.IsRunning;
    }


  }  // class DbVouchersImporter

}  // namespace Empiria.FinancialAccounting.BanobrasIntegration.VouchersImporter
