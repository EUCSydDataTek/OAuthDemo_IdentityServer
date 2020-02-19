

### Execute
Begynd med at vælge at køre koden som selfhosting (altså ikke IIS Express)
Vælg Properties af Solution og vælg: Multiple Startup Projects.
 sæt rækkefølgen (Start) til:
- AuthorizationServer
- RewardsApi
- Client.M?

Start processen op ved at klikke på den grønne pil.

#### UWP
Her skal man start IdentityServer og Api først med Ctrl+F5.
Derefter skal man sætte UWP projektet til Startup og starte det med Ctrl+F5.

### Fiddler
Filter sættes således op:
- Filters | Use Filters checked
- Show only the folloving Hosts: localhost:5000; localhost:5001; localhost:5002;
- Hid if URL contains: lib logo css favicon

Husk: Clear Cache.

