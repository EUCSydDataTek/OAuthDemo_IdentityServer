

### Execute
Begynd med at vælge at køre koden som selfhosting (altså ikke IIS Express)
Vælg Properties af Solution og vælg: Multiple Startup Projects.
 sæt rækkefølgen (Start) til:
- AuthorizationServer (port 5000)
- RewardsApi (port 5002)
- Client.M?


Start processen op ved at klikke på den grønne pil.

Log på med følgende credentials:


> Username: scott
> 
> Password: scott

&nbsp;

### Fiddler
Filter sættes således op:
- Filters | Use Filters checked
- Show only the folloving Hosts: localhost:5000; localhost:5001; localhost:5002;
- Hide if URL contains: lib logo css favicon

Husk: Clear Cache.

> Authentication Server: Port 5000
> 
> Client.M?.Application: Port 5001
> 
> RewardsApi: Port 5002

&nbsp;

#### Client.M2.Simple

![Client.M2.Simple](images/Client.M2.Simple.png)