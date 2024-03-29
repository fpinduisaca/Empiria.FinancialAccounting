﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Accounts Chart                             Component : Interface adapters                      *
*  Assembly : FinancialAccounting.Core.dll               Pattern   : Command payload                         *
*  Type     : AccountsSearchCommand                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Command payload used for accounts searching.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Empiria.FinancialAccounting.Adapters {

  /// <summary>Command payload used for accounts searching.</summary>
  public class AccountsSearchCommand {

    public string Keywords {
      get; set;
    } = string.Empty;


    public DateTime? Date {
      get; set;
    } = DateTime.Today;


    public string Ledger {
      get; set;
    } = string.Empty;


    public string FromAccount {
      get; set;
    } = string.Empty;


    public string ToAccount {
      get; set;
    } = string.Empty;


    public int Level {
      get; set;
    } = 0;


    public string[] Types {
      get; set;
    } = new string[0];


    public AccountRole[] Roles {
      get; set;
    } = new AccountRole[0];


    public string[] Sectors {
      get; set;
    } = new string[0];


    public string[] Currencies {
      get; set;
    } = new string[0];


    public bool WithSectors {
      get; set;
    }

  }  // class AccountsSearchCommand



  /// <summary>Extension methods for AccountsSearchCommand interface adapter.</summary>
  static public class AccountsSearchCommandExtension {

    #region Public methods

    static public string MapToFilterString(this AccountsSearchCommand command,
                                           AccountsChart accountsChart) {
      string keywordsFilter = BuildKeywordsFilter(command.Keywords, accountsChart);
      string rangeFilter = BuildAccountsRangeFilter(command.FromAccount, command.ToAccount);
      string typeFilter = BuildAccountsTypeFilter(command.Types);
      string roleFilter = BuildAccountsRoleFilter(command.Roles);

      string dateFilter = BuildDateFilter(command.Date);

      var filter = new Filter(keywordsFilter);

      filter.AppendAnd(rangeFilter);
      filter.AppendAnd(typeFilter);
      filter.AppendAnd(roleFilter);
      filter.AppendAnd(dateFilter);

      return filter.ToString();
    }


    static internal FixedList<Account> Restrict(this AccountsSearchCommand command,
                                                FixedList<Account> accounts) {
      FixedList<Account> restricted;

      restricted = RestrictLedger(command.Ledger, accounts);
      restricted = RestrictLevels(command.Level, restricted);
      restricted = RestrictSectors(command.Sectors, restricted);
      restricted = RestrictCurrencies(command.Currencies, restricted);

      return restricted;
    }


    #endregion Public methods

    #region Private methods

    static private string BuildAccountsRangeFilter(string fromAccount, string toAccount) {
      string filter = String.Empty;

      if (fromAccount.Length != 0) {
        filter = $"'{fromAccount}' <= NUMERO_CUENTA_ESTANDAR";
      }
      if (toAccount.Length != 0) {
        if (filter.Length != 0) {
          filter += " AND ";
        }
        filter += $"NUMERO_CUENTA_ESTANDAR <= '{toAccount}'";
      }

      return filter;
    }


    static private string BuildAccountsRoleFilter(AccountRole[] accountsRoles) {
      if (accountsRoles.Length == 0) {
        return string.Empty;
      }

      string[] rolesAsStrings = accountsRoles.Select(x => $"'{(char) x}'")
                                             .ToArray();

      return $"ROL_CUENTA IN ({String.Join(", ", rolesAsStrings)})";
    }


    static private string BuildAccountsTypeFilter(string[] accountsTypesUIDs) {
      if (accountsTypesUIDs.Length == 0) {
        return string.Empty;
      }

      var list = new List<AccountType>();

      foreach (var uid in accountsTypesUIDs) {
        var accountType = AccountType.Parse(uid);
        list.Add(accountType);
      }
      string[] idsArray = list.Select(x => x.Id.ToString()).ToArray();

      return $"ID_TIPO_CUENTA IN ({String.Join(", ", idsArray)})";
    }


    static private string BuildDateFilter(DateTime? date) {
      if (!date.HasValue) {
        date = DateTime.Today;
      }

      string formattedDate = CommonMethods.FormatSqlDate(date.Value);

      return $"FECHA_INICIO <= '{formattedDate}' AND '{formattedDate}' <= FECHA_FIN";
    }



    static private string BuildKeywordsFilter(string keywords, AccountsChart accountsChart) {
      return accountsChart.BuildSearchAccountsFilter(keywords);
    }


    static private FixedList<Account> RestrictCurrencies(string[] currencies, FixedList<Account> accounts) {
      if (currencies.Length == 0) {
        return accounts;
      }

      var restricted = accounts.FindAll(acct => acct.CurrencyRules.Contains(acctCurcy => acctCurcy.Currency.UID == currencies[0]));

      for (int i = 1; i < currencies.Length; i++) {
        var temp = restricted.FindAll(acct => acct.CurrencyRules.Contains(acctCurcy => acctCurcy.Currency.UID == currencies[i]));
        restricted = restricted.Intersect(temp);
      }

      return restricted;
    }


    static private FixedList<Account> RestrictLedger(string ledger, FixedList<Account> accounts) {
      if (ledger.Length == 0) {
        return accounts;
      }
      return accounts.FindAll(x => x.LedgerRules.Contains(y => y.Ledger.UID == ledger));
    }


    static private FixedList<Account> RestrictLevels(int level, FixedList<Account> accounts) {
      if (level > 0) {
        return accounts.FindAll(x => x.Level <= level);
      } else {
        return accounts;
      }
    }


    static private FixedList<Account> RestrictSectors(string[] sectors, FixedList<Account> accounts) {
      if (sectors.Length == 0) {
        return accounts;
      }
      var restricted = accounts.FindAll(acct => acct.SectorRules.Contains(acctSector => acctSector.Sector.UID == sectors[0]));
      for (int i = 1; i < sectors.Length; i++) {
        var temp = restricted.FindAll(acct => acct.SectorRules.Contains(acctSector => acctSector.Sector.UID == sectors[i]));
        restricted = restricted.Intersect(temp);
      }
      return restricted;
    }

    #endregion Private methods

  }  // AccountsSearchCommandExtension

}  // namespace Empiria.FinancialAccounting.Adapters
