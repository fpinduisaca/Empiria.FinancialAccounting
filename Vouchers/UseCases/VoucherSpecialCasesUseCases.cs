﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Vouchers Management                        Component : Use cases Layer                         *
*  Assembly : FinancialAccounting.Vouchers.dll           Pattern   : Use case interactor class               *
*  Type     : VoucherSpecialCasesUseCases                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases used to generate special case vouchers.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Services;

using Empiria.FinancialAccounting.Vouchers.Adapters;

namespace Empiria.FinancialAccounting.Vouchers.UseCases {

  /// <summary>Use cases used to generate special case vouchers.</summary>
  public class VoucherSpecialCasesUseCases : UseCase {

    #region Constructors and parsers

    protected VoucherSpecialCasesUseCases() {
      // no-op
    }

    static public VoucherSpecialCasesUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<VoucherSpecialCasesUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases


    public VoucherDto CreateSpecialCaseVoucher(VoucherSpecialCaseFields fields) {
      Assertion.AssertObject(fields, "fields");

      var builder = VoucherBuilder.SelectBuilder(fields);

      Voucher voucher = builder.BuildVoucher();

      return VoucherMapper.Map(voucher);
    }


    public FixedList<VoucherSpecialCaseTypeDto> GetSpecialCaseTypes() {
      FixedList<VoucherSpecialCaseType> list = VoucherSpecialCaseType.GetList();

      return VoucherSpecialCaseTypeMapper.Map(list);
    }

    #endregion Use cases

  }  // class VoucherSpecialCasesUseCases

}  // namespace Empiria.FinancialAccounting.Vouchers.UseCases
