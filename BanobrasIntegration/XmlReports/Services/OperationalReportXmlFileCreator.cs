﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras Integration Services                Component : Xml Reports                           *
*  Assembly : FinancialAccounting.BanobrasIntegration.dll  Pattern   : Service                               *
*  Type     : XmlFileCreator                               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Creates a Xml file with operational reports information.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

using Empiria.FinancialAccounting.BanobrasIntegration.OperationalReports;

namespace Empiria.FinancialAccounting.BanobrasIntegration.XmlReports {

  /// <summary>Creates a Xml file with trial balance information.</summary>
  internal class OperationalReportXmlFileCreator {

    private OperationalReportCommand _command = new OperationalReportCommand();

    private XmlFile _xmlFile;

    public OperationalReportXmlFileCreator() {
      // no-op
    }


    internal XmlFile CreateXmlFile(OperationalReportDto operationalReport) {
      Assertion.AssertObject(operationalReport, "operationalReport");

      _command = operationalReport.Command;

      _xmlFile = new XmlFile();

      SetXmlHeader();

      SetXmlContent(operationalReport);

      _xmlFile.Save(_xmlFile.XmlStructure, _command.ReportType.ToString());

      return _xmlFile;
    }


    #region Private methods

    private string[] GetXmlHeaderName() {

      if (_command.ReportType == OperationalReportType.BalanzaSAT) {
        return new string[] { "BCE", "Balanza"};
      } else if (_command.ReportType == OperationalReportType.CatalogoSAT) {
        return new string[] { "catalogocuentas", "Catalogo" };
      } else {
        throw Assertion.AssertNoReachThisCode();
      }
    }


    private void SetXmlHeader() {
      string[] headerName = GetXmlHeaderName();

      XmlDocument xml = new XmlDocument();
      XmlElement header = xml.CreateElement(headerName[1]);
      xml.AppendChild(header);
      header.Prefix = headerName[0];

      List<XmlFileAttributes> attributes = SetHeaderAttributes();

      XmlAttribute xsi = xml.CreateAttribute("xmlns:xsi");
      xsi.Value = "http://www.w3.org/2001/XMLSchema-instance";
      header.Attributes.Append(xsi);

      XmlAttribute version = xml.CreateAttribute("Version");
      version.Value = attributes.First().Version;
      header.Attributes.Append(version);

      XmlAttribute rfc = xml.CreateAttribute("RFC");
      rfc.Value = attributes.First().RFC;
      header.Attributes.Append(rfc);

      XmlAttribute mes = xml.CreateAttribute("Mes");
      mes.Value = _command.Date.ToString("MM");
      header.Attributes.Append(mes);

      XmlAttribute anio = xml.CreateAttribute("Anio");
      anio.Value = _command.Date.ToString("yyyy");
      header.Attributes.Append(anio);


      foreach (var attr in attributes) {
        XmlAttribute attribute = xml.CreateAttribute(attr.Name);
        attribute.Value = attr.Property;
        header.Attributes.Append(attribute);
      }

      _xmlFile.XmlStructure = xml;
    }


    private List<XmlFileAttributes> SetHeaderAttributes() {
      List<XmlFileAttributes> attributes = new List<XmlFileAttributes>();

      if (_command.ReportType == OperationalReportType.BalanzaSAT) {
        attributes = GetBalanceAttributes();

      } else if(_command.ReportType == OperationalReportType.CatalogoSAT) {
        attributes = GetAccountsChartAttributes();

      }

      return attributes;
    }


    private void SetXmlContent(OperationalReportDto operationalReport) {
      switch (_command.ReportType) {

        case OperationalReportType.BalanzaSAT:
          FillOutBalanza(operationalReport.Entries.Select(x => (OperationalReportEntryDto) x));
          return;

        case OperationalReportType.CatalogoSAT:
          FillOutCatalogoDeCuentas(operationalReport.Entries.Select(x => (OperationalReportEntryDto) x));
          return;

        default:
          throw Assertion.AssertNoReachThisCode();
      }
    }


    private List<XmlFileAttributes> GetAccountsChartAttributes() {
      List<XmlFileAttributes> attributes = new List<XmlFileAttributes>();

      attributes.Add(new XmlFileAttributes() {
        Name = "xmlns:catalogocuentas",
        Property = "http://www.sat.gob.mx/esquemas/ContabilidadE/1_3/CatalogoCuentas"
      });

      attributes.Add(new XmlFileAttributes() {
        Name = "xsi:schemaLocation",
        Property = "http://www.sat.gob.mx/esquemas/ContabilidadE/1_3/CatalogoCuentas " +
                   "http://www.sat.gob.mx/esquemas/ContabilidadE/1_3/CatalogoCuentas/CatalogoCuentas_1_3.xsd"
      });

      return attributes;
    }


    private List<XmlFileAttributes> GetBalanceAttributes() {
      List<XmlFileAttributes> attributes = new List<XmlFileAttributes>();

      attributes.Add(new XmlFileAttributes() {
        Name = "TipoEnvio",
        Property = "N"
      });

      attributes.Add(new XmlFileAttributes() {
        Name = "xmlns:BCE",
        Property = "http://www.sat.gob.mx/esquemas/ContabilidadE/1_3/BalanzaComprobacion"
      });

      attributes.Add(new XmlFileAttributes() {
        Name = "xsi:schemaLocation",
        Property = "http://www.sat.gob.mx/esquemas/ContabilidadE/1_3/BalanzaComprobacion " +
        "http://www.sat.gob.mx/esquemas/ContabilidadE/1_3/BalanzaComprobacion/BalanzaComprobacion_1_3.xsd"
      });

      return attributes;
    }


    private void FillOutCatalogoDeCuentas(IEnumerable<OperationalReportEntryDto> entries) {

      XmlDocument doc = _xmlFile.XmlStructure;

      XmlNode root = doc.SelectSingleNode("Catalogo");

      foreach (var entry in entries) {
        XmlElement ctas = doc.CreateElement("Ctas");
        root.AppendChild(ctas);
        ctas.Prefix = "catalogocuentas";

        XmlAttribute codAgrup = doc.CreateAttribute("CodAgrup");
        codAgrup.Value = entry.GroupingCode;
        ctas.Attributes.Append(codAgrup);

        XmlAttribute numCta = doc.CreateAttribute("NumCta");
        numCta.Value = entry.AccountNumber;
        ctas.Attributes.Append(numCta);

        XmlAttribute desc = doc.CreateAttribute("Desc");
        desc.Value = entry.AccountName.ToString();
        ctas.Attributes.Append(desc);

        XmlAttribute subCtaDe = doc.CreateAttribute("SubCtaDe");
        subCtaDe.Value = entry.AccountNumber.ToString();
        ctas.Attributes.Append(subCtaDe);

        XmlAttribute nivel = doc.CreateAttribute("Nivel");
        nivel.Value = entry.AccountLevel.ToString();
        ctas.Attributes.Append(nivel);

        XmlAttribute natur = doc.CreateAttribute("Natur");
        natur.Value = entry.Naturaleza;
        ctas.Attributes.Append(natur);

      }
      _xmlFile.XmlStructure = doc;
    }

    private void FillOutBalanza(IEnumerable<OperationalReportEntryDto> entries) {

      XmlDocument doc = _xmlFile.XmlStructure;

      XmlNode root = doc.SelectSingleNode("Balanza");

      foreach (var entry in entries) {
        XmlElement ctas = doc.CreateElement("Ctas");
        root.AppendChild(ctas);
        ctas.Prefix = "BCE";

        
        XmlAttribute numCta = doc.CreateAttribute("NumCta");
        numCta.Value = entry.AccountNumber;
        ctas.Attributes.Append(numCta);
        XmlAttribute saldoIni = doc.CreateAttribute("SaldoIni");
        saldoIni.Value = entry.InitialBalance.ToString();
        ctas.Attributes.Append(saldoIni);

        XmlAttribute debe = doc.CreateAttribute("Debe");
        debe.Value = entry.Debit.ToString();
        ctas.Attributes.Append(debe);

        XmlAttribute haber = doc.CreateAttribute("Haber");
        haber.Value = entry.Credit.ToString();
        ctas.Attributes.Append(haber);

        XmlAttribute saldoFin = doc.CreateAttribute("SaldoFin");
        saldoFin.Value = entry.InitialBalance.ToString();
        ctas.Attributes.Append(saldoFin);

      }
      _xmlFile.XmlStructure = doc;
    }


    #endregion


  } // class XmlFileCreator

} // namespace Empiria.FinancialAccounting.BanobrasIntegration.SATReports
