// See https://aka.ms/new-console-template for more information

using Log  = dbj.Kontalog.Kontalog;

Log.error("Hello, KONTALOG logging Q!");

for (System.Int32 k = 0; k < 0xF; ++k)
    Log.info("Hello, {0,4} !", k);

Log.fatal("I am off!");

Log.info("Done");
Console.ReadLine();

// WARNING: Console.Writeline will be out of sync, if used 
