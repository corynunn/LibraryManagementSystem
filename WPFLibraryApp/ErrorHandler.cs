using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFLibraryApp
{
    public static class ErrorHandler
    {
        
        /// <summary>
        /// Finds the innermost exception and returns the message.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string InnermostExceptionMessage(Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex.Message;
        }
    }
}
