

### Execute
Begynd med at v�lge at k�re koden som selfhosting (alts� ikke IIS Express)
V�lg Properties af Solution og v�lg: Multiple Startup Projects.
 s�t r�kkef�lgen (Start) til:
- AuthorizationServer
- RewardsApi
- Client.M?

Start processen op ved at klikke p� den gr�nne pil.

Log p� med f�lgende credentials:


> Username: scott
> 
> Password: scott


#### UWP
Her skal man start IdentityServer og Api f�rst med Ctrl+F5.
Derefter skal man s�tte UWP projektet til Startup og starte det med Ctrl+F5.

### Fiddler
Filter s�ttes s�ledes op:
- Filters | Use Filters checked
- Show only the folloving Hosts: localhost:5000; localhost:5001; localhost:5002;
- Hide if URL contains: lib logo css favicon

Husk: Clear Cache.

> Authentication Server: Port 5000
> 
> Client Application: Port 5001

&nbsp;

## Client.M2.Simple

![Client.M2.Simple](images/Client.M2.Simple.png)