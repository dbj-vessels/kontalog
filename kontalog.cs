#define LOCAL_ISO_8601

// using System;
using System.Runtime.CompilerServices;
using System.Collections.Concurrent;
// using System.Threading;
// using System.Threading.Tasks;



#if KONTALOG_HANDLES_SQLSVR_CLIENT_EXCEPTION
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
// need Microsoft.Data.SqlClient.SqlException
#endif

#region queued_console

namespace dbj.Kontalog;

// Kontalog means Kontainer Logger
// and it is queued. But do not attack it, just use it please.
//
// https://stackoverflow.com/a/3670628
// using System.Collections.Concurrent;
//
// When in a container, for logging, code need only write to STDOUT
//

public static class Kontalog
{
#if LOCAL_ISO_8601
    // Console.WriteLine("Hello my UTC timestamp      : " + iso8601(1));
    // Console.WriteLine("Hello iso8601 with 'T'      : " + iso8601(2));
    // Console.WriteLine("Hello full iso8601          : " + iso8601(3));
    // Console.WriteLine("Hello  my local timestamp   : " + iso8601());
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string iso8601(short kind_ = 0)
    {
        switch (kind_)
        {
            case 1:
                // no 'T' fromat e.g. "2023-07-15 05:56:27"
                return DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            case 2:
                // official 'T' format e.g.  2023-07-15T07:56:27
                return DateTime.Now.ToLocalTime().ToString("s", System.Globalization.CultureInfo.InvariantCulture);
            case 3:
                // a sortable date/time pattern; conforms to ISO 8601.
                return DateTime.Now.ToLocalTime().ToString("o", System.Globalization.CultureInfo.InvariantCulture);
            default:
                // default e.g. 2023-07-15 07:56:27
                return DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
#endif

    private static readonly int Queue_size = 0xFF;
    // "blocking" means "wait until the operation can be completed"
    // Note that blocking on maximum capacity is only enabled 
    // when the BlockingCollection 
    // has been created with a maximum capacity specified in the constructor
    private static BlockingCollection<string> Queue_
    = new BlockingCollection<string>(Queue_size);

    static Kontalog()
    {
        // this code works, unfortunately  ;)
        // WARNING: Console.Writeline will be out of sync, if used in a program
        var thread = new Thread(
          () =>
          {
              // When in a container, for logging, code need only write to STDOUT
              // while (true) System.Console.WriteLine(Queue_.Take());
              while (!Queue_.IsCompleted)
              {

                  string data = string.Empty;
                  // https://learn.microsoft.com/en-us/dotnet/standard/collections/thread-safe/blockingcollection-overview
                  try
                  {
                      data = Queue_.Take();
                  }
                  catch (InvalidOperationException) { }

                  if (data != string.Empty)
                  {
                      System.Console.WriteLine(data);
                  }

                  // Slow down consumer just a little to cause
                  // collection to fill up faster, and lead to "AddBlocked"
                  const int spin_wait = 0xFFFFF;
                  Thread.SpinWait(spin_wait);
              }
          });
        thread.IsBackground = true;
        thread.Start();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add(string format, params object[] args)
    {
        if (args.Length < 1)
            Queue_.Add(format);
        else
        {
            Queue_.Add(string.Format(format, args));
        }
    }

#if KONTALOG_HANDLES_SQLSVR_CLIENT_EXCEPTION
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(Microsoft.Data.SqlClient.SqlException sqx)
        {
            Queue_.Add(
            string.Format("[" + iso8601() + "]" + "SQL SRV name: {0}\nSQL Exception code: {1}\nmessage: {2}", sqx.Server, sqx.ErrorCode, sqx.Message));
        }
#endif
    // and  now lets turn this into the 'logging lib'
    public enum Level
    {
        fatal, error, debug, info
    }

    // there are no levels ordering in here
    // if in Production only fatal messages will be logged
    public static bool Production
    {
        get
        {
#if DEBUG
            return false;
#else
                return true;
#endif
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void log_(Level lvl_, string format, params object[] args)
    {
        // if in Production only fatal messages will be logged
        if (Production)
        {
            if (lvl_ > Level.fatal) return;
        }

        var prefix_ = "[" + iso8601() + "|" + lvl_.ToString() + "]";

        if (args.Length < 1)
            Queue_.Add(prefix_ + format);
        else
        {
            Queue_.Add(prefix_ + string.Format(format, args));
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void fatal(string format, params object[] args)
    {
        log_(Level.fatal, format, args);
    }

    // error and debug work when fatal or info are not the levels
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void error(string format, params object[] args)
    {
        // if in Production only fatal messages will be logged
        if (Production) return;
        log_(Level.error, format, args);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void debug(string format, params object[] args)
    {
        // if in Production only fatal messages will be logged
        if (Production) return;
        log_(Level.debug, format, args);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void info(string format, params object[] args)
    {
        // if in Production only fatal messages will be logged
        if (Production) return;
        log_(Level.info, format, args);
    }
}



#endregion queued_console