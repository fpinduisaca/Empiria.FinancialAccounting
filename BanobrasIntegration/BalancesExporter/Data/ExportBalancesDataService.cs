﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras Integration Services                Component : Balances Exporter                     *
*  Assembly : FinancialAccounting.BanobrasIntegration.dll  Pattern   : Data Service                          *
*  Type     : ExportBalancesDataService                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Data methods to store exported balances.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Collections;
using Empiria.Data;

using Empiria.FinancialAccounting.BanobrasIntegration.BalancesExporter.Adapters;

namespace Empiria.FinancialAccounting.BanobrasIntegration.BalancesExporter.Data {

  /// <summary>Data methods to store exported balances.</summary>
  static internal class ExportBalancesDataService {

    #region Public methods

    static internal EmpiriaHashTable<CalificacionMoneda> GetCalificacionMonedaHashTable() {
      var sql = "SELECT * FROM CALIFICA_MONEDA " +
                "WHERE CALIFICA_SALDO IS NOT NULL";

      var dataOperation = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectHashTable<CalificacionMoneda>(dataOperation,
                                                                    x => $"{x.Cuenta}||{x.Sector}||{x.Auxiliar}");
    }


    static internal void WriteBalancesByDay(DateTime fecha, FixedList<ExportedBalancesDto> balances) {
      throw new NotImplementedException();
    }


    static internal void WriteBalancesByMonth(ExportBalancesCommand command, FixedList<ExportedBalancesDto> balances) {
      DeleteStoredBalancesByMonth(command);

      foreach (var balance in balances) {
        WriteBalancesByMonth(balance);
      }
    }


    #endregion Public methods

    #region Private methods

    static private void DeleteStoredBalancesByMonth(ExportBalancesCommand command) {
      var dataOperation = DataOperation.Parse("del_scon_saldos_ant",
                                              command.Empresa,
                                              command.Fecha.Year,
                                              command.Fecha.Month);

      DataWriter.Execute(dataOperation);
    }


    static private void WriteBalancesByMonth(ExportedBalancesDto o) {
      string nullString = null;

      var dataOperation = DataOperation.Parse("apd_scon_saldos_ant",
                              o.Anio, o.Mes, o.Area, o.Moneda, o.NumeroMayor,
                              o.Cuenta, o.Sector, o.Auxiliar, o.FechaUltimoMovimiento,
                              o.Saldo, o.MonedaOrigen, o.NaturalezaCuenta, o.SaldoPromedio,
                              o.MontoDebito, o.MontoCredito, o.SaldoAnterior, o.Empresa,
                              o.CalificaMoneda == "null" ? nullString : o.CalificaMoneda,
                              DBNull.Value);

      DataWriter.Execute(dataOperation);
    }

    #endregion Private methods

  }  // class ExportBalancesDataService

}  // namespace Empiria.FinancialAccounting.BanobrasIntegration.BalancesExporter.Data
