﻿

using System;

namespace Empiria.FinancialAccounting.BalanceEngine {

  internal class TrialBalanceTypes {

    #region Public methods
    
    static public string[] MapToFieldString(int trialBalanceType) {
      string[] fieldsAndGrouping = new string[2];

      trialBalanceType = trialBalanceType != -1 ? trialBalanceType : 1;

      fieldsAndGrouping = BuildFieldTypes(trialBalanceType);

      return fieldsAndGrouping;
    }

    #endregion

    #region Private methods
    
    static private string[] BuildFieldTypes(int typeId) {
      string[] fieldsGrouping = new string[2];
      string fields = String.Empty;
      string grouping = String.Empty;

      if (typeId == 1) {
        fields = "-1 AS ID_MAYOR, -1 AS ID_MONEDA, -1 AS ID_CUENTA, ID_CUENTA_ESTANDAR, " +
                 "'-1' AS NUMERO_CUENTA_ESTANDAR, ID_SECTOR, '-1' AS NOMBRE_CUENTA_ESTANDAR, " +
                 "SALDO_ANTERIOR, CARGOS, ABONOS, SALDO_ACTUAL ";

        grouping = "ID_CUENTA_ESTANDAR, ID_SECTOR, " +
                   "SALDO_ANTERIOR, CARGOS, ABONOS, SALDO_ACTUAL ";
      }

      if (typeId == 2) {
        fields = "ID_MAYOR, ID_MONEDA, ID_CUENTA, ID_CUENTA_ESTANDAR, " +
                 "NUMERO_CUENTA_ESTANDAR, ID_SECTOR, NOMBRE_CUENTA_ESTANDAR, " +
                 "SALDO_ANTERIOR, CARGOS, ABONOS, SALDO_ACTUAL ";

        grouping = "ID_MAYOR, ID_MONEDA, ID_CUENTA, ID_CUENTA_ESTANDAR, " +
                   "NUMERO_CUENTA_ESTANDAR, ID_SECTOR, NOMBRE_CUENTA_ESTANDAR, " +
                   "SALDO_ANTERIOR, CARGOS, ABONOS, SALDO_ACTUAL ";
      }

      fieldsGrouping[0] = fields;
      fieldsGrouping[1] = grouping;

      return fieldsGrouping;
    }




    #endregion


  }
}
