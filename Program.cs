#define stress_test

using Log = dbj.Kontalog.Kontalog;

Log.info("Hello, KONTALOG logging Q!");

#if stress_test
Log.info("KONTALOG stress test will attempt to run forever.");

long counter = 0L;
while (true)
{
    counter = 1 + (counter % long.MaxValue);
    // in prod only fatal will be shown
    Log.fatal("Counter: {0,12} !", counter);
}
#else
for (System.Int32 k = 0; k < 0xF; ++k)
    Log.debug("Hello, {0,12} !", k);
Log.fatal("Loop (size = {0}) done.", 0xF);

Log.info("Done. Press ENTER");
Console.ReadLine();
#endif

// WARNING: Console.Writeline will be out of sync, if used 
