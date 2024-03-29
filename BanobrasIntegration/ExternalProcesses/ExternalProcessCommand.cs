﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras Integration Services                Component : External Processes Invoker            *
*  Assembly : FinancialAccounting.BanobrasIntegration.dll  Pattern   : Command payload                       *
*  Type     : ExternalProcessCommand                       License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Command data to execute Banobras' external system processes (Marimba).                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.FinancialAccounting.BanobrasIntegration {

  /// <summary>Datos para la ejecución del proceso de rentabilidad.</summary>
  public class RentabilidadExternalProcessCommand {

    public int Anio {
      get; set;
    }

    public int Mes {
      get; set;
    }

    public int Metodologia {
      get; set;
    }

  }  // class RentabilidadExternalProcessCommand

}  // namespace Empiria.FinancialAccounting.BanobrasIntegration
