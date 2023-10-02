// See https://aka.ms/new-console-template for more information

using Kontilog  = dbj.Kontalog.Kontalog;

Kontilog.error("Hello, World!");

for (short k = 0; k < 0xFFF; ++k)
    Kontilog.info("Hello, {0} !", k);

Kontilog.fatal("I am off!");
