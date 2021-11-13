﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Ledger Management                          Component : Interface adapters                      *
*  Assembly : FinancialAccounting.Core.dll               Pattern   : Mapper class                            *
*  Type     : SubledgerMapper                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for subledger books and subledger accounts.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.FinancialAccounting.Adapters {

  /// <summary>Mapping methods for subledger books and subledger accounts.</summary>
  static public class SubledgerMapper {

    static internal FixedList<SubledgerDto> Map(FixedList<Subledger> subledgers) {
      return new FixedList<SubledgerDto>(subledgers.Select(x => Map(x)));
    }


    static internal SubledgerDto Map(Subledger subledger) {
      return new SubledgerDto {
        UID = subledger.UID,
        TypeName = subledger.SubledgerType.Name,
        Name = subledger.Name,
        Description = subledger.Description,
        AccountsPrefix = subledger.AccountsPrefix,
        BaseLedger = subledger.BaseLedger.MapToNamedEntity()
      };
    }


    static public FixedList<SubledgerAccountDto> Map(FixedList<SubledgerAccount> list) {
      return new FixedList<SubledgerAccountDto>(list.Select(x => MapAccount(x)));
    }


    static public SubledgerAccountDescriptorDto MapAccountToDescriptor(SubledgerAccount subledgerAccount) {
      return new SubledgerAccountDescriptorDto {
        Id = subledgerAccount.Id,
        Number = subledgerAccount.Number,
        Name = subledgerAccount.Name,
        FullName = $"{subledgerAccount.Number} - {subledgerAccount.Name}"
      };
    }


    static internal SubledgerAccountDto MapAccount(SubledgerAccount subledgerAccount) {
      return new SubledgerAccountDto {
        Id = subledgerAccount.Id,
        BaseLedger = subledgerAccount.Ledger.MapToNamedEntity(),
        Subledger = subledgerAccount.Subledger.MapToNamedEntity(),
        Name = subledgerAccount.Name,
        Number = subledgerAccount.Number,
        Description = subledgerAccount.Description
      };
    }

  }  // class SubledgerMapper

}  // namespace Empiria.FinancialAccounting.Adapters