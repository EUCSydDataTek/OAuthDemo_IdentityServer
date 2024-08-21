using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Pluralsight.AuthorizationServer
{
    [SecurityHeaders]
    [Authorize]
    public class ConsentController : Controller
    {
        private readonly IIdentityServerInteractionService interaction;
        private readonly IClientStore clientStore;
        private readonly IResourceStore resourceStore;
        private readonly IEventService events;
        private readonly ILogger<ConsentController> logger;

        public ConsentController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IResourceStore resourceStore,
            IEventService events, 
            ILogger<ConsentController> logger)
        {
            this.interaction = interaction;
            this.clientStore = clientStore;
            this.resourceStore = resourceStore;
            this.events = events;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            var vm = await BuildViewModelAsync(returnUrl);
            if (vm != null)
            {
                return View("Index", vm);
            }

            return View("Error");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ConsentInputModel model)
        {
            var result = await ProcessConsent(model);

            if (result.IsRedirect)
            {
                if (await clientStore.IsPkceClientAsync(result.ClientId))
                {
                    return View("Redirect", new RedirectViewModel { RedirectUrl = result.RedirectUri });
                }

                return Redirect(result.RedirectUri);
            }

            if (result.HasValidationError)
            {
                ModelState.AddModelError("", result.ValidationError);
            }

            if (result.ShowView)
            {
                return View("Index", result.ViewModel);
            }

            return View("Error");
        }


        private async Task<ProcessConsentResult> ProcessConsent(ConsentInputModel model)
        {
            var result = new ProcessConsentResult();

            var request = await interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            if (request == null) return result;

            ConsentResponse grantedConsent = null;

            if (model.Button == "no")
            {
                await events.RaiseAsync(new ConsentDeniedEvent(User.GetSubjectId(), result.ClientId, request.ValidatedResources.InvalidScopes));
            }
            else if (model.Button == "yes")
            {
                if (model.ScopesConsented != null && model.ScopesConsented.Any())
                {
                    var scopes = model.ScopesConsented;
                    
                    grantedConsent = new ConsentResponse
                    {
                        RememberConsent = model.RememberConsent,
                        ScopesValuesConsented = scopes.ToArray()
                    };

                    await events.RaiseAsync(new ConsentGrantedEvent(User.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.Resources.ApiScopes.Select(s => s.Name), grantedConsent.ScopesValuesConsented, grantedConsent.RememberConsent));
                }
                else
                {
                    result.ValidationError = "You must pick at least one permission";
                }
            }
            else
            {
                result.ValidationError = "Invalid selection";
            }

            if (grantedConsent != null)
            {
                await interaction.GrantConsentAsync(request, grantedConsent);

                result.RedirectUri = model.ReturnUrl;
                result.ClientId = request.Client.ClientId;
            }
            else
            {
                result.ViewModel = await BuildViewModelAsync(model.ReturnUrl, model);
            }

            return result;
        }

        private async Task<ConsentViewModel> BuildViewModelAsync(string returnUrl, ConsentInputModel model = null)
        {
            var request = await interaction.GetAuthorizationContextAsync(returnUrl);
            if (request != null)
            {
                var client = await clientStore.FindEnabledClientByIdAsync(request.Client.ClientId);
                if (client != null)
                {
                    var resources = await resourceStore.FindEnabledResourcesByScopeAsync(request.ValidatedResources.Resources.ApiScopes.Select(s => s.Name));
                    if (resources != null && (resources.IdentityResources.Any() || resources.ApiResources.Any()))
                    {
                        return CreateConsentViewModel(model, returnUrl, client, resources);
                    }

                    logger.LogError("No scopes matching: {0}", request.ValidatedResources.Resources.ApiScopes.Select(s => s.Name).Aggregate((x, y) => x + ", " + y));
                }
                else
                {
                    logger.LogError("Invalid client id: {0}", request.Client.ClientId);
                }
            }
            else
            {
                logger.LogError("No consent request matching request: {0}", returnUrl);
            }

            return null;
        }

        private static ConsentViewModel CreateConsentViewModel(ConsentInputModel model, string returnUrl, Client client, Resources resources)
        {
            var vm = new ConsentViewModel
            {
                RememberConsent = model?.RememberConsent ?? true,
                ScopesConsented = model?.ScopesConsented ?? Enumerable.Empty<string>(),

                ReturnUrl = returnUrl,

                ClientName = client.ClientName ?? client.ClientId,
                ClientUrl = client.ClientUri,
                ClientLogoUrl = client.LogoUri,
                AllowRememberConsent = client.AllowRememberConsent
            };

            vm.IdentityScopes = resources.IdentityResources.Select(x => CreateScopeViewModel(x, vm.ScopesConsented.Contains(x.Name) || model == null)).ToArray();
            vm.ResourceScopes = resources.ApiResources.SelectMany(x => x.Scopes).Select(x => CreateScopeViewModel(resources.FindIdentityResourcesByScope(x), vm.ScopesConsented.Contains(x) || model == null)).ToArray();
            if (resources.OfflineAccess)
            {
                vm.ResourceScopes = vm.ResourceScopes.Union(new[] {
                    GetOfflineAccessScope(vm.ScopesConsented.Contains(Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess) || model == null)
                });
            }

            return vm;
        }

        private static ScopeViewModel CreateScopeViewModel(IdentityResource identity, bool check)
        {
            return new ScopeViewModel
            {
                Name = identity.Name,
                DisplayName = identity.DisplayName,
                Description = identity.Description,
                Emphasize = identity.Emphasize,
                Required = identity.Required,
                Checked = check || identity.Required
            };
        }

        private static ScopeViewModel CreateScopeViewModel(ApiScope scope, bool check)
        {
            return new ScopeViewModel
            {
                Name = scope.Name,
                DisplayName = scope.DisplayName,
                Description = scope.Description,
                Emphasize = scope.Emphasize,
                Required = scope.Required,
                Checked = check || scope.Required
            };
        }

        private static ScopeViewModel GetOfflineAccessScope(bool check)
        {
            return new ScopeViewModel
            {
                Name = Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess,
                DisplayName = "Offline Access",
                Description = "Access to your applications and resources, even when you are offline",
                Emphasize = true,
                Checked = check
            };
        }
    }
}