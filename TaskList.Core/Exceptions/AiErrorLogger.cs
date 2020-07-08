using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ApplicationInsights;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace TaskList.Core.Exceptions
{
    public static class AiErrorLogger
    {
        private static readonly TelemetryClient Telemetry = new TelemetryClient { InstrumentationKey = AppSettings.APPINSIGHTS_INSTRUMENTATIONKEY };

        public static void LogException(Exception ex)
        {
            if (ex == null)
            {
                return;
            }

            Telemetry.TrackException(ex, GetExceptionDetails(ex), null);
        }

        private static Dictionary<string, string> GetExceptionDetails(Exception ex)
        {
            var exceptionDetails = new Dictionary<string, string>();
            //GetDbEntityValidationExceptionDetails(ex, exceptionDetails);
            GetDbUpdateExceptionDetails(ex, exceptionDetails);

            return exceptionDetails;
        }

        private static void GetDbUpdateExceptionDetails(Exception exception, Dictionary<string, string> exceptionDetails)
        {
            if (!(exception is DbUpdateException)
                || !(exception.InnerException is DbUpdateException)
                || !(exception.InnerException.InnerException is SqlException))
            {
                return;
            }

            // try to decode inner Update Exception
            var sqlException = (SqlException)exception.InnerException.InnerException;
            for (var i = 0; i < sqlException.Errors.Count; i++)
            {
                exceptionDetails.Add("Index #" + i, sqlException.Errors[i].ToString());
            }
        }

        //private static void GetDbEntityValidationExceptionDetails(Exception exception, Dictionary<string, string> exceptionDetails)
        //{
        //    if (!(exception is DbEntityValidationException))
        //    {
        //        return;
        //    }

        //    var ex = (DbEntityValidationException)exception;
        //    var exceptionString = new StringBuilder();
        //    foreach (var eve in ex.EntityValidationErrors)
        //    {
        //        exceptionString.Append($"Entity of type \"");
        //        exceptionString.Append(eve.Entry.Entity.GetType().Name);
        //        exceptionString.Append(" in state \"{eve.Entry.State}\" has the following validation errors: /n --------- ");

        //        foreach (var ve in eve.ValidationErrors)
        //        {
        //            exceptionString.Append(" /n --------- Property: \"");
        //            exceptionString.Append(ve.PropertyName);
        //            exceptionString.Append("\", Error: \"");
        //            exceptionString.Append(ve.ErrorMessage);
        //            exceptionString.Append("\"");
        //        }
        //    }

        //    exceptionDetails.Add("Entity-Framework-DbEntityValidationException", exceptionString.ToString());
        //}
    }
}
