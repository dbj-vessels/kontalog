# WIP

## Kontalog&trade; 

Kontalog&trade; means Container Logger. And it is queued. It is async logging. Do not attack it, just use it.

<!-- 
https://stackoverflow.com/a/3670628
```c#
using System.Collections.Concurrent;
``` -->

When in a container, for logging, code need only write to container logs. Over STDOUT/STDERR. No need to "sync into" anything else but console.
