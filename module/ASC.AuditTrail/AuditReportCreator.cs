/*
 *
 * (c) Copyright Ascensio System Limited 2010-2016
 *
 * This program is freeware. You can redistribute it and/or modify it under the terms of the GNU 
 * General Public License (GPL) version 3 as published by the Free Software Foundation (https://www.gnu.org/copyleft/gpl.html). 
 * In accordance with Section 7(a) of the GNU GPL its Section 15 shall be amended to the effect that 
 * Ascensio System SIA expressly excludes the warranty of non-infringement of any third-party rights.
 *
 * THIS PROGRAM IS DISTRIBUTED WITHOUT ANY WARRANTY; WITHOUT EVEN THE IMPLIED WARRANTY OF MERCHANTABILITY OR
 * FITNESS FOR A PARTICULAR PURPOSE. For more details, see GNU GPL at https://www.gnu.org/copyleft/gpl.html
 *
 * You can contact Ascensio System SIA by email at sales@onlyoffice.com
 *
 * The interactive user interfaces in modified source and object code versions of ONLYOFFICE must display 
 * Appropriate Legal Notices, as required under Section 5 of the GNU GPL version 3.
 *
 * Pursuant to Section 7 § 3(b) of the GNU GPL you must retain the original ONLYOFFICE logo which contains 
 * relevant author attributions when distributing the software. If the display of the logo in its graphic 
 * form is not reasonably feasible for technical reasons, you must include the words "Powered by ONLYOFFICE" 
 * in every copy of the program you distribute. 
 * Pursuant to Section 7 § 3(e) we decline to grant you any rights under trademark law for use of our trademarks.
 *
*/


using ASC.Web.Core.Files;
using ASC.Web.Files.Classes;
using ASC.Web.Files.Utils;
using ASC.Web.Studio.Utility;
using CsvHelper;
using CsvHelper.Configuration;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ASC.AuditTrail
{
    public static class AuditReportCreator
    {
        private static readonly ILog Log = LogManager.GetLogger("ASC.Messaging");

        public static string CreateCsvReport(IEnumerable<LoginEvent> events, string reportName)
        {
            try
            {
                using (var stream = new MemoryStream())
                using (var writer = new StreamWriter(stream, Encoding.UTF8))
                using (var csv = new CsvWriter(writer))
                {
                    csv.Configuration.RegisterClassMap<LoginEventMap>();

                    csv.WriteHeader<LoginEvent>();
                    foreach (var evt in events)
                    {
                        csv.WriteRecord(evt);
                    }

                    writer.Flush();

                    var file = FileUploader.Exec(Global.FolderMy.ToString(), reportName, stream.Length, stream, true);
                    var fileUrl = CommonLinkUtility.GetFullAbsolutePath(FilesLinkUtility.GetFileWebEditorUrl((int) file.ID));

                    fileUrl += string.Format("&options={{\"delimiter\":{0},\"codePage\":{1}}}",
                                             (int) FileUtility.CsvDelimiter.Comma,
                                             Encoding.UTF8.CodePage);
                    return fileUrl;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error while generating login report: " + ex);
                throw;
            }
        }

        internal class LoginEventMap : CsvClassMap<LoginEvent>
        {
            public override void CreateMap()
            {
                Map(m => m.IP).Name(AuditReportResource.IpCol);
                Map(m => m.Browser).Name(AuditReportResource.BrowserCol);
                Map(m => m.Platform).Name(AuditReportResource.PlatformCol);
                Map(m => m.Date).Name(AuditReportResource.DateCol);
                Map(m => m.UserName).Name(AuditReportResource.UserCol);
                Map(m => m.Page).Name(AuditReportResource.PageCol);
                Map(m => m.ActionText).Name(AuditReportResource.ActionCol);
            }
        }

        public static string CreateCsvReport(IEnumerable<AuditEvent> events, string reportName)
        {
            try
            {
                using (var stream = new MemoryStream())
                using (var writer = new StreamWriter(stream))
                using (var csv = new CsvWriter(writer))
                {
                    csv.Configuration.RegisterClassMap<AuditEventMap>();

                    csv.WriteHeader<AuditEvent>();
                    foreach (var evt in events)
                    {
                        csv.WriteRecord(evt);
                    }

                    writer.Flush();

                    var file = FileUploader.Exec(Global.FolderMy.ToString(), reportName, stream.Length, stream, true);
                    var fileUrl = CommonLinkUtility.GetFullAbsolutePath(FilesLinkUtility.GetFileWebEditorUrl((int) file.ID));

                    fileUrl += string.Format("&options={{\"delimiter\":{0},\"codePage\":{1}}}",
                                             (int) FileUtility.CsvDelimiter.Comma,
                                             Encoding.UTF8.CodePage);
                    return fileUrl;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error while generating audit report: " + ex);
                throw;
            }
        }

        internal class AuditEventMap : CsvClassMap<AuditEvent>
        {
            public override void CreateMap()
            {
                Map(m => m.IP).Name(AuditReportResource.IpCol);
                Map(m => m.Browser).Name(AuditReportResource.BrowserCol);
                Map(m => m.Platform).Name(AuditReportResource.PlatformCol);
                Map(m => m.Date).Name(AuditReportResource.DateCol);
                Map(m => m.UserName).Name(AuditReportResource.UserCol);
                Map(m => m.Page).Name(AuditReportResource.PageCol);
                Map(m => m.ActionTypeText).Name(AuditReportResource.ActionTypeCol);
                Map(m => m.ActionText).Name(AuditReportResource.ActionCol);
                Map(m => m.Product).Name(AuditReportResource.ProductCol);
                Map(m => m.Module).Name(AuditReportResource.ModuleCol);
                Map(m => m.Action).Name(AuditReportResource.ActionIdCol);
                Map(m => m.Target).Name(AuditReportResource.TargetIdCol);
            }
        }
    }
}