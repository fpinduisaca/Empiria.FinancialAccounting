﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Reporting Services                         Component : Interface adapters                      *
*  Assembly : FinancialAccounting.Reporting.dll          Pattern   : Type Extension methods                  *
*  Type     : PolizasCommandExtensions                    License   : Please read LICENSE.txt file           *
*                                                                                                            *
*  Summary  : Type extension methods for PolizasCommand.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Linq;

namespace Empiria.FinancialAccounting.Reporting.Adapters {

  /// <summary>Type extension methods for PolizasCommand.</summary>
  internal class PolizasCommandExtensions {

    #region Public methods

    internal PolizaCommandData MapToPolizaCommandData(PolizasCommand command) {

      var clauses = new PolizaClausesHelper(command);

      return clauses.GetPolizaCommandData();
    }


    #endregion


    private class PolizaClausesHelper {

      private readonly PolizasCommand _command;

      internal PolizaClausesHelper(PolizasCommand command) {
        this._command = command;
      }


      #region Public methods

      internal PolizaCommandData GetPolizaCommandData() {
        var commandData = new PolizaCommandData();

        var accountsChart = AccountsChart.Parse(_command.AccountsChartUID);

        commandData.AccountsChart = accountsChart;
        commandData.FromDate = _command.FromDate;
        commandData.ToDate = _command.ToDate;
        commandData.Ledgers = GetLedgerFilter();

        return commandData;
      }

      private string GetLedgerFilter() {
        if (_command.Ledgers.Length == 0) {
          return string.Empty;
        }

        int[] ledgerIds = _command.Ledgers.Select(uid => Ledger.Parse(uid).Id)
                                          .ToArray();

        return $" AND T.ID_MAYOR IN ({String.Join(", ", ledgerIds)})";
      }

      #endregion

    }


  } // class PolizasCommandExtensions

} // namespace Empiria.FinancialAccounting.Reporting.Adapters
