

### Execute
Begynd med at v�lge at k�re koden som selfhosting (alts� ikke IIS Express)
V�lg Properties af Solution og v�lg: Multiple Startup Projects.
 s�t r�kkef�lgen (Start) til:
- AuthorizationServer
- RewardsApi
- Client.M?

Start processen op ved at klikke p� den gr�nne pil.



### Fiddler
Filter s�ttes s�ledes op:
- Filters | Use Filters checked
- Show only the folloving Hosts: localhost:5000; localhost:5001; localhost:5002;
- Hid if URL contains: lib logo css favicon

Husk: Clear Cache.

