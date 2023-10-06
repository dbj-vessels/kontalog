// See https://aka.ms/new-console-template for more information

using Kontilog  = dbj.Kontalog.Kontalog;

Kontilog.error("Hello, World!");

for (System.Int32 k = 0; k < 255; ++k)
    Kontilog.info("Hello, {0,4} !", k);

Kontilog.fatal("I am off!");

Console.WriteLine("Done");
Console.ReadLine();
