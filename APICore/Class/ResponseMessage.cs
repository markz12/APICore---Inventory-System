using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICore.Class
{
    public class ResponseMessage
    {
        public static string StandardMessage(int code)
        {
            string message = string.Empty;
            switch (code)
            {
                case 100:
                    message = "Continue";
                    break;
                case 200:
                    message = "Success";
                    break;
                case 201:
                    message = "Created";
                    break;
                case 202:
                    message =  "Accepted";
                    break;
                case 300:
                    message = "Parameter is Null";
                    break;
                case 400:
                    message = "Bad Request";
                    break;
                case 401:
                    message = "UnAuthorized";
                    break;
                case 403:
                    message = "Forbidden";
                    break;
                case 404:
                    message = "No record found";
                    break;
                case 409:
                    message = "Duplicate";
                    break;
                case 500:
                    message = "Internal Server Error";
                    break;
                case 0:
                    message = "Query Error";
                    break;
                default:
                    message = "Unidentified";
                    break;
            }
            return message;
        }
    }
}
