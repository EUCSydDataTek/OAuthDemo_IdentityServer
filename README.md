

### Execute
Begynd med at konfigurere opstarten af de 3 projekter. Det er nemmerst at g�re ved at v�lge Properties p� hvert enkelt
projekt og g� ind p� fanen Debug.
Der skal laves f�lgende:
1. V�lg at k�re koden som selfhosting (alts� ikke IIS Express). Det sker ved under **Profile** v�lge projektets navn og under **Launch** at v�lge *Project*.
2. �ndre App URL til f�lgende:
   a. AuthorizationServer: https://localhost:5000
   b. Client.M3.AuthorizationCode: https://localhost:5001
   c. RewardsApi: https://localhost:5002
3. Kun Clienten skal have et flueben ved Launch browser. Hverken Authorizationserver eller RewardsApi bruger en webside.

![Project Config](images/ProjectConfig.jpg)

4. Til sidst v�lges *Properties* af Solution og v�lg: *Multiple Startup Projects*. S�t r�kkef�lgen (Start) til:
- AuthorizationServer
- RewardsApi
- Client.M3.AuthorizationCode

![Multiple Startup](images/MultipleStartup.jpg)

5. Start processen op ved at klikke p� den gr�nne pil (Debug).

6. Log p� med f�lgende credentials:


> Username: scott
> 
> Password: scott

&nbsp;

### Fiddler
Filter s�ttes s�ledes op:
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