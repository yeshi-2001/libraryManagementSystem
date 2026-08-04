using System.Web;

namespace libraryManagementSystem.Utils
{

    public static class ToastHelper
    {
        private const string MessageKey = "ToastMessage";
        private const string TypeKey = "ToastType";

        /// <summary>Queues a toast. type should be 'success' | 'info'.</summary>
        public static void QueueToast(string message, string type = "success")
        {
            HttpContext.Current.Session[MessageKey] = message;
            HttpContext.Current.Session[TypeKey] = type;
        }


        public static (string Message, string Type) ConsumeQueuedToast()
        {
            string message = HttpContext.Current.Session[MessageKey] as string;
            string type = HttpContext.Current.Session[TypeKey] as string;

            HttpContext.Current.Session.Remove(MessageKey);
            HttpContext.Current.Session.Remove(TypeKey);

            return (message, type);
        }
    }
}