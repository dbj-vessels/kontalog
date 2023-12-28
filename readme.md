# Kontalog&trade; 

Kontalog&trade; means Container Logger. And it is queued. It is async logging. But do not attack it, just use it.

> Caveat Emptor: Code in a constructor is not my idea. But. I do not like it much. How long will it run without a restart? Will it overflow and when? Comprehensive UT is required.

```
https://stackoverflow.com/a/3670628
```
<!-- ```c#
using System.Collections.Concurrent;
``` -->

When in a container, for logging, code need only write to STDOUT. No need to "sync into" anything else but console.