namespace MyDownload
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Form1 form1 = new Form1();
            Console.SetOut(new ConsoleWriter(form1.AddMessage, form1.ShowMessage));
            Application.Run(form1);

        }
    }
}